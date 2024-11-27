using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IIssueClosedService
    {
        Task<IssueClosedModel> Closed(IssueClosedModel issueClosed);
    }
}
