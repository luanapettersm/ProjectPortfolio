using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public class SystemUserRepository(IDbContextFactory<Repository> dbContextFactory) : ISystemUserRepository
    {
        public IQueryable<SystemUserModel> GetAll()
        {
            var ct = dbContextFactory.CreateDbContext();
            return ct.Set<SystemUserModel>();
        }

        public async Task<SystemUserModel> InsertAsync(SystemUserModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = await ct.AddAsync(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = ct.Update(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            await ct.Set<SystemUserModel>().Where(e => e.Id == id).ExecuteDeleteAsync();
            await ct.SaveChangesAsync();
        }
    }
}
