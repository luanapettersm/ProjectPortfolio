using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface ISystemUserRepository
    {
        Task<FilterResponseModel<SystemUserModel>> FilterAsync(FilterRequestModel model);
        IQueryable<SystemUserModel> GetAll();
        Task<SystemUserModel> InsertAsync(SystemUserModel model);
        Task<SystemUserModel> UpdateAsync(SystemUserModel model);
        Task DeleteAsync(Guid id);
    }
}