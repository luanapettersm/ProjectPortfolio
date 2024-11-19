using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface IIssueRepository
    {
        Task<FilterResponseModel<IssueModel>> FilterAsync(FilterRequestModel filter);
        IQueryable<IssueModel> GetAll();
        Task<IssueModel> InsertAsync(IssueModel model);
        Task<IssueModel> UpdateAsync(IssueModel model);
    }
}
