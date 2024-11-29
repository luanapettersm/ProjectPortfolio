﻿using Microsoft.EntityFrameworkCore;
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
            var messages = new ResponseModel<IssueModel> { ValidationMessages = model.CreateValidator()  };
            
            return model;
        }

        public async Task<bool> ValidateIssueIsOpened(Guid issueId)
        {
            var issue = await repository.GetAll().AsNoTracking().Where(e => e.Id == issueId).Select(e => new { e.DateClosed, e.Status }).FirstOrDefaultAsync();
            if (issue == null || issue.DateClosed != null || (issue.Status == IssueStatusEnum.Closed))
                return false;
            return true;
        }

        public async Task<string> StatusChangedMessage(IssueStatusEnum firstStatus, IssueStatusEnum newerStatus)
        {
            var previousStatus = await repository
                .GetAll()
                .Where(e => e.Status == firstStatus)
                .AsNoTracking()
                .Select(e => e.Status)
                .FirstOrDefaultAsync();
            var currentStatus = await repository
                .GetAll()
                .Where(e => e.Status == newerStatus)
                .AsNoTracking()
                .Select(e => e.Status)          
            .FirstOrDefaultAsync();

            return string.Format(
                    "O status de atendimento da atividade foi alterado de {0} para {1}.",
                    previousStatus,
                    currentStatus
                );
        }

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

            if (!await ValidateIssueIsOpened(db.Id))
                throw new Exception("Atividade encontra-se encerrada e não pode ser editada.");
            if (db.Title != model.Title)
                throw new Exception("Título não pode ser alterado.");
            if (db.ClientId != model.ClientId)
                throw new Exception("O cliente não pode ser alterado.");
            if (model.Priority.GetType() == null)
                throw new Exception("A prioridade é obrigatória.");

            var role = await systemUserRepository.GetAll().Where(e => e.Id == systemUser.Id).Select(e => e.SystemRole).FirstOrDefaultAsync();

            if (role != SystemRoleEnum.admin && db.Priority != model.Priority)
                throw new Exception("Prioridade da atividade só pode ser alterado pelo administrador.");

            await repository.UpdateAsync(db);

            return db;
        }
    }
}
