using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class ClientProjectService(IClientProjectRepository repository) : IClientProjectService
    {
        public async Task<ClientProjectModel> CreateAsync(ClientProjectModel model)
        {
           var messages = new ResponseModel<ClientProjectModel> { ValidationMessages = model.CreateValidator() };
            if (messages.IsError)
                return null;

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