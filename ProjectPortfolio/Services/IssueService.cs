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
            if (model.Title.Length < 3 && model.Title.Length > 100 && !string.IsNullOrEmpty(model.Title))
                throw new Exception("O título deve ter entre 3 e 100 caracteres.");
            if (model.Description.Length < 3 && model.Description.Length > 2000 && !string.IsNullOrEmpty(model.Description))
                throw new Exception("O título deve ter entre 3 e 2000 caracteres.");
            if (model.ClientId == Guid.Empty)
                throw new Exception("O cliente é obrigatório.");
            if (model.Priority == null)
                throw new Exception("A prioridade é obrigatória.");

            var db = await repository.GetAll().AsNoTracking().OrderByDescending(e => e.SequentialId).LastAsync();
            return model;
        }

        public async Task<IssueModel> UpdateAsync(IssueModel model)
        {
            if (!await ValidateIssueIsOpened(model.Id))
                throw new Exception("Atividade encontra-se encerrada e não pode ser editada.");

            var db = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id && model.Status != IssueStatusEnum.Closed).FirstOrDefaultAsync();
            if (db.Title != model.Title)
                throw new Exception("Título não pode ser alterado.");
            if (db.ClientId != model.ClientId)
                throw new Exception("O cliente não pode ser alterado.");
            if (model.Priority == null)
                throw new Exception("A prioridade é obrigatória.");

            return model;
        }

        public async Task<bool> ValidateIssueIsOpened(Guid issueId)
        {
            var issue = await repository.GetAll().AsNoTracking().Where(e => e.Id == issueId).Select(e => new { e.DateClosed, e.Status }).FirstOrDefaultAsync();
            if (issue == null || issue.DateClosed != null || (issue.Status == IssueStatusEnum.Closed))
                return false;
            return true;
        }
    }
}
