using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface ISystemUserService
    {
        Task<SystemUserModel> CreateAsync(SystemUserModel model);
        Task<SystemUserModel> UpdateAsync(SystemUserModel model);
        Task DeleteAsync(Guid id);
    }
}
