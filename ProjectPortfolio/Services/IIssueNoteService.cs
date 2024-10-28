using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public interface IIssueNoteService
    {
        Task<IssueNoteModel> CreateAsync(IssueNoteModel model);
    }
}
