using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IClientService
    {
        Task<ClientModel> CreateAsync(ClientModel model);
        Task<ClientModel> UpdateAsync(ClientModel model);
        Task DeleteAsync(Guid id);
    }
}
