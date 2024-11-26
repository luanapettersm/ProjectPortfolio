using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;
using System.Linq.Dynamic.Core;

namespace ProjectPortfolio.Data
{
    internal class ClientProjectRepository(IDbContextFactory<Repository> dbContextFactory) : IClientProjectRepository
    {
        public async Task<FilterResponseModel<ClientProjectModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var query = ct.Set<ClientProjectModel>().AsQueryable();

            if (filter.Filters.ContainsKey("search"))
            {
                var search = filter.Filters["search"];
                query = query.Where(e => e.Title.Contains(search));
            }

            if (filter.SortColumn != "")
                query = query.OrderBy($" {filter.SortColumn} {filter.SortDirection} ");

            var queryResult = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).Select(e => e);

            var result = new FilterResponseModel<ClientProjectModel>
            {
                Page = filter.Page,
                Total = query.Count()
            };

            result.Result = await (from projetcts in queryResult
                                   select new ClientProjectModel
                                   {
                                       Id = projetcts.Id,
                                       Title = projetcts.Title,
                                       Address = projetcts.Address,
                                       City = projetcts.City,
                                       ClientId = projetcts.ClientId,    
                                       Number = projetcts.Number,
                                       Description = projetcts.Description,
                                       ZipCode = projetcts.ZipCode,
                                       IsEnabled = projetcts.IsEnabled
                                   }).ToListAsync();

            return result; 
        }

        public async Task DeleteAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            await ct.Set<ClientProjectModel>().Where(e => e.Id == id).ExecuteDeleteAsync();
            await ct.SaveChangesAsync();
        }

        public IQueryable<ClientProjectModel> GetAll()
        {
            var ct = dbContextFactory.CreateDbContext();
            return ct.Set<ClientProjectModel>();
        }

        public async Task<ClientProjectModel> InsertAsync(ClientProjectModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = await ct.AddAsync(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ClientProjectModel> UpdateAsync(ClientProjectModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = ct.Update(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<ClientProjectModel>> GetProjectsByClientId(Guid clientId)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<ClientProjectModel>().Where(e => e.ClientId == clientId && e.IsEnabled)
                .Select(e => new ClientProjectModel
                {
                    Id = e.Id,
                    Title = e.Title
                }).ToListAsync();
        }
    }
}
