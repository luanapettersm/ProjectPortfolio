using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;

namespace ProjectPortfolio.Services
{
    public class IssueService(IIssueRepository repository) : IIssueService
    {
        public async Task<bool> ValidateIssueIsOpened(Guid issueId)
        {
            var issue = await repository.GetAll().AsNoTracking().Where(e => e.Id == issueId).Select(e => new { e.DateClosed, e.Status }).FirstOrDefaultAsync();
            if (issue == null || issue.DateClosed != null || (issue.Status == IssueStatusEnum.Closed))
                return false;
            return true;
        }
    }
}
