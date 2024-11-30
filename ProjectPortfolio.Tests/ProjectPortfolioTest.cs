using Moq;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Tests
{
    public class ProjectPortfolioTest
    {
        private readonly Mock<ISystemUserRepository> _systemUserRepositoryMock;
        private readonly Mock<IIssueRepository> _issueRepositoryMock;
        private readonly SystemUserService _systemUserService;
        private readonly Mock<IClientRepository> _clientRepositoryMock;
        private readonly ClientService _clientService;
        private readonly Mock<IClientProjectRepository> _clientProjectRepositoryMock;
        private readonly ClientProjectService _clientProjectService;

        public ProjectPortfolioTest()
        {
            _systemUserRepositoryMock = new Mock<ISystemUserRepository>();
            _issueRepositoryMock = new Mock<IIssueRepository>();
            _systemUserService = new SystemUserService(_systemUserRepositoryMock.Object, _issueRepositoryMock.Object);
            _clientRepositoryMock = new Mock<IClientRepository>();
            _clientService = new ClientService(_clientRepositoryMock.Object, _issueRepositoryMock.Object);
            _clientProjectRepositoryMock = new Mock<IClientProjectRepository>();
            _clientProjectService = new ClientProjectService(_clientProjectRepositoryMock.Object);
        }

        [Fact(DisplayName = "Given valid CPF numbers")]
        public void GivenValidCPFNumbers()
        {
            const string cpf = "12345678909"; 
            const string expectedResult = "123.456.789-09";

            var client = new ClientModel();

            client.CPF = cpf; 
            var actualResult = client.cpfformatado;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "Given invalid CPF numbers")]
        public void GivenInvalidCPFNumbers()
        {
            const string cpf = "123";

            var client = new ClientModel();

            var exception = Assert.Throws<ArgumentException>(() => client.CPF = cpf);
            Assert.Equal("CPF invalido.", exception.Message);
        }

        [Fact(DisplayName = "Given valid CNPJ numbers")]
        public void GivenValidCNPJNumbers()
        {
            const string cnpj = "12345678000195"; 
            const string expectedResult = "12.345.678/0001-95";

            var client = new ClientModel();

            client.CNPJ = cnpj;
            var actualFormattedCNPJ = client.cnpjformatado;

            Assert.Equal(expectedResult, actualFormattedCNPJ);
        }

        [Fact(DisplayName = "Given invalid CNPJ numbers")]
        public void GivenInvalidCNPJNumbers()
        {
            const string cnpj = "123";

            var client = new ClientModel();

            var exception = Assert.Throws<ArgumentException>(() => client.CNPJ = cnpj);
            Assert.Equal("CNPJ invalido.", exception.Message);
        }

        [Fact(DisplayName = "Should return error when ZipCode is null or empty")]
        public void Validator_ShouldReturnError_WhenZipCodeIsNullOrEmpty()
        {
            var model = new ClientModel
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Mail = "john.doe@example.com",
                City = "City",
                State = "State",
                Address = "Address",
                ZipCode = null
            };

            var validationMessages = model.Validator();

            Assert.Contains("O CEP e obrigatorio.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when Address is null or empty")]
        public void Validator_ShouldReturnError_WhenAddressIsNullOrEmpty()
        {
            var model = new ClientModel
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Mail = "john.doe@example.com",
                ZipCode = "12345",
                City = "City",
                State = "State",
                Address = ""
            };

            var validationMessages = model.Validator();

            Assert.Contains("O endereco e obrigatorio.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when PhoneNumber is null or empty")]
        public void Validator_ShouldReturnError_WhenPhoneNumberIsNullOrEmpty()
        {
            var model = new ClientModel
            {
                Name = "John Doe",
                Mail = "john.doe@example.com",
                ZipCode = "12345",
                Address = "Address",
                City = "City",
                State = "State",
                PhoneNumber = null
            };

            var validationMessages = model.Validator();

            Assert.Contains("O numero e obrigatorio.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when City is null or empty")]
        public void Validator_ShouldReturnError_WhenCityIsNullOrEmpty()
        {
            var model = new ClientModel
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Mail = "john.doe@example.com",
                ZipCode = "12345",
                Address = "Address",
                State = "State",
                City = null
            };

            var validationMessages = model.Validator();

            Assert.Contains("A cidadde e obrigatoria.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when State is null or empty")]
        public void Validator_ShouldReturnError_WhenStateIsNullOrEmpty()
        {
            var model = new ClientModel
            {
                Name = "John Doe",
                PhoneNumber = "1234567890",
                Mail = "john.doe@example.com",
                ZipCode = "12345",
                Address = "Address",
                City = "City",
                State = ""
            };

            var validationMessages = model.Validator();

            Assert.Contains("O estado e obrigatorio.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when Name is null or empty")]
        public void Validator_ShouldReturnError_WhenNameIsNullOrEmpty()
        {
            var model = new SystemUserModel
            {
                Name = "",
                Surname = "Doe",
                UserName = "johndoe",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager
            };

            var validationMessages = model.Validator();

            Assert.Contains("Nome deve ter entre 3 e 35 caracteres.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when Surname is null or empty")]
        public void Validator_ShouldReturnError_WhenSurnameIsTooShort()
        {
            var model = new SystemUserModel
            {
                Name = "John",
                Surname = "",
                UserName = "johndoe",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager
            };

            var validationMessages = model.Validator();

            Assert.Contains("O sobrenome deve ter entre 3 e 100 caracteres.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when UserName is null or empty")]
        public void Validator_ShouldReturnError_WhenUserNameIsTooLong()
        {
            var model = new SystemUserModel
            {
                Name = "John",
                Surname = "Doe",
                UserName = "johndoe123456johndoe123456johndoe123456johndoe123456johndoe123456",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager
            };

            var validationMessages = model.Validator();

            Assert.Contains("O login deve ter entre 3 e 50 caracteres.", validationMessages);
        }

        [Fact(DisplayName = "Should return error when Password is null or empty")]
        public void Validator_ShouldReturnError_WhenPasswordIsNull()
        {
            var model = new SystemUserModel
            {
                Name = "John",
                Surname = "Doe",
                UserName = "johndoe",
                Password = null,
                BusinessRole = BusinessRoleEnum.manager
            };

            var validationMessages = model.Validator();

            Assert.Contains("A senha e obrigatoria.", validationMessages);
        }


        [Fact(DisplayName = "Should not return any error when is valid")]
        public void Validator_ShouldReturnNoErrors_WhenModelIsValid()
        {
            var model = new SystemUserModel
            {
                Name = "John",
                Surname = "Doe",
                UserName = "johndoe",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager
            };

            var validationMessages = model.Validator();

            Assert.Empty(validationMessages);
        }

        [Fact(DisplayName = "Should return valid create SystemUser")]
        public async Task CreateAsync_ShouldReturnSystemUser_WhenValidModel()
        {
            var model = new SystemUserModel
            {
                Name = "John",
                Surname = "Doe",
                UserName = "johndoe",
                Password = "password123",
                BusinessRole = BusinessRoleEnum.manager
            };

            var expected = new SystemUserModel { Id = Guid.NewGuid() };

            _systemUserRepositoryMock.Setup(repo => repo.InsertAsync(It.IsAny<SystemUserModel>())).ReturnsAsync(expected);

            var result = await _systemUserService.CreateAsync(model);

            Assert.NotNull(result);
            Assert.Equal(expected.Id, result.Id);
            _systemUserRepositoryMock.Verify(repo => repo.InsertAsync(It.IsAny<SystemUserModel>()), Times.Once);
        }

        [Fact(DisplayName = "Should return error when Title is null or empty")]
        public void CreateValidator_ShouldReturnError_WhenTitleIsNullOrEmpty()
        {
            var model = new ClientProjectModel 
            {
                Title = "",
                City = "City",
                Address = "Address",
                ZipCode = "12345",
                Number = 10
            };

            var result = model.CreateValidator();

            Assert.Contains("O titulo deve ter entre 3 e 50 caracteres.", result);
        }

        [Fact(DisplayName = "Should return error when Title is null or empty")]
        public void CreateValidator_ShouldReturnError_WhenTitleIsTooLong()
        {
            var model = new ClientProjectModel 
            { 
                Title = new string('A', 51) 
            };

            var result = model.CreateValidator();

            Assert.Contains("O titulo deve ter entre 3 e 50 caracteres.", result);
        }

        [Fact(DisplayName = "Should return any error when Title is valid")]
        public void CreateValidator_ShouldNotReturnError_WhenTitleIsValid()
        {
            var model = new ClientProjectModel 
            { 
                Title = "Projeto de Teste",
                City = "City",
                Address = "Address",
                ZipCode = "12345",
                Number = 10
            };

            var result = model.CreateValidator();

            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should return error when Address is null or empty")]
        public void CreateValidator_ShouldReturnError_WhenAddressIsNullOrEmpty()
        {
            var model = new ClientProjectModel 
            {
                Title = "Projeto de Teste",
                City = "City",
                Address = null,
                ZipCode = "12345",
                Number = 10
            };

            var result = model.CreateValidator();

            Assert.Contains("Necessário informar o endereco da obra.", result);
        }

        [Fact(DisplayName = "Should return error when ZipCode is null or empty")]
        public void CreateValidator_ShouldReturnError_WhenZipCodeIsNullOrEmpty()
        {
            var model = new ClientProjectModel 
            {
                Title = "Projeto de Teste",
                City = "City",
                Address = "Address",
                Number = 10,
                ZipCode = ""
            };

            var result = model.CreateValidator();

            Assert.Contains("Necessário informar o CEP.", result);
        }

        [Fact(DisplayName = "Should return error when City is null or empty")]
        public void CreateValidator_ShouldReturnError_WhenCityIsNullOrEmpty()
        {
            var model = new ClientProjectModel 
            {
                Title = "Projeto de Teste",
                City = null,
                Address = "Address",
                ZipCode = "12345",
                Number = 10
            };

            var result = model.CreateValidator();

            Assert.Contains("Necessário informar a cidade em que acontecerá a obra.", result);
        }

        [Fact(DisplayName = "Should create a valid project")]
        public async Task CreateAsync_ShouldReturnClientProject_WhenValidModel()
        {
            var validModel = new ClientProjectModel { Title = "Projeto A", Address = "Rua Teste", City = "Cidade", ZipCode = "12345", Number = 123 };
            _clientProjectRepositoryMock.Setup(r => r.InsertAsync(It.IsAny<ClientProjectModel>())).ReturnsAsync(validModel);

            var result = await _clientProjectService.CreateAsync(validModel);

            Assert.NotNull(result);
            Assert.Equal("Projeto A", result.Title);
        }

        [Fact(DisplayName = "Should return error when Issue Title is null or empty")]
        public void CreateValidator_ShouldReturnError_WhenIssueTitleIsNullOrEmpty()
        {
            var model = new IssueModel
            {
                Title = "", 
                Description = "Descricao valida",
                ClientId = Guid.NewGuid(),
                Priority = PriorityEnum.Low
            };

            var result = model.CreateValidator();

            Assert.Contains("O titulo deve ter entre 3 e 100 caracteres.", result);
        }

        [Fact(DisplayName = "Should return error when Issue Description is too short")]
        public void CreateValidator_ShouldReturnError_WhenIssueDescriptionIsTooShort()
        {
            var model = new IssueModel
            {
                Title = "titulo valido",
                Description = "AB",
                ClientId = Guid.NewGuid(),
                Priority = PriorityEnum.Low
            };

            var result = model.CreateValidator();

            Assert.Contains("O titulo deve ter entre 3 e 2000 caracteres.", result);
        }

        [Fact(DisplayName = "Should return error when Issue ClientId is empty")]
        public void CreateValidator_ShouldReturnError_WhenIssueClientIdIsEmpty()
        {
            var model = new IssueModel
            {
                Title = "titulo valido",
                Description = "Descricao valida",
                ClientId = Guid.Empty,
                Priority = PriorityEnum.Low
            };

            var result = model.CreateValidator();

            Assert.Contains("O cliente e obrigatorio.", result);
        }

        [Fact(DisplayName = "Should return any error when valid")]
        public void CreateValidator_ShouldNotReturnError_WhenModelIsValid()
        {
            var model = new IssueModel
            {
                Title = "titulo valido",
                Description = "Descricao valida",
                ClientId = Guid.NewGuid(),
                Priority = PriorityEnum.Low
            };

            var result = model.CreateValidator();

            Assert.Empty(result);
        }
    }
}