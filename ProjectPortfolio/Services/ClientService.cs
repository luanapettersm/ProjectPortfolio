using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class ClientService(Repository repository) : IClientService
    {
        public async Task<ClientModel> CreateAsync(ClientModel model)
        {
            model.RemoveMasks();
            await ValidateCNPJExists(model);
            await ValidateCPFExists(model);
             
            return model;
        }

        public async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            model.RemoveMasks();
            await ValidateCNPJExists(model);
            await ValidateCPFExists(model);

            return model;
        }

        public async Task<ClientModel> DeleteAsync(Guid id)
        {
            var query = await repository.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();
            return query;
        }

        public Task<ClientModel> FilterAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ClientModel>> GetAllAsync()
        {
            return await repository.Clients.ToListAsync();
        }

        public async Task<ClientModel> GetAsync(Guid id)
        {
            return await repository.Clients.Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        private async Task<ClientModel> ValidateCNPJExists(ClientModel model)
        {
            var query = await repository.Clients.AsNoTracking()
                .Where(e => e.CNPJNumber == model.CNPJNumber && e.Id != model.Id)
                .Select(e => new { e.Id, e.CNPJ, e.IsEnabled }).FirstOrDefaultAsync();

            if(query != null)
            {
                if (!query.IsEnabled)
                    throw new Exception("CNPJ já existe");
                else if (query.IsEnabled && query.Id != Guid.Empty)
                    throw new Exception("CNPJ já existe em um registro desativado");
                else
                {
                    model.Id = query.Id;
                    model.IsEnabled = true;
                }
            }
            return model;
        }

        private async Task<ClientModel> ValidateCPFExists(ClientModel model)
        {
            var query = await repository.Clients.AsNoTracking()
                .Where(e => e.CPFNumber == model.CPFNumber && e.Id != model.Id)
                .Select(e => new { e.Id, e.CNPJ, e.IsEnabled }).FirstOrDefaultAsync();

            if (query != null)
            {
                if (!query.IsEnabled)
                    throw new Exception("CPF já existe");
                else if (query.IsEnabled && query.Id != Guid.Empty)
                    throw new Exception("CPF já existe em um registro desativado");
                else
                {
                    model.Id = query.Id;
                    model.IsEnabled = true;
                }
            }
            return model;
        }
    }
}
