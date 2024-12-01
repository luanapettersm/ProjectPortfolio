using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IIssueService
    {
        Task<bool> ValidateIssueIsOpened(Guid issueId);
        Task<IssueModel> CreateAsync(IssueModel model);
        Task<IssueCardSaveModel> UpdateAsync(IssueCardSaveModel issueCard);
        Task<IssueModel> UpdateAsync(IssueModel model);
        Task<IssueClosedModel> Closed(IssueClosedModel issueClosed);
    }
}
