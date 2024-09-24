using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface ISystemUserService
    {
        Task<SystemUserModel> CreateAsync(SystemUserModel model);
        Task<SystemUserModel> UpdateAsync(SystemUserModel model);
        Task<SystemUserModel> DeleteAsync(Guid id);
        Task<List<SystemUserModel>> GetAllAsync();
        Task<SystemUserModel> GetAsync(Guid id);
    }
}
