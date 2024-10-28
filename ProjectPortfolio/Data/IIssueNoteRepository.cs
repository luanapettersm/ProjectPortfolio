using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IIssueNoteRepository
    {
        IQueryable<IssueNoteModel> GetAll();
        Task<IssueNoteModel> InsertAsync(IssueNoteModel model);
        Task<IssueNoteModel> UpdateAsync(IssueNoteModel model);
    }
}
