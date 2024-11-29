using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IIssueService
    {
        Task<bool> ValidateIssueIsOpened(Guid issueId);
        Task<IssueModel> CreateAsync(IssueModel model);
        Task<string> StatusChangedMessage(IssueStatusEnum firstStatus, IssueStatusEnum newerStatus);
        Task<IssueCardSaveModel> UpdateAsync(IssueCardSaveModel issueCard);
        Task<IssueModel> UpdateAsync(IssueModel model);
    }
}
