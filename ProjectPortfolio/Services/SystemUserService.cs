using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Enumerators;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class SystemUserService(ISystemUserRepository repository, IIssueRepository issueRepository) : ISystemUserService
    {
        public async Task<SystemUserModel> CreateAsync(SystemUserModel model)
        {
            List<string> msg = Validator(model);

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
            model.Password = hashedPassword;
            model.DateCreated = DateTimeOffset.Now;

            return await repository.InsertAsync(model);
        }

        public async Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
            var db = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            List<string> msg = Validator(model);

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

        public async Task<bool> AuthenticateAsync(string userName, string password)
        {
            var systemUser = await repository.GetUserByUserName(userName);

            if (systemUser == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, systemUser.Password);
        }

        public List<string> Validator(SystemUserModel model)
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 3 || model.Name.Length > 35)
                messages.Add("Nome deve ter entre 3 e 35 caracteres.");
            if (string.IsNullOrEmpty(model.Surname) || model.Surname.Length < 3 || model.Surname.Length > 100)
                messages.Add("O sobrenome deve ter entre 3 e 100 caracteres.");
            if (string.IsNullOrEmpty(model.UserName) || model.UserName.Length < 3 || model.UserName.Length > 50)
                messages.Add("O login deve ter entre 3 e 50 caracteres.");
            if (model.Password == null)
                messages.Add("A senha é obrigatória.");
            if (model.BusinessRole.GetType() == null)
                messages.Add("O cargo é obrigatório.");

            return messages;
        }
    }
}
