using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class IssueClosedService(IIssueRepository repository,
        ISystemUserRepository systemUserRepository) : IIssueClosedService
    {
        public async Task<IssueClosedModel> Closed(IssueClosedModel issueClosed)
        {
            var db = await repository.GetAll().Where(e => e.Id == issueClosed.IssueId).FirstOrDefaultAsync();
            //var userId = await cookie.GetSystemUserId();
            var systemUser = await systemUserRepository.GetAsync((Guid)issueClosed.AttendantId);
            db.Solution = issueClosed.Solution;
            db.AttendantId = systemUser.Id;
            db.Status = Enumerators.IssueStatusEnum.Closed;

            if (db.DateClosed != null)
                throw new Exception("Atividade encerrada Nao pode ser editada.");

            if (issueClosed.Solution != null && issueClosed.Solution.Length < 20 || issueClosed.Solution.Length > 4000)
                throw new Exception("A solucao da atividade deve ter entre 20 e 4000 caracteres.");

            db.DateClosed = DateTimeOffset.Now;

            await repository.UpdateAsync(db);
            return issueClosed;
        }
    }
}