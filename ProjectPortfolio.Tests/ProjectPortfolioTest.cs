using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;
using ProjectPortfolio.Services.Exceptions;

namespace ProjectPortfolio.Tests
{
    public class ProjectPortfolioTest
    {
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<IIssueRepository> _mockIssueRepository;
        private readonly ClientService _clientService;
        private readonly Mock<ISystemUserRepository> _mockSystemUserRepository;

        public ProjectPortfolioTest()
        {
            _mockClientRepository = new Mock<IClientRepository>();
            _mockIssueRepository = new Mock<IIssueRepository>();
            _clientService = new ClientService(_mockClientRepository.Object, _mockIssueRepository.Object);
            _mockSystemUserRepository = new Mock<ISystemUserRepository>();
        }

        public static class DbSetMock
        {
            public static DbSet<T> SetupData<T>(params T[] items) where T : class
            {
                var queryable = items.AsQueryable();
                var mockSet = new Mock<DbSet<T>>();
                mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
                mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
                mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
                mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
                return mockSet.Object;
            }
        }

        [Fact(DisplayName = "Given valid CPF numbers")]
        public void GivenValidCPFNumbers()
        {
            //Arrange
            const string cpf = "12345678909"; 
            const string expectedResult = "123.456.789-09";

            var client = new ClientModel();

            //Act
            client.CPF = cpf; 
            var actualResult = client.cpfformatado;

            //Assert
            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "Given invalid CPF numbers")]
        public void GivenInvalidCPFNumbers()
        {
            // Arrange
            const string cpf = "123";

            var client = new ClientModel();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => client.CPF = cpf);
            Assert.Equal("CPF inválido.", exception.Message);
        }

        [Fact(DisplayName = "Given valid CNPJ numbers")]
        public void GivenValidCNPJNumbers()
        {
            // Arrange
            const string cnpj = "12345678000195"; 
            const string expectedResult = "12.345.678/0001-95";

            var client = new ClientModel();

            // Act
            client.CNPJ = cnpj;
            var actualFormattedCNPJ = client.cnpjformatado;

            // Assert
            Assert.Equal(expectedResult, actualFormattedCNPJ);
        }

        [Fact(DisplayName = "Given invalid CNPJ numbers")]
        public void GivenInvalidCNPJNumbers()
        {
            // Arrange
            const string cnpj = "123";

            var client = new ClientModel();

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => client.CNPJ = cnpj);
            Assert.Equal("CNPJ inválido.", exception.Message);
        }

        //[Fact(DisplayName = "Invalid delete client has active issues")]
        //public async Task DeleteInvalidClientHasActiveIssues()
        //{
        //    var clientId = Guid.NewGuid();
        //    var activeIssues = new[]
        //    {
        //        new IssueModel { ClientId = clientId, Status = Enumerators.IssueStatusEnum.Opened },
        //        new IssueModel { ClientId = clientId, Status = Enumerators.IssueStatusEnum.Pending }
        //    };

        //    _mockIssueRepository.Setup(r => r.GetAll())
        //        .Returns(DbSetMock.SetupData(activeIssues));

        //    var exception = await Assert.ThrowsAsync<UserFriendlyException>(() => _clientService.DeleteAsync(clientId));

        //    Assert.Equal("Cliente está vinculado a atividade ativa e não pode ser excluído.", exception.Message);
        //}


        //[Fact(DisplayName = "Creating valid system user")]
        //public async Task CreateValidSystemUser()
        //{
        //    // Arrange
        //    var userModel = new SystemUserModel
        //    {
        //        Name = "John",
        //        Surname = "Doe",
        //        UserName = "johndoe",
        //        Password = "password123",
        //        BusinessRole = new BusinessRoleEnum() 
        //    };
        //    var mockRepository = new _mockSystemUserRepository;
        //    var service = new SystemUserService(mockRepository.Object);

        //    mockRepository.Setup(r => r.InsertAsync(It.IsAny<SystemUserModel>())).ReturnsAsync(userModel);

        //    // Act
        //    var result = await service.CreateAsync(userModel);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("John", result.Name);
        //    Assert.Equal("Doe", result.Surname);
        //}

        //[Fact(DisplayName = "Creating invalid system user name")]
        //public async Task CreateInvalidSystemUser()
        //{
        //    // Arrange
        //    var userModel = new SystemUserModel
        //    {
        //        Name = "Jo", 
        //        Surname = "Doe",
        //        UserName = "johndoe",
        //        Password = "password123",
        //        BusinessRole = new BusinessRoleEnum()
        //    };
        //    var mockRepository = new Mock<IRepository<SystemUserModel>>();
        //    var service = new SystemUserService(mockRepository.Object);

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => service.CreateAsync(userModel));
        //}

        //[Fact(DisplayName = "Updating valid system user")]
        //public async Task UpdateValidSystemUser()
        //{
        //    // Arrange
        //    var existingUser = new SystemUserModel
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "John",
        //        Surname = "Doe",
        //        UserName = "johndoe",
        //        Password = "password123",
        //        BusinessRole = new BusinessRoleEnum(),
        //        DateCreated = DateTimeOffset.Now.AddDays(-1)
        //    };

        //    var updatedUser = new SystemUserModel
        //    {
        //        Id = existingUser.Id,
        //        Name = "John Updated",
        //        Surname = "Doe",
        //        UserName = "johndoe_updated",
        //        Password = "newpassword123",
        //        BusinessRole = new BusinessRoleEnum(),
        //        DateCreated = existingUser.DateCreated
        //    };

        //    var mockRepository = new Mock<IRepository<SystemUserModel>>();
        //    mockRepository.Setup(r => r.GetAll()).Returns(new List<SystemUserModel> { existingUser }.AsQueryable());
        //    mockRepository.Setup(r => r.UpdateAsync(It.IsAny<SystemUserModel>())).ReturnsAsync(updatedUser);

        //    var service = new SystemUserService(mockRepository.Object);

        //    // Act
        //    var result = await service.UpdateAsync(updatedUser);

        //    // Assert
        //    Assert.Equal("John Updated", result.Name);
        //    Assert.Equal("johndoe_updated", result.UserName);
        //}

        //[Fact(DisplayName = "Updating valid system user name")]
        //public async Task UpdateInvalidSystemUserName()
        //{
        //    // Arrange
        //    var existingUser = new SystemUserModel
        //    {
        //        Id = Guid.NewGuid(),
        //        Name = "John",
        //        Surname = "Doe",
        //        UserName = "johndoe",
        //        Password = "password123",
        //        BusinessRole = new BusinessRoleEnum(),
        //        DateCreated = DateTimeOffset.Now.AddDays(-1)
        //    };

        //    var updatedUser = new SystemUserModel
        //    {
        //        Id = existingUser.Id,
        //        Name = "Jo",  
        //        Surname = "Doe",
        //        UserName = "johndoe_updated",
        //        Password = "newpassword123",
        //        BusinessRole = new BusinessRoleEnum(),
        //        DateCreated = existingUser.DateCreated
        //    };

        //    var mockRepository = new Mock<IRepository<SystemUserModel>>();
        //    mockRepository.Setup(r => r.GetAll()).Returns(new List<SystemUserModel> { existingUser }.AsQueryable());

        //    var service = new SystemUserService(mockRepository.Object);

        //    // Act & Assert
        //    await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(updatedUser));
        //}

        //[Fact(DisplayName = "Valid system user deleting")]
        //public async Task DeleteValidSystemUser()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var mockRepository = new Mock<IRepository<SystemUserModel>>();
        //    var mockIssueRepository = new Mock<IIssueRepository>();

        //    mockRepository.Setup(r => r.GetAll()).Returns(new List<SystemUserModel> { new SystemUserModel { Id = userId } }.AsQueryable());
        //    mockIssueRepository.Setup(r => r.GetAll()).Returns(new List<IssueModel>().AsQueryable()); 

        //    var service = new SystemUserService(mockRepository.Object, mockIssueRepository.Object);

        //    // Act
        //    await service.DeleteAsync(userId);

        //    // Assert
        //    mockRepository.Verify(r => r.DeleteAsync(userId), Times.Once);
        //}

        //[Fact(DisplayName = "Invalid system user deleting")]
        //public async Task DeleteInvalidSystemUser()
        //{
        //    // Arrange
        //    var userId = Guid.NewGuid();
        //    var mockRepository = new Mock<ISystemUserRepository<SystemUserModel>>();
        //    var mockIssueRepository = new Mock<IIssueRepository>();

        //    mockRepository.Setup(r => r.GetAll()).Returns(new List<SystemUserModel> { new SystemUserModel { Id = userId } }.AsQueryable());
        //    mockIssueRepository.Setup(r => r.GetAll()).Returns(new List<IssueModel>().AsQueryable());

        //    var service = new SystemUserService(mockRepository.Object, mockIssueRepository.Object);

        //    // Act
        //    await service.DeleteAsync(userId);

        //    // Assert
        //    mockRepository.Verify(r => r.DeleteAsync(userId), Times.Once);
        //}

    }
}