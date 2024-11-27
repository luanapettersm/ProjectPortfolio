using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;
using System.Linq.Dynamic.Core;

namespace ProjectPortfolio.Data
{
    internal class SystemUserRepository(IDbContextFactory<Repository> dbContextFactory) : ISystemUserRepository
    {
        public async Task<FilterResponseModel<SystemUserModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var query = ct.Set<SystemUserModel>().AsQueryable();

            if (filter.Filters.ContainsKey("IsEnabled") && !String.IsNullOrEmpty(filter.Filters["IsEnabled"]))
                query = query.Where(e => e.IsEnabled == bool.Parse(filter.Filters["IsEnabled"]));

            if (filter.Filters.ContainsKey("search"))
            {
                var search = filter.Filters["search"];
                query = query.Where(e => e.Name.Contains(search) || e.Surname.Contains(search)
                                                                 || e.UserName.Contains(search));
            }

            if (filter.SortColumn != "")
                query = query.OrderBy($" {filter.SortColumn} {filter.SortDirection} ");

            var queryResult = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).Select(e => e);

            var result = new FilterResponseModel<SystemUserModel>
            {
                Page = filter.Page,
                Total = query.Count()
            };

            result.Result = await (from systemUsers in queryResult
                                   select new SystemUserModel
                                   {
                                       Id = systemUsers.Id,
                                       Name = systemUsers.Name,
                                       Surname = systemUsers.Surname,
                                       UserName = systemUsers.UserName,
                                       IsEnabled = systemUsers.IsEnabled,
                                   }).ToListAsync();

            return result;
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

        public async Task<IEnumerable<SystemUserModel>> GetListAsync(IEnumerable<Guid> ids)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<SystemUserModel>().AsNoTracking().ToListAsync();
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
    }
}