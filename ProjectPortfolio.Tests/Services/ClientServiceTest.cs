using Microsoft.EntityFrameworkCore;
using Moq;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services;

namespace ProjectPortfolio.Tests.Services
{
    public class ClientServiceTest
    {
        private readonly Mock<IClientRepository> _mockClientRepository;
        private readonly Mock<IIssueRepository> _mockIssueRepository;
        private readonly ClientService _clientService;

        //public ClientServiceTests()
        //{
        //    _mockClientRepository = new Mock<IClientRepository>();
        //    _clientService = new ClientService(_mockClientRepository.Object, _mockIssueRepository.Object);
        //}

        //[Fact]
        //public async Task CreateAsync_ShouldThrowException_WhenCPFAlreadyExists()
        //{
        //    // Arrange
        //    var client = new ClientModel { CPF = "12345678901", Name = "Client1" };

        //    _mockClientRepository.Setup(repo => repo.GetAll())
        //                         .Returns(new Mock<IQueryable<ClientModel>>().Object);

        //    _mockClientRepository.Setup(repo => repo.GetAll()
        //                                             .Where(e => e.CPF == client.CPF)
        //                                             .AnyAsync())
        //                         .ReturnsAsync(true); // CPF already exists

        //    // Act & Assert
        //    var exception = await Assert.ThrowsAsync<Exception>(() => _clientService.CreateAsync(client));
        //    Assert.Equal("Já existe cliente criado para este CPF.", exception.Message);
        //}

        [Fact]
        public async Task CreateAsync_ShouldReturnClient_WhenValidData()
        {
            // Arrange
            var client = new ClientModel { CPF = "12345678901", Name = "Client1" };

            _mockClientRepository.Setup(repo => repo.GetAll())
                                 .Returns(new Mock<IQueryable<ClientModel>>().Object);

            _mockClientRepository.Setup(repo => repo.InsertAsync(It.IsAny<ClientModel>()))
                                 .ReturnsAsync(client);

            // Act
            var result = await _clientService.CreateAsync(client);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(client.CPF, result.CPF);
            Assert.Equal(client.Name, result.Name);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenCPFAndCNPJAreEmpty()
        {
            // Arrange
            var client = new ClientModel { CPF = "", CNPJ = "" };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _clientService.CreateAsync(client));
            Assert.Equal("É necessário informar o CPF ou o CNPJ.", exception.Message);
        }
    }
}
