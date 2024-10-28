using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public class IssueNoteRepository(IDbContextFactory<Repository> dbContextFactory) : IIssueNoteRepository
    {
        public IQueryable<IssueNoteModel> GetAll()
        {
            var ct = dbContextFactory.CreateDbContext();
            return ct.Set<IssueNoteModel>();
        }

        public async Task<IssueNoteModel> InsertAsync(IssueNoteModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = await ct.AddAsync(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IssueNoteModel> UpdateAsync(IssueNoteModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = ct.Update(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }
    }
}
