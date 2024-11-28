using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IClientRepository
    {
        IQueryable<ClientModel> GetAll();
        Task<ClientModel> InsertAsync(ClientModel model);
        Task<ClientModel> UpdateAsync(ClientModel model);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ClientModel>> GetListAsync();
        Task<ClientModel> GetAsync(Guid id);
        Task<IEnumerable<ClientModel>> GetListAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<ClientModel>> GetAllClients();
    }
}
