using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class ClientService(IClientRepository repository) : IClientService
    {
        public async Task<ClientModel> CreateAsync(ClientModel model)
        {
            model.RemoveMasks();
            await ValidateCNPJExists(model);
            await ValidateCPFExists(model);

            if (await repository.GetAll().Where(e => e.Name.Equals(model.Name, StringComparison.CurrentCultureIgnoreCase) && e.Id != model.Id).AnyAsync())
                throw new Exception("Nome já existe.");

            return model;
        }

        public async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            model.RemoveMasks();
            await ValidateCNPJExists(model);
            await ValidateCPFExists(model);

            return model;
        }

        public async Task DeleteAsync(Guid id)
        {
            await repository.DeleteAsync(id);
        }

        private async Task<ClientModel> ValidateCNPJExists(ClientModel model)
        {
            var query = await repository.GetAll().AsNoTracking()
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
            var query = await repository.GetAll().AsNoTracking()
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
