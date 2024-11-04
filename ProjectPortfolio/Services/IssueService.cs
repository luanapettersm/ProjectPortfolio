using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class IssueService(IIssueRepository repository) : IIssueService
    {
        public async Task<IssueModel> CreateAsync(IssueModel model)
        {
            IssueValidator(model);
            return model;
        }

        public async Task<IssueModel> UpdateAsync(IssueModel model)
        {
            return model;
        }

        public async Task<bool> ValidateIssueIsOpened(Guid issueId)
        {
            var issue = await repository.GetAll().AsNoTracking().Where(e => e.Id == issueId).Select(e => new { e.DateClosed, e.Status }).FirstOrDefaultAsync();
            if (issue == null || issue.DateClosed != null || (issue.Status == IssueStatusEnum.Closed))
                return false;
            return true;
        }

        private static void IssueValidator(IssueModel model)
        {
            if (model.Title.Length < 3 && model.Title.Length > 100 && !model.Title.IsNullOrEmpty())
                throw new Exception("O título deve ter entre 3 e 100 caracteres.");
            if (model.Description.Length < 3 && model.Description.Length > 4000 && !model.Description.IsNullOrEmpty())
                throw new Exception("O título deve ter entre 3 e 100 caracteres.");
            if (model.ClientId == Guid.Empty)
                throw new Exception("O cliente é obrigatório.");
        }
    }
}
