namespace ProjectPortfolio.Services
{
    public interface IIssueService
    {
        Task<bool> ValidateIssueIsOpened(Guid issueId);
    }
}
