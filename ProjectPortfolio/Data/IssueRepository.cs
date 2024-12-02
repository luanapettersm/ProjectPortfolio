using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public class IssueRepository(IDbContextFactory<Repository> dbContextFactory) : IIssueRepository
    {
        public IQueryable<IssueModel> GetAll()
        {
            var ct = dbContextFactory.CreateDbContext();
            return ct.Set<IssueModel>();
        }

        public async Task<IssueModel> InsertAsync(IssueModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = await ct.AddAsync(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IssueModel> UpdateAsync(IssueModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = ct.Update(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IssueModel> GetAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<IssueModel>().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<IssueModel>> ListIssues(IssueStatusEnum status)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            return await ct.Set<IssueModel>()
                .Where(e => e.Status == status)
                .ToListAsync();
        }
    }
}
