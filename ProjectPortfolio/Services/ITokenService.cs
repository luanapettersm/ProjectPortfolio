using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface ITokenService
    {
        Task<string> GetTokenAsync(AuthenticateModel authenticate);
    }
}
