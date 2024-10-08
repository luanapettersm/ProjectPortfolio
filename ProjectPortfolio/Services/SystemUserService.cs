using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class SystemUserService(Repository repository) : ISystemUserService
    {
        public Task<SystemUserModel> CreateAsync(SystemUserModel model)
        {
            throw new NotImplementedException();
        }

        public Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = repository.SystemUsers.Where(e => e.Id == id).FirstOrDefaultAsync();

            await repository.SystemUsers.ExecuteDeleteAsync(query);
        }

        public async Task<List<SystemUserModel>> GetAllAsync()
        {
            return await repository.SystemUsers.ToListAsync();
        }

        public async Task<SystemUserModel> GetAsync(Guid id)
        {
            return await repository.SystemUsers.Where(e => e.Id == id).FirstOrDefaultAsync();
        }
    }
}
