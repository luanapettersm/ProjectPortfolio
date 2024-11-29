using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IIssueRepository
    {
        IQueryable<IssueModel> GetAll();
        Task<IssueModel> InsertAsync(IssueModel model);
        Task<IssueModel> UpdateAsync(IssueModel model);
        Task<IssueModel> GetAsync(Guid id);
        Task<IEnumerable<IssueModel>> ListIssues(IssueStatusEnum status, AuthenticateModel auth);
    }
}
