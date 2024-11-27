using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;
using System.Linq.Dynamic.Core;

namespace ProjectPortfolio.Data
{
    internal class ClientRepository(IDbContextFactory<Repository> dbContextFactory) : IClientRepository
    {
        public async Task<FilterResponseModel<ClientModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();

            var query = ct.Set<ClientModel>().AsQueryable();

            if (filter.Filters.ContainsKey("search"))
            {
                var search = filter.Filters["search"];
                query = query.Where(e => e.Name.Contains(search) || e.City.Contains(search)
                    || e.CNPJ.Contains(search)
                    || e.CPF.Contains(search));
            }

            if (!string.IsNullOrEmpty(filter.SortColumn))
                query = query.OrderBy($" {filter.SortColumn} {filter.SortDirection} ");

            var queryResult = query.Skip((filter.Page - 1) * filter.PageSize).Take(filter.PageSize).Select(e => e);

            var result = new FilterResponseModel<ClientModel>
            {
                Page = filter.Page,
                Total = await query.CountAsync()
            };

            result.Result = await queryResult.Select(e => new ClientModel
                                   {
                                       Id = e.Id,
                                       Name = e.Name,
                                       CNPJ = e.CNPJ,
                                       CPF = e.CPF,
                                       IsEnabled = e.IsEnabled
                                   }).ToListAsync();

            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            await ct.Set<ClientModel>().Where(e => e.Id == id).ExecuteDeleteAsync();
            await ct.SaveChangesAsync();
        }

        public IQueryable<ClientModel> GetAll()
        {
            var ct = dbContextFactory.CreateDbContext();
            return ct.Set<ClientModel>();
        }

        public async Task<ClientModel> InsertAsync(ClientModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = await ct.AddAsync(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = ct.Update(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<ClientModel>> GetListAsync()
        {
            var ct = await dbContextFactory.CreateDbContextAsync();

            return await ct.Set<ClientModel>().Where(e => e.IsEnabled).OrderBy(e => e.Name).Select(e => new ClientModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
        }

        public async Task<ClientModel> GetAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<ClientModel>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ClientModel>> GetListAsync(IEnumerable<Guid> ids)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<ClientModel>().AsNoTracking().ToListAsync();
        }
    }
}
