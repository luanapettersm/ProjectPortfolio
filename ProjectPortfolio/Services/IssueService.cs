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
            List<string> msgs = Validator(model);
            
            return await repository.InsertAsync(model);
        }

        public async Task<bool> ValidateIssueIsOpened(Guid issueId)
        {
            var issue = await repository.GetAll().AsNoTracking().Where(e => e.Id == issueId).Select(e => new { e.DateClosed, e.Status }).FirstOrDefaultAsync();
            if (issue == null || issue.DateClosed != null || (issue.Status == IssueStatusEnum.Closed))
                return false;
            return true;
        }

        public async Task<IssueCardSaveModel> UpdateAsync(IssueCardSaveModel issueCard)
        {
            var systemUser = await systemUserRepository.GetAsync((Guid)issueCard.AttendantId);

            var issue = await repository.GetAsync(issueCard.Id);

            if (issue.DateClosed != null)
            {
                var allowedToBeMoved = DateTimeOffset.Now.AddDays(-7);

                if (issue.DateClosed != null && issue.DateClosed <= allowedToBeMoved)
                    throw new Exception("Atividade encerrada não pode ser editada.");

                issue.DateClosed = null;
            }

            if (issueCard.IsMovedInAttendancePanel && issue.AttendantId != null && issue.AttendantId != systemUser.Id)
                throw new Exception("Atividade já possui atendente.");

            var issueStatus = issue.Status;
            var issueCardStatus = issueCard.Status;

            issue.AttendantId = issueCard.AttendantId;

            if (!(issue.Status == IssueStatusEnum.Pending
                && (issueCard.Status == IssueStatusEnum.Pending || issueCard.Status == IssueStatusEnum.Opened)))
                issue.Status = issueCard.Status;

            var newIssue = await repository.UpdateAsync(issue);

            var result = new IssueCardSaveModel
            {
                Id = newIssue.Id,
                AttendantId = newIssue.AttendantId,
                Status = newIssue.Status,
            };

            return result;
        }

        public async Task<IssueModel> UpdateAsync(IssueModel model)
        {
            var db = await repository.GetAll().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            var systemUser = await systemUserRepository.GetAsync((Guid)model.AttendantId);

            if (!await ValidateIssueIsOpened(db.Id))
                throw new Exception("Atividade encontra-se encerrada e não pode ser editada.");
            if (db.Title != model.Title)
                throw new Exception("Título não pode ser alterado.");
            if (db.ClientId != model.ClientId)
                throw new Exception("O cliente não pode ser alterado.");
            if (model.Priority.GetType() == null)
                throw new Exception("A prioridade é obrigatória.");

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
                messages.Add("O cliente é obrigatório.");
            if (model.Priority.GetType() == null)
                messages.Add("A prioridade é obrigatória.");

            return messages;
        }
    }
}
