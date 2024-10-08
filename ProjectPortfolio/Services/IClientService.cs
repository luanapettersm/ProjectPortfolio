using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IClientService
    {
        Task<ClientModel> FilterAsync();
        Task<IEnumerable<ClientModel>> GetAllAsync();
        Task<ClientModel> GetAsync(Guid id);
        Task<ClientModel> CreateAsync(ClientModel model);
        Task<ClientModel> UpdateAsync(ClientModel model);
        Task DeleteAsync(Guid id);
    }
}
