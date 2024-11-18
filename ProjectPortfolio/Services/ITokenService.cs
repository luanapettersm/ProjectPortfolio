using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface ITokenService
    {
        string GetToken(SystemUserModel systemUser);
    }
}
