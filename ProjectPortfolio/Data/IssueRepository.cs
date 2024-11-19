using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    internal class IssueRepository(IDbContextFactory<Repository> dbContextFactory) : IIssueRepository
    {
        public async Task<FilterResponseModel<IssueModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var query = ct.Set<IssueModel>().AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(e => e.Title.Contains(filter.Search));
            }

            var totalRecords = await query.CountAsync();
            var filteredRecords = await query.CountAsync();

            var result = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize).ToList();

            return new FilterResponseModel<IssueModel>
            {
                Total = totalRecords,
                FilteredRecords = filteredRecords,
                Data = result
            };
        }

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
    }
}
