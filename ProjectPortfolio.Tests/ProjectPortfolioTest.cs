using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;
using System.ComponentModel.DataAnnotations;

namespace ProjectPortfolio.Tests
{
    public class ProjectPortfolioTest
    {
        private static List<ValidationResult> ValidateModel(SystemUserModel model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, serviceProvider: null, items: null);
            Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true);
            return validationResults;
        }

        #region SystemUser
        [Fact(DisplayName = "Nome não pode ser nulo ou vazio")]
        public void ShouldHaveValidationErrors_WhenNameIsEmpty()
        {
            var model = new SystemUserModel
            {
                Name = "",
                Surname = "Sobrenome",
                UserName = "username123",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager,
                DateCreated = DateTimeOffset.Now
            };

            var validationResults = ValidateModel(model);

            Assert.Contains(validationResults, r => r.ErrorMessage == "O campo Nome é obrigatório.");
        }

        [Fact(DisplayName = "Sobrenome deve ter entre 3 e 100 caracteres")]
        public void ShouldHave_ValidationErrors_WhenSurnameIsTooShort()
        {
            var model = new SystemUserModel
            {
                Name = "Nome",
                Surname = "So", 
                UserName = "username123",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager,
                DateCreated = DateTimeOffset.Now
            };

            var validationResults = ValidateModel(model);

            Assert.Contains(validationResults, r => r.ErrorMessage == "O sobrenome deve ter entre 3 e 100 caracteres.");
        }

        [Fact(DisplayName = "Deve retornar o nome completo concatenado com nome e sobrenome")]
        public void Should_Return_DisplayName_As_Concatenated_NameAnd_Surname()
        {
            var model = new SystemUserModel
            {
                Name = "Nome",
                Surname = "Sobrenome",
                UserName = "username123",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager,
                DateCreated = DateTimeOffset.Now
            };

            var displayName = model.DisplayName;

            Assert.Equal("Nome Sobrenome", displayName);
        }

        [Fact(DisplayName = "Login deve ter entre 3 e 50 caracteres")]
        public void ShouldHave_ValidationErrors_WhenUserNameIsTooShort()
        {
            var model = new SystemUserModel
            {
                Name = "Nome",
                Surname = "Sobrenome",
                UserName = "us", 
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager,
                DateCreated = DateTimeOffset.Now
            };

            var validationResults = ValidateModel(model);

            Assert.Contains(validationResults, r => r.ErrorMessage == "O login deve ter entre 3 e 50 caracteres.");
        }

        [Fact(DisplayName = "Senha não pode ser nula ou vazia")]
        public void ShouldHave_ValidationErrors_WhenPasswordIsEmpty()
        {
            var model = new SystemUserModel
            {
                Name = "Nome",
                Surname = "Sobrenome",
                UserName = "username123",
                Password = "", 
                BusinessRole = BusinessRoleEnum.manager,
                DateCreated = DateTimeOffset.Now
            };

            var validationResults = ValidateModel(model);

            Assert.Contains(validationResults, r => r.ErrorMessage == "O campo Senha é obrigatório.");
        }

        [Fact(DisplayName = "Deve criar um novo usuário com senha criptografada")]
        public async Task CreateAsync_ShouldHashPasswordAndCreateUser()
        {
            var systemUserMockRepository = new Mock<ISystemUserRepository>();
            var issueMockRepository = new Mock<IIssueRepository>();
            var service = new SystemUserService(systemUserMockRepository.Object, issueMockRepository.Object);

            var model = new SystemUserModel
            {
                Name = "Nome",
                Surname = "Sobrenome",
                UserName = "username123",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager
            };

            systemUserMockRepository.Setup(r => r.InsertAsync(It.IsAny<SystemUserModel>())).ReturnsAsync(model);

            var result = await service.CreateAsync(model);

            Assert.Equal(model, result);
            Assert.NotNull(result.Password); 
            Assert.NotEqual("password123", result.Password);  
            Assert.Equal(model.DateCreated.Date, DateTimeOffset.Now.Date);
        }

        //[Fact(DisplayName = "Deve lançar exceção se o usuário estiver vinculado a atividades ativas")]
        //public async Task DeleteAsync_Should_Throw_Exception_When_User_Has_Active_Issues()
        //{
        //    var mockRepository = new Mock<ISystemUserRepository>();
        //    var mockIssueRepository = new Mock<IIssueRepository>();
        //    var service = new SystemUserService(mockRepository.Object, mockIssueRepository.Object);

        //    var userId = Guid.NewGuid();
        //    var issues = new List<IssueModel>
        //    {
        //        new IssueModel { AttendantId = userId, Status = IssueStatusEnum.Opened }  
        //    };

        //    mockIssueRepository.Setup(r => r.GetAll()).ReturnsAsync(issues);

        //    var exception = await Assert.ThrowsAsync<Exception>(() => service.DeleteAsync(userId));
        //    Assert.Equal("Usuário está vinculado a atividade ativa e não pode ser deletado.", exception.Message);
        //}

        [Fact(DisplayName = "Deve autenticar o usuário corretamente")]
        public async Task AuthenticateAsync_ShouldReturnTrue_WhenPasswordIsValid()
        {
            var mockRepository = new Mock<ISystemUserRepository>();
            var mockIssueRepository = new Mock<IIssueRepository>();
            var service = new SystemUserService(mockRepository.Object, mockIssueRepository.Object);

            var user = new SystemUserModel
            {
                UserName = "username123",
                Password = BCrypt.Net.BCrypt.HashPassword("password123") 
            };

            mockRepository.Setup(r => r.GetUserByUserName("username123")).ReturnsAsync(user);

            var result = await service.AuthenticateAsync("username123", "password123");

            Assert.True(result); 
        }

        [Fact(DisplayName = "Não deve autenticar o usuário com senha inválida")]
        public async Task AuthenticateAsync_ShouldReturnFalse_WhenPasswordIsInvalid()
        {
            var mockRepository = new Mock<ISystemUserRepository>();
            var mockIssueRepository = new Mock<IIssueRepository>();
            var service = new SystemUserService(mockRepository.Object, mockIssueRepository.Object);

            var user = new SystemUserModel
            {
                UserName = "username123",
                Password = BCrypt.Net.BCrypt.HashPassword("password123") 
            };

            mockRepository.Setup(r => r.GetUserByUserName("username123")).ReturnsAsync(user);

            var result = await service.AuthenticateAsync("username123", "wrongpassword");

            Assert.False(result);  
        }
        #endregion


        #region Client
        [Fact(DisplayName = "CPF válido")]
        public void GivenValidCPFNumbers()
        {
            const string cpf = "12345678909";
            const string expectedResult = "123.456.789-09";

            var client = new ClientModel();

            client.CPF = cpf;
            var actualResult = client.cpfformatado;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "CPF inválido")]
        public void GivenInvalidCPFNumbers()
        {
            const string cpf = "123";

            var client = new ClientModel();

            var exception = Assert.Throws<ArgumentException>(() => client.CPF = cpf);
            Assert.Equal("CPF inválido.", exception.Message);
        }

        [Fact(DisplayName = "CNPJ válido")]
        public void GivenValidCNPJNumbers()
        {
            const string cnpj = "12345678000195";
            const string expectedResult = "12.345.678/0001-95";

            var client = new ClientModel();

            client.CNPJ = cnpj;
            var actualFormattedCNPJ = client.cnpjformatado;

            Assert.Equal(expectedResult, actualFormattedCNPJ);
        }

        [Fact(DisplayName = "CNPJ inválido")]
        public void GivenInvalidCNPJNumbers()
        {
            const string cnpj = "123";

            var client = new ClientModel();

            var exception = Assert.Throws<ArgumentException>(() => client.CNPJ = cnpj);
            Assert.Equal("CNPJ inválido.", exception.Message);
        }

        //[Fact(DisplayName = "Deve validar o campo 'Name' como obrigatório e com comprimento entre 3 e 100 caracteres")]
        //public void ClientModel_NameValidation_ShouldBeValid()
        //{
        //    var client = new ClientModel();

        //    var validationContext = new ValidationContext(client);
        //    var validationResults = new List<ValidationResult>();

        //    var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.False(isValid); 
        //    Assert.Contains(validationResults, v => v.ErrorMessage == "O nome é obrigatório.");

        //    client.Name = "John";

        //    isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.True(isValid); 
        //}

        //[Fact(DisplayName = "Deve validar o campo 'ZipCode' como obrigatório e com 8 caracteres")]
        //public void ClientModel_ZipCodeValidation_ShouldBeValid()
        //{
        //    var client = new ClientModel();

        //    var validationContext = new ValidationContext(client);
        //    var validationResults = new List<ValidationResult>();

        //    var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.False(isValid); 
        //    Assert.Contains(validationResults, v => v.ErrorMessage == "O CEP é obrigatório.");

        //    client.ZipCode = "12345678"; 

        //    isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.True(isValid);  
        //}

        //[Fact(DisplayName = "Deve validar o campo 'Address' como obrigatório")]
        //public void ClientModel_AddressValidation_ShouldBeValid()
        //{
        //    var client = new ClientModel();

        //    var validationContext = new ValidationContext(client);
        //    var validationResults = new List<ValidationResult>();

        //    var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.False(isValid);  
        //    Assert.Contains(validationResults, v => v.ErrorMessage == "O endereço é obrigatório.");

        //    client.Address = "Rua Exemplo, 123";  

        //    isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.True(isValid); 
        //}

        //[Fact(DisplayName = "Deve validar o campo 'City' como obrigatório")]
        //public void ClientModel_CityValidation_ShouldBeValid()
        //{
        //    var client = new ClientModel();

        //    var validationContext = new ValidationContext(client);
        //    var validationResults = new List<ValidationResult>();

        //    var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.False(isValid);  
        //    Assert.Contains(validationResults, v => v.ErrorMessage == "A cidade é obrigatória.");

        //    client.City = "São Paulo";  
        //    isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);
        //    Assert.True(isValid); 
        //}

        //[Fact(DisplayName = "Deve validar o campo 'PhoneNumber' com formato correto, caso seja informado")]
        //public void ClientModel_PhoneNumberValidation_ShouldBeValid()
        //{
        //    var client = new ClientModel();
        //    client.PhoneNumber = "123-456-7890";  

        //    var validationContext = new ValidationContext(client);
        //    var validationResults = new List<ValidationResult>();

        //    var isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.True(isValid);  

        //    client.PhoneNumber = "invalid-phone"; 

        //    validationResults.Clear();
        //    isValid = Validator.TryValidateObject(client, validationContext, validationResults, true);

        //    Assert.False(isValid);  
        //}

        //[Fact(DisplayName = "Deve lançar exceção se o número do projeto for inválido")]
        //public async Task ClientProjectService_CreateAsync_Should_Throw_Exception_When_Number_Is_Invalid()
        //{
        //    var project = new ClientProjectModel
        //    {
        //        Address = "Rua Teste",
        //        ZipCode = "12345678",
        //        City = "Cidade Teste",
        //        Title = "Projeto Teste",
        //        Number = 0
        //    };

        //    var mockRepository = new Mock<IClientProjectRepository>();
        //    var service = new ClientProjectService(mockRepository.Object);

        //    var exception = await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(project));
        //    Assert.Contains("O número deve ser um valor válido e maior que zero.", exception.Message);
        //}

        [Fact(DisplayName = "Deve lançar exceção se CPF ou CNPJ não forem informados ao criar um cliente")]
        public async Task ClientService_CreateAsyncShouldThrowException_WhenNoCPFOrCNPJ()
        {
            var client = new ClientModel { Name = "Client Test", ZipCode = "12345678", Address = "Rua Teste", City = "Cidade Teste" };
            var mockRepository = new Mock<IClientRepository>();
            var mockIssueRepository = new Mock<IIssueRepository>();
            var service = new ClientService(mockRepository.Object, mockIssueRepository.Object);

            await Assert.ThrowsAsync<Exception>(() => service.CreateAsync(client));
        }
        #endregion

        #region ClientProjects
        //[Fact(DisplayName = "Deve lançar exceção se campos obrigatórios não forem preenchidos ao criar um projeto")]
        //public async Task ClientProjectService_CreateAsyncShouldThrowException_WhenRequiredFieldsAreMissing()
        //{
        //    var project = new ClientProjectModel { Address = "", ZipCode = "", City = "", Title = "" };
        //    var mockRepository = new Mock<IClientProjectRepository>();
        //    var service = new ClientProjectService(mockRepository.Object);

        //    var exception = await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(project));
        //    Assert.Contains("O endereço é obrigatório.", exception.Message);
        //    Assert.Contains("O CEP é obrigatório.", exception.Message);
        //    Assert.Contains("A cidade é obrigatória.", exception.Message);
        //    Assert.Contains("O título é obrigatório.", exception.Message);
        //}

        //[Fact(DisplayName = "Deve lançar exceção se o título tiver comprimento inválido")]
        //public async Task ClientProjectService_CreateAsync_Should_Throw_Exception_When_Title_Length_Is_Invalid()
        //{
        //    var project = new ClientProjectModel { Address = "Rua Teste", ZipCode = "12345678", City = "Cidade Teste", Title = "A" };
        //    var mockRepository = new Mock<IClientProjectRepository>();
        //    var service = new ClientProjectService(mockRepository.Object);

        //    var exception = await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(project));
        //    Assert.Contains("O título deve ter entre 3 e 50 caracteres.", exception.Message);
        //}

        [Fact(DisplayName = "Deve criar um novo projeto com sucesso")]
        public async Task ClientProjectService_CreateAsync_Should_Create_New_Project_When_Model_Is_Valid()
        {
            var project = new ClientProjectModel
            {
                Address = "Rua Teste",
                ZipCode = "12345678",
                City = "Cidade Teste",
                Title = "Projeto Teste",
                Number = 123,
                ClientId = Guid.NewGuid()
            };

            var mockRepository = new Mock<IClientProjectRepository>();
            mockRepository.Setup(r => r.InsertAsync(It.IsAny<ClientProjectModel>())).ReturnsAsync(project);

            var service = new ClientProjectService(mockRepository.Object);

            var result = await service.CreateAsync(project);

            Assert.NotNull(result);
            Assert.Equal("Rua Teste", result.Address);
            Assert.Equal("Cidade Teste", result.City);
            Assert.Equal("Projeto Teste", result.Title);
            mockRepository.Verify(r => r.InsertAsync(It.Is<ClientProjectModel>(p => p.Title == "Projeto Teste")), Times.Once);
        }

        //[Fact(DisplayName = "Deve lançar exceção se os campos obrigatórios não forem preenchidos")]
        //public async Task ClientProjectService_CreateAsync_Should_Throw_Validation_Exception_When_Required_Fields_Are_Missing()
        //{
        //    var project = new ClientProjectModel
        //    {
        //        Address = "",
        //        ZipCode = "",
        //        City = "",
        //        Title = "",
        //        Number = 0
        //    };

        //    var mockRepository = new Mock<IClientProjectRepository>();
        //    var mockClientRepository = new Mock<IClientRepository>();
        //    var service = new ClientProjectService(mockRepository.Object);

        //    var exception = await Assert.ThrowsAsync<ValidationException>(() => service.CreateAsync(project));
        //    Assert.Contains("O endereço é obrigatório.", exception.Message);
        //    Assert.Contains("O CEP é obrigatório.", exception.Message);
        //    Assert.Contains("A cidade é obrigatória.", exception.Message);
        //    Assert.Contains("O título é obrigatório.", exception.Message);
        //}
        #endregion

        #region Issue
        #endregion
    }
}