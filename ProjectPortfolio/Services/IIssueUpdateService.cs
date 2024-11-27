using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IIssueUpdateService
    {
        Task<IssueCardSaveModel> UpdateAsync(IssueCardSaveModel issueCard);
        Task<IssueModel> UpdateAsync(IssueModel model);
        //Task<IEnumerable<IssueModel>> DetachTicketsFromSystemUser(Guid attendantId);
        Task<IssueModel> UpdateAsync(IssueForwardingModel issueForwarding);
    }
}
