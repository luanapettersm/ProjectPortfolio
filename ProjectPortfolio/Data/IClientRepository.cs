using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IClientRepository
    {
        Task<FilterResponseModel<ClientModel>> FilterAsync(FilterRequestModel filter);
        IQueryable<ClientModel> GetAll();
        Task<ClientModel> InsertAsync(ClientModel model);
        Task<ClientModel> UpdateAsync(ClientModel model);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<ClientModel>> GetListAsync();
    }
}
