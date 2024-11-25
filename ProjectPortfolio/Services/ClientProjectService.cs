using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class ClientProjectService(IClientProjectRepository repository) : IClientProjectService
    {
        public async Task<ClientProjectModel> CreateAsync(ClientProjectModel model)
        {
            if (model.Title.Length < 3 || model.Title.Length > 50 || string.IsNullOrEmpty(model.Title))
                throw new Exception("O título deve ter entre 3 e 50 caracteres.");
            if (string.IsNullOrEmpty(model.Address))
                throw new Exception("Necessário informar o endereço da obra.");
            if (model.Number == null)
                throw new Exception("Necessário informar o número.");
            if (string.IsNullOrEmpty(model.City))
                throw new Exception("Necessário informar a cidade em que acontecerá a obra.");
            if (string.IsNullOrEmpty(model.ZipCode))
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