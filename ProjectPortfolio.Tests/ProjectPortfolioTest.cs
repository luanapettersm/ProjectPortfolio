using ProjectPortfolio.Models;

namespace ProjectPortfolio.Tests
{
    public class ProjectPortfolioTest
    {
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
    }
}