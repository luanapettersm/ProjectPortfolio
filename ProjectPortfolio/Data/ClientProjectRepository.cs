using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;
using System.Linq.Dynamic.Core;

namespace ProjectPortfolio.Data
{
    public class ClientProjectRepository(IDbContextFactory<Repository> dbContextFactory) : IClientProjectRepository
    {
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

        public async Task<IEnumerable<ClientProjectModel>> GetAllClientProjects(Guid clientId)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<ClientProjectModel>().Where(e => e.ClientId == clientId).ToListAsync();
        }

        public async Task<ClientProjectModel> GetAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<ClientProjectModel>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }
    }
}
