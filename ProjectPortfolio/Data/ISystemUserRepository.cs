using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface ISystemUserRepository
    {
        IQueryable<SystemUserModel> GetAll();
        Task<SystemUserModel> InsertAsync(SystemUserModel model);
        Task<SystemUserModel> UpdateAsync(SystemUserModel model);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<SystemUserModel>> GetListAsync(IEnumerable<Guid> ids);
        Task<IEnumerable<SystemUserModel>> GetListAsync();
        Task<SystemUserModel> GetAsync(Guid id);
        Task<IEnumerable<SystemUserModel>> GetAllSystemUsers();
        Task<SystemUserModel> GetUserByUserName(AuthenticateModel auth);
    }
}