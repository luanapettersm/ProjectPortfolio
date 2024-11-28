using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class SystemUserService(ISystemUserRepository repository, IIssueRepository issueRepository) : ISystemUserService
    {
        public async Task<SystemUserModel> CreateAsync(SystemUserModel model)
        {
            Validator(model);
            
            model.DateCreated = DateTimeOffset.Now;

            var result = await repository.InsertAsync(model);
            return result;
        }

        public async Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
            var db = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            Validator(model);

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

        private static void Validator(SystemUserModel model)
        {
            //var messages = new ResponseModel();
            if (model.Name.Length < 3 || model.Name.Length > 35 || string.IsNullOrEmpty(model.Name))
                throw new Exception("Nome deve ter entre 3 e 35 caracteres.");
            if (model.Surname.Length < 3 || model.Surname.Length > 100 || string.IsNullOrEmpty(model.Surname))
                throw new Exception("O sobrenome deve ter entre 3 e 100 caracteres.");
            if(model.UserName.Length < 3 || model.UserName.Length > 50 || string.IsNullOrEmpty(model.Name))
                throw new Exception("O login deve ter entre 3 e 50 caracteres.");
            if(model.Password == null)
                throw new Exception("A senha é obrigatória.");
            if (model.BusinessRole.GetType() == null)
                throw new Exception("O cargo é obrigatório.");
        }
    }
}
  