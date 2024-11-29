using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;
using System.Linq.Dynamic.Core;

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

        public async Task<IEnumerable<SystemUserModel>> GetListAsync()
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<SystemUserModel>().AsNoTracking().Where(e => e.IsEnabled).Select(e => new SystemUserModel
            {
                Id = e.Id,
                Name = e.DisplayName
            }).ToListAsync();
        }

        public async Task<SystemUserModel> GetAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<SystemUserModel>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SystemUserModel>> GetAllSystemUsers()
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Set<SystemUserModel>().ToListAsync();
        }

        public async Task<SystemUserModel> GetUserByUserName(AuthenticateModel auth)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<SystemUserModel>().Where(e => e.UserName == auth.UserName).FirstOrDefaultAsync();
        }
    }
}