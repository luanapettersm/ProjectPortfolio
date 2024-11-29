using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectPortfolio.Services
{
    public class SystemUserService(ISystemUserRepository repository, IIssueRepository issueRepository) : ISystemUserService
    {
        public async Task<SystemUserModel> CreateAsync(SystemUserModel model)
        {
            var messages = new ResponseModel<SystemUserModel> { ValidationMessages = model.Validator() };
         
            model.DateCreated = DateTimeOffset.Now;

            var result = await repository.InsertAsync(model);
            return result;
        }

        public async Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
            var db = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            var messages = new ResponseModel<SystemUserModel> { ValidationMessages = model.Validator() };

            model.DateCreated = db.DateCreated;

            var result = await repository.UpdateAsync(model);   
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var issues = await issueRepository.GetAll().AsNoTracking().Where(e => e.AttendantId == id && e.Status != IssueStatusEnum.Closed).ToListAsync();
            if (issues.Count > 0)
                throw new Exception("Usuário está vinculado a atividade ativa e não pode ser deletado.");

            await repository.DeleteAsync(id);
        }
    }
}
  