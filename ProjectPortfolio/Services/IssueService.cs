using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
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

        public async Task<IssueCardSaveModel> UpdateAsync(IssueCardSaveModel issueCard)
        {
            var systemUser = await systemUserRepository.GetAsync((Guid)issueCard.AttendantId);

            var issue = await repository.GetAsync(issueCard.Id);

            if (issue.DateClosed != null)
            {
                var allowedToBeMoved = DateTimeOffset.Now.AddDays(-7);

                if (issue.DateClosed != null && issue.DateClosed <= allowedToBeMoved)
                    throw new Exception("Atividade encerrada Nao pode ser editada.");

                issue.DateClosed = null;
            }

            if (issueCard.IsMovedInAttendancePanel && issue.AttendantId != null && issue.AttendantId != systemUser.Id)
                throw new Exception("Atividade ja possui atendente.");

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
    }
}
