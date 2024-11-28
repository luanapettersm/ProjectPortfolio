using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class IssueUpdateService(ISystemUserRepository systemUserRepository, 
        IIssueRepository repository,
        IIssueNoteService issueNoteService,
        IIssueService service) : IIssueUpdateService
    {
        public async Task<IssueCardSaveModel> UpdateAsync(IssueCardSaveModel issueCard)
        {
            //var userId = await cookie.GetSystemUserId();
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
            if (issueStatus != issueCardStatus)
            {
                var changeStatusMessage = await service.StatusChangedMessage(issueStatus, issueCardStatus);
                var newNote = new IssueNoteModel
                {
                    Description = changeStatusMessage,
                    DateCreated = DateTimeOffset.Now,
                    SystemUserId = systemUser.Id,
                    IssueId = issueCard.Id
                };
                await issueNoteService.CreateAsync(newNote);
            }

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
            //var userId = await cookie.GetSystemUserId();
            var systemUser = await systemUserRepository.GetAsync((Guid)model.AttendantId);

            if (!await service.ValidateIssueIsOpened(db.Id))
                throw new Exception("Atividade encontra-se encerrada e não pode ser editada.");
            if (db.Title != model.Title)
                throw new Exception("Título não pode ser alterado.");
            if (db.ClientId != model.ClientId)
                throw new Exception("O cliente não pode ser alterado.");
            if (model.Priority == null)
                throw new Exception("A prioridade é obrigatória.");

            var role = await systemUserRepository.GetAll().Where(e => e.Id == systemUser.Id).Select(e => e.SystemRole).FirstOrDefaultAsync();

            if (role != SystemRoleEnum.admin && db.Priority != model.Priority)
                throw new Exception("Prioridade da atividade só pode ser alterado pelo administrador.");

            await repository.UpdateAsync(db);

            return db;
        }

        //public async Task<IEnumerable<IssueModel>> DetachTicketsFromSystemUser(Guid attendantId)
        //{
        //    var issues = (await repository.RetrieveInProgressIssuesPerAttendant(attendantId)).ToList();

        //    if (issues.Count == 0)
        //        return null;

        //    foreach (var t in issues)
        //    {
        //        t.AttendantId = null;
        //        t.Status = IssueStatusEnum.Opened;
        //    }

        //    return await repository.UpdateRangeAsync(issues);
        //}

        public async Task<IssueModel> UpdateAsync(IssueForwardingModel issueForwarding)
        {
            //var systemUserId = await cookie.GetSystemUserId();
            var systemUser = await systemUserRepository.GetAsync((Guid)issueForwarding.AttendantId);

            var db = await repository.GetAsync(issueForwarding.IssueId);
            if (db.DateClosed != null)
                throw new Exception("Atividade encerrada não pode ser editada.");

            db.AttendantId = issueForwarding.AttendantId;
            db.Status = IssueStatusEnum.Pending;
            db = await repository.UpdateAsync(db);

            var note = string.Format("A atividada foi encaminhada para o atendente {0}, observação:", issueForwarding.AttendantId.ToString(), issueForwarding.Description);

            await issueNoteService.CreateAsync(new IssueNoteModel
            {
                IssueId = db.Id,
                Description = note,
                DateCreated = DateTimeOffset.Now,
                //SystemUserId = await cookie.GetSystemUserId()
            });

            return db;
        }
    }
}