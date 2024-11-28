using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;
using System.Linq.Dynamic.Core;

namespace ProjectPortfolio.Data
{
    internal class ClientProjectRepository(IDbContextFactory<Repository> dbContextFactory) : IClientProjectRepository
    {
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

        public async Task<IEnumerable<ClientProjectModel>> GetAllClientProjects(Guid clientId)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            return await dbContext.Set<ClientProjectModel>().Where(e => e.ClientId == clientId).ToListAsync();
        }
    }
}
