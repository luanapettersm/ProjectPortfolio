using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class IssueNoteService(IIssueService issueService,
        IIssueNoteRepository repository) : IIssueNoteService
    {
        public async Task<IssueNoteModel> CreateAsync(IssueNoteModel model)
        {
            if (!await issueService.ValidateIssueIsOpened(model.IssueId))
                throw new Exception("Atividade encontra-se encerrada.");

            model.DateCreated = DateTimeOffset.Now;
            model.Issue = null;
            if ((model.Description.Length < 5 && model.Description.Length > 2000) || model.Description.IsNullOrEmpty())
                throw new Exception("A descrição deve ter entre 5 e 2000 caracteres");
            return await repository.InsertAsync(model);
        }

        public async Task<IssueNoteModel> UpdateAsync(IssueNoteModel model)
        {
            var query = await repository.GetAll().Where(e => e.Id == model.IssueId).FirstOrDefaultAsync();

            if (model.SystemUserId != query.SystemUserId)
                throw new Exception("Apenas quem adicionou a nota pode editá-la.");

            model.DateCreated = query.DateCreated;
            return await repository.UpdateAsync(model);
        }
    }
}