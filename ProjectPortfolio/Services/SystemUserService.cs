using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class SystemUserService(ISystemUserRepository repository) : ISystemUserService
    {
        public async Task<SystemUserModel> CreateAsync(SystemUserModel model)
        {
            SystemUserValidator(model);
            
            model.DateCreated = DateTimeOffset.Now;

            var result = await repository.InsertAsync(model);
            return result;
        }

        public Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id)
        {
            var query = repository.GetAll().Where(e => e.Id == id).FirstOrDefaultAsync();

            await repository.DeleteAsync(id);
        }

        private static void SystemUserValidator(SystemUserModel model)
        {
            if (model.Name.Length < 3 || model.Name.Length > 35 || model.Name.IsNullOrEmpty())
                throw new Exception("Nome deve ter entre {0} e {1} caracteres.");
            if (model.Surname.Length < 3 || model.Surname.Length > 100 || model.Surname.IsNullOrEmpty())
                throw new Exception("O sobrenome deve ter entre 3 e 100 caracteres.");
            if(model.UserName.Length < 3 || model.UserName.Length > 50 || model.Name.IsNullOrEmpty())
                throw new Exception("O login deve ter entre 3 e 50 caracteres.");
            if(model.BusinessRole.GetType() == null)
                throw new Exception("O cargo é obrigatório.");
        }
    }
}
  