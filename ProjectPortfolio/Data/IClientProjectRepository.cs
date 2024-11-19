using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IClientProjectRepository
    {
        Task<FilterResponseModel<ClientProjectModel>> FilterAsync(FilterRequestModel filter);
        IQueryable<ClientProjectModel> GetAll();
        Task<ClientProjectModel> InsertAsync(ClientProjectModel model);
        Task<ClientProjectModel> UpdateAsync(ClientProjectModel model);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ClientProjectModel>> GetProjectsByClientId(Guid clientId);
    }
}
