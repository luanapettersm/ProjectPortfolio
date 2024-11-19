using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    internal class ClientProjectRepository(IDbContextFactory<Repository> dbContextFactory) : IClientProjectRepository
    {
        public async Task<FilterResponseModel<ClientProjectModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var query = ct.Set<ClientProjectModel>().AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(e => e.Title.Contains(filter.Search));
            }

            var totalRecords = await query.CountAsync();
            var filteredRecords = await query.CountAsync();

            var result = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize).ToList();

            return new FilterResponseModel<ClientProjectModel>
            {
                Total = totalRecords,
                FilteredRecords = filteredRecords,
                Data = result
            };
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
