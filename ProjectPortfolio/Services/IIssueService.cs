using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IIssueService
    {
        Task<bool> ValidateIssueIsOpened(Guid issueId);
        Task<IssueModel> CreateAsync(IssueModel model);
        Task<IssueModel> UpdateAsync(IssueModel model);
    }
}
