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
            if (!string.IsNullOrEmpty(model.Password))
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                model.Password = hashedPassword;
            }
            
            model.DateCreated = DateTimeOffset.Now;

            return await repository.InsertAsync(model);
        }

        public async Task<SystemUserModel> UpdateAsync(SystemUserModel model)
        {
            var db = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();

            if(model.Password != db.Password && !string.IsNullOrEmpty(model.Password))
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password);
                model.Password = hashedPassword;
            }

            model.DateCreated = db.DateCreated;

            var result = await repository.UpdateAsync(model);
            return result;
        }

        public async Task DeleteAsync(Guid id)
        {
            var issues = await issueRepository.GetAll().AsNoTracking().Where(e => e.AttendantId == id && e.Status != IssueStatusEnum.Closed).ToListAsync();
            if (issues.Count > 0)
                throw new Exception("usuario esta vinculado a atividade ativa e Nao pode ser deletado.");

            await repository.DeleteAsync(id);
        }

        public async Task<bool> AuthenticateAsync(string userName, string password)
        {
            var systemUser = await repository.GetUserByUserName(userName);

            if (systemUser == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, systemUser.Password);
        }
    }
}
