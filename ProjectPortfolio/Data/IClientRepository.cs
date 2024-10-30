using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IClientRepository
    {
        IQueryable<ClientModel> GetAll();
        Task<ClientModel> InsertAsync(ClientModel model);
        Task<ClientModel> UpdateAsync(ClientModel model);
        Task DeleteAsync(Guid id);
    }
}
