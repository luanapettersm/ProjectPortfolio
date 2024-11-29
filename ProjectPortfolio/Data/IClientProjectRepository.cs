using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IClientProjectRepository
    {
        IQueryable<ClientProjectModel> GetAll();
        Task<ClientProjectModel> InsertAsync(ClientProjectModel model);
        Task<ClientProjectModel> UpdateAsync(ClientProjectModel model);
        Task<IEnumerable<ClientProjectModel>> GetAllClientProjects(Guid clientId);
        Task<ClientProjectModel> GetAsync(Guid id);
    }
}
