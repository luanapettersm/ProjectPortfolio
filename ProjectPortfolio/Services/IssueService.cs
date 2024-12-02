using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class IssueService(IIssueRepository repository,
        ISystemUserRepository systemUserRepository) : IIssueService
    {
        public async Task<IssueModel> CreateAsync(IssueModel model)
        {
            model.Validator();
            var sequentialId = await repository.GetAll().OrderByDescending(e => e.SequentialId).Select(e => e.SequentialId).FirstAsync();
            model.SequentialId = sequentialId + 1;
            model.DateCreated = DateTimeOffset.Now;
            return await repository.InsertAsync(model);
        }

        public async Task<bool> ValidateIssueIsOpened(Guid issueId)
        {
            var issue = await repository.GetAll().AsNoTracking().Where(e => e.Id == issueId).Select(e => new { e.DateClosed, e.Status }).FirstOrDefaultAsync();
            if (issue == null || issue.DateClosed != null || (issue.Status == IssueStatusEnum.Closed))
                return false;
            return true;
        }

        public async Task<bool> ChangeStatusCard(Guid id, IssueStatusEnum status)
        {
            var issue = await repository.GetAll().Where(e => e.Id == id).FirstOrDefaultAsync();

            if (issue.Status == status)
                return false;

            if (status == IssueStatusEnum.Closed)
                issue.DateClosed = DateTimeOffset.Now;

            issue.Status = status;
            await repository.UpdateAsync(issue);

            return true;
        }

        public async Task<IssueModel> UpdateAsync(IssueModel model)
        {
            var db = await repository.GetAll().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            var systemUser = await systemUserRepository.GetAsync((Guid)model.AttendantId);

            if (!await ValidateIssueIsOpened(db.Id))
                throw new Exception("Atividade encontra-se encerrada e Nao pode ser editada.");
            if (db.Title != model.Title)
                throw new Exception("Título Nao pode ser alterado.");
            if (db.ClientId != model.ClientId)
                throw new Exception("O cliente Nao pode ser alterado.");
            if (model.Priority.GetType() == null)
                throw new Exception("A prioridade e obrigatoria.");

            await repository.UpdateAsync(db);

            return db;
        }

        public List<string> Validator(IssueModel model)
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(model.Title) || model.Title.Length < 3 || model.Title.Length > 100)
                messages.Add("O título deve ter entre 3 e 100 caracteres.");
            if (string.IsNullOrEmpty(model.Description) || model.Description.Length < 3 || model.Description.Length > 2000)
                messages.Add("O título deve ter entre 3 e 2000 caracteres.");
            if (model.ClientId == Guid.Empty)
                messages.Add("O cliente e obrigatorio.");
            if (model.Priority.GetType() == null)
                messages.Add("A prioridade e obrigatoria.");

            return messages;
        }
    }
}
