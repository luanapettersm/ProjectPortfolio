using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class SystemUserService(ISystemUserRepository repository, IIssueRepository issueRepository) : ISystemUserService
    {
        public async Task<SystemUserModel> CreateAsync(SystemUserModel model)
        {
            SystemUserValidator(model);
            
            model.DateCreated = DateTimeOffset.Now;

            var result = await repository.InsertAsync(model);
            return result;
        }

        public async Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
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

        private static void SystemUserValidator(SystemUserModel model)
        {
            if (model.Name.Length < 3 || model.Name.Length > 35 || model.Name.IsNullOrEmpty())
                throw new Exception("Nome deve ter entre 3 e 35 caracteres.");
            if (model.Surname.Length < 3 || model.Surname.Length > 100 || model.Surname.IsNullOrEmpty())
                throw new Exception("O sobrenome deve ter entre 3 e 100 caracteres.");
            if(model.UserName.Length < 3 || model.UserName.Length > 50 || model.Name.IsNullOrEmpty())
                throw new Exception("O login deve ter entre 3 e 50 caracteres.");
            if(model.BusinessRole.GetType() == null)
                throw new Exception("O cargo é obrigatório.");
        }
    }
}
  