using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    internal class SystemUserRepository(IDbContextFactory<Repository> dbContextFactory) : ISystemUserRepository
    {
        public async Task<FilterResponseModel<SystemUserModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var query = ct.Set<SystemUserModel>().AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(e => e.Name.Contains(filter.Search) || e.Surname.Contains(filter.Search)); 
            }

            var totalRecords = await query.CountAsync();
            var filteredRecords = await query.CountAsync();

            var result = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize).ToList();

            return new FilterResponseModel<SystemUserModel>
            {
                Total = totalRecords,
                FilteredRecords = filteredRecords,
                Data = result
            };
        }

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
