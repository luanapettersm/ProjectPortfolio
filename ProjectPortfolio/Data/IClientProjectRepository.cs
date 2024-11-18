using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IClientProjectRepository
    {
        IQueryable<ClientProjectModel> GetAll();
        Task<ClientProjectModel> InsertAsync(ClientProjectModel model);
        Task<ClientProjectModel> UpdateAsync(ClientProjectModel model);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ClientProjectModel>> GetProjectsByClientId(Guid clientId);
    }
}
