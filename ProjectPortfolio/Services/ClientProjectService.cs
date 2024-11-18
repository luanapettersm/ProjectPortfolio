using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class ClientProjectService(ClientProjectRepository repository) : IClientProjectService
    {
        public async Task<ClientProjectModel> CreateAsync(ClientProjectModel model)
        {
            if (model.Title.Length < 3 || model.Title.Length > 50 || model.Title.IsNullOrEmpty())
                throw new Exception("O título deve ter entre 3 e 50 caracteres.");
            if (model.Address.IsNullOrEmpty())
                throw new Exception("Necessário informar o endereço da obra.");
            if (model.Number == null)
                throw new Exception("Necessário informar o número.");
            if (model.City.IsNullOrEmpty())
                throw new Exception("Necessário informar a cidade em que acontecerá a obra.");
            if (model.ZipCode.IsNullOrEmpty())
                throw new Exception("Necessário informar o CEP.");

            model.Client = null;
            var result = await repository.InsertAsync(model);
            return result;
        }

        public async Task<ClientProjectModel> UpdateAsync(ClientProjectModel model)
        {
            var db = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            if(db.Address != model.Address || db.Number != model.Number || db.City != model.City || db.ZipCode != model.ZipCode)
                throw new Exception("Detalhes do endereço não podem ser alterados.");

            return model;
        }
    }
}