using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IIssueRepository
    {
        IQueryable<IssueModel> GetAll();
        Task<IssueModel> InsertAsync(IssueModel model);
        Task<IssueModel> UpdateAsync(IssueModel model);
    }
}
