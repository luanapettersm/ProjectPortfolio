using Moq;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;
using System.ComponentModel.DataAnnotations;

namespace ProjectPortfolio.Test
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
        [Fact(DisplayName = "DisplayName combina Nome e Sobrenome corretamente")]
        public void DisplayName_ShouldCombineNameAndSurname()
        {
            var user = new SystemUserModel
            {
                Name = "John",
                Surname = "Doe"
            };

            var displayName = user.DisplayName;

            Assert.Equal("John Doe", displayName);
        }

        [Fact(DisplayName = "Validacao falha quando Nome e nulo")]
        public void Validation_ShouldFail_WhenNameIsNull()
        {
            var user = new SystemUserModel
            {
                Name = null,
                Surname = "Doe",
                UserName = "jdoe",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.analyst
            };

            var validationResults = ValidateModel(user);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O campo Nome e obrigatorio.");
        }

        [Fact(DisplayName = "Validacao falha quando Nome esta fora do tamanho permitido")]
        public void Validation_ShouldFail_WhenNameLengthIsInvalid()
        {
            var user = new SystemUserModel
            {
                Name = "Jo", 
                Surname = "Doe",
                UserName = "jdoe",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.analyst
            };

            var validationResults = ValidateModel(user);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O nome deve ter entre 3 e 35 caracteres.");
        }

        [Fact(DisplayName = "Validacao falha quando Sobrenome e nulo")]
        public void Validation_ShouldFail_WhenSurnameIsNull()
        {
            var user = new SystemUserModel
            {
                Name = "John",
                Surname = null,
                UserName = "jdoe",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.analyst
            };

            var validationResults = ValidateModel(user);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O campo Sobrenome e obrigatorio.");
        }

        [Fact(DisplayName = "Validacao falha quando Login e nulo")]
        public void Validation_ShouldFail_WhenUserNameIsNull()
        {
            var user = new SystemUserModel
            {
                Name = "John",
                Surname = "Doe",
                UserName = null,
                Password = "password123",
                BusinessRole = BusinessRoleEnum.analyst
            };

            var validationResults = ValidateModel(user);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O campo Login e obrigatorio.");
        }

        [Fact(DisplayName = "Validacao falha quando Senha e nula")]
        public void Validation_ShouldFail_WhenPasswordIsNull()
        {
            var user = new SystemUserModel
            {
                Name = "John",
                Surname = "Doe",
                UserName = "jdoe",
                Password = null,
                BusinessRole = BusinessRoleEnum.analyst
            };

            var validationResults = ValidateModel(user);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O campo Senha e obrigatorio.");
        }

        [Fact(DisplayName = "Nome nao pode ser nulo ou vazio")]
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

            Assert.Contains(validationResults, r => r.ErrorMessage == "O campo Nome e obrigatorio.");
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

        [Fact(DisplayName = "Senha nao pode ser nula ou vazia")]
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

            Assert.Contains(validationResults, r => r.ErrorMessage == "O campo Senha e obrigatorio.");
        }

        [Fact(DisplayName = "Deve criar um novo usuario com senha criptografada")]
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

        //[Fact(DisplayName = "Deve lancar excecao se o usuario estiver vinculado a atividades ativas")]
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
        //    Assert.Equal("usuario esta vinculado a atividade ativa e Nao pode ser deletado.", exception.Message);
        //}

        //[Fact(DisplayName = "Deve autenticar o usuario corretamente")]
        //public async Task AuthenticateAsync_ShouldReturnTrue_WhenPasswordIsValid()
        //{
        //    var mockRepository = new Mock<ISystemUserRepository>();
        //    var mockIssueRepository = new Mock<IIssueRepository>();
        //    var service = new SystemUserService(mockRepository.Object, mockIssueRepository.Object);

        //    var user = new SystemUserModel
        //    {
        //        UserName = "username123",
        //        Password = BCrypt.Net.BCrypt.HashPassword("password123")
        //    };

        //    mockRepository.Setup(r => r.GetUserByUserName("username123")).ReturnsAsync(user);

        //    var result = await service.AuthenticateAsync("username123", "password123");

        //    Assert.True(result);
        //}

        //[Fact(DisplayName = "Nao deve autenticar o usuario com senha invalida")]
        //public async Task AuthenticateAsync_ShouldReturnFalse_WhenPasswordIsInvalid()
        //{
        //    var mockRepository = new Mock<ISystemUserRepository>();
        //    var mockIssueRepository = new Mock<IIssueRepository>();
        //    var service = new SystemUserService(mockRepository.Object, mockIssueRepository.Object);

        //    var user = new SystemUserModel
        //    {
        //        UserName = "username123",
        //        Password = BCrypt.Net.BCrypt.HashPassword("password123")
        //    };

        //    mockRepository.Setup(r => r.GetUserByUserName("username123")).ReturnsAsync(user);

        //    var result = await service.AuthenticateAsync("username123", "wrongpassword");

        //    Assert.False(result);
        //}
        #endregion


        #region Client
        [Fact(DisplayName = "CPF valido")]
        public void GivenValidCPFNumbers()
        {
            const string cpf = "12345678909";
            const string expectedResult = "123.456.789-09";

            var client = new ClientModel();

            client.CPF = cpf;
            var actualResult = client.cpfformatado;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "CPF invalido")]
        public void GivenInvalidCPFNumbers()
        {
            const string cpf = "123";

            var client = new ClientModel();

            var exception = Assert.Throws<ArgumentException>(() => client.CPF = cpf);
            Assert.Equal("CPF invalido.", exception.Message);
        }

        [Fact(DisplayName = "CNPJ valido")]
        public void GivenValidCNPJNumbers()
        {
            const string cnpj = "12345678000195";
            const string expectedResult = "12.345.678/0001-95";

            var client = new ClientModel();

            client.CNPJ = cnpj;
            var actualFormattedCNPJ = client.cnpjformatado;

            Assert.Equal(expectedResult, actualFormattedCNPJ);
        }

        [Fact(DisplayName = "CNPJ invalido")]
        public void GivenInvalidCNPJNumbers()
        {
            const string cnpj = "123";

            var client = new ClientModel();

            var exception = Assert.Throws<ArgumentException>(() => client.CNPJ = cnpj);
            Assert.Equal("CNPJ invalido.", exception.Message);
        }

        [Fact(DisplayName = "Deve lancar excecao se CPF ou CNPJ Nao forem informados ao criar um cliente")]
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

        [Fact(DisplayName = "Validacao falha quando Endereco e nulo")]
        public void Validation_ShouldFail_WhenAddressIsNull()
        {
            var project = new ClientProjectModel
            {
                Address = null,
                ZipCode = "12345-678",
                Number = 10,
                City = "CityName",
                Title = "Project Title"
            };

            var validationResults = ValidateModel(project);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O endereco e obrigatorio.");
        }

        [Fact(DisplayName = "Validacao falha quando CEP e nulo")]
        public void Validation_ShouldFail_WhenZipCodeIsNull()
        {
            var project = new ClientProjectModel
            {
                Address = "Street Name",
                ZipCode = null,
                Number = 10,
                City = "CityName",
                Title = "Project Title"
            };

            var validationResults = ValidateModel(project);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O CEP e obrigatorio.");
        }

        [Fact(DisplayName = "Validacao falha quando Numero e menor ou igual a zero")]
        public void Validation_ShouldFail_WhenNumberIsZeroOrNegative()
        {
            var project = new ClientProjectModel
            {
                Address = "Street Name",
                ZipCode = "12345-678",
                Number = 0,
                City = "CityName",
                Title = "Project Title"
            };

            var validationResults = ValidateModel(project);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O numero deve ser um valor valido e maior que zero.");
        }

        [Fact(DisplayName = "Validacao falha quando Cidade e nula")]
        public void Validation_ShouldFail_WhenCityIsNull()
        {
            var project = new ClientProjectModel
            {
                Address = "Street Name",
                ZipCode = "12345-678",
                Number = 10,
                City = null,
                Title = "Project Title"
            };

            var validationResults = ValidateModel(project);

            Assert.Contains(validationResults, result => result.ErrorMessage == "A cidade e obrigatoria.");
        }

        [Fact(DisplayName = "Validacao falha quando Titulo e nulo")]
        public void Validation_ShouldFail_WhenTitleIsNull()
        {
            var project = new ClientProjectModel
            {
                Address = "Street Name",
                ZipCode = "12345-678",
                Number = 10,
                City = "CityName",
                Title = null
            };

            var validationResults = ValidateModel(project);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O titulo e obrigatorio.");
        }

        [Fact(DisplayName = "Validacao falha quando Titulo esta fora do tamanho permitido")]
        public void Validation_ShouldFail_WhenTitleLengthIsInvalid()
        {
            var project = new ClientProjectModel
            {
                Address = "Street Name",
                ZipCode = "12345-678",
                Number = 10,
                City = "CityName",
                Title = "Pr"
            };

            var validationResults = ValidateModel(project);

            Assert.Contains(validationResults, result => result.ErrorMessage == "O titulo deve ter entre 3 e 50 caracteres.");
        }

        [Fact(DisplayName = "Validacao bem-sucedida com dados validos")]
        public void Validation_ShouldPass_WhenAllFieldsAreValid()
        {
            var project = new ClientProjectModel
            {
                Address = "Street Name",
                ZipCode = "12345-678",
                Number = 10,
                City = "CityName",
                Title = "Project Title"
            };

            var validationResults = ValidateModel(project);

            Assert.Empty(validationResults); 
        }

        private List<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        //[Fact(DisplayName = "Deve lancar excecao se os campos obrigatorios Nao forem preenchidos")]
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
        //    Assert.Contains("O endereco e obrigatorio.", exception.Message);
        //    Assert.Contains("O CEP e obrigatorio.", exception.Message);
        //    Assert.Contains("A cidade e obrigatoria.", exception.Message);
        //    Assert.Contains("O título e obrigatorio.", exception.Message);
        //}
        #endregion

        #region Issue
        [Fact(DisplayName = "Validacao falha quando Descricao e nula ou fora do tamanho permitido")]
        public void Validator_ShouldFail_WhenDescriptionIsNullOrInvalid()
        {
            var issue = new IssueModel
            {
                Title = "Titulo valido",
                Description = null,
                ClientId = Guid.NewGuid(),
                Priority = PriorityEnum.High
            };

            var validationMessages = issue.Validator();

            Assert.Contains("O titulo deve ter entre 3 e 2000 caracteres.", validationMessages);
        }

        [Fact(DisplayName = "Validacao falha quando Cliente e nulo")]
        public void Validator_ShouldFail_WhenClientIdIsEmpty()
        {
            var issue = new IssueModel
            {
                Title = "Titulo valido",
                Description = "Descricao valida",
                ClientId = Guid.Empty,
                Priority = PriorityEnum.High
            };

            var validationMessages = issue.Validator();

            Assert.Contains("O cliente e obrigatorio.", validationMessages);
        }
        #endregion
    }
}