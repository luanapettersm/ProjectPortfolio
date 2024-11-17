using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IClientProjectService
    {
        Task<ClientProjectModel> CreateAsync(ClientProjectModel model);
        Task<ClientProjectModel> UpdateAsync(ClientProjectModel model);
    }
}
