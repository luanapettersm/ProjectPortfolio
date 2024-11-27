using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    internal class ClientService(IClientRepository repository, IIssueRepository issueRepository) : IClientService
    {
        public async Task<ClientModel> CreateAsync(ClientModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CPF) && string.IsNullOrWhiteSpace(model.CNPJ))
                throw new ArgumentException("É necessário informar o CPF ou o CNPJ.");

            if(model.CPF != null)
            {
                await ValidateCPFExists(model);
                if (await repository.GetAll().Where(e => e.CPF == model.CPF).AnyAsync())
                    throw new Exception("Já existe cliente criado para este CPF.");
            }
            else if(model.CNPJ != null)
            {
                await ValidateCNPJExists(model);
                if (await repository.GetAll().Where(e => e.CNPJ == model.CNPJ).AnyAsync())
                    throw new Exception("Já existe cliente criado para este CNPJ.");
            }

            var result = await repository.InsertAsync(model);
            return result;
        }

        public async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            var dbClient = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            if (model.CPF != dbClient.CPF)
                throw new Exception("O CPF do cliente não pode ser alterado.");
            if(model.CNPJ != dbClient.CNPJ)
                throw new Exception("O CPF do cliente não pode ser alterado.");

            var result = await repository.UpdateAsync(model);
            return model;
        }

        public async Task DeleteAsync(Guid id)
        {
            var issues = await issueRepository.GetAll().AsNoTracking().Where(e => e.ClientId == id && e.Status != Enumerators.IssueStatusEnum.Closed).ToListAsync();
            if (issues.Count > 0)
                throw new Exception("Cliente está vinculado a atividade ativa e não pode ser excluído.");

            await repository.DeleteAsync(id);
        }

        private async Task<ClientModel> ValidateCNPJExists(ClientModel model)
        {
            var query = await repository.GetAll().AsNoTracking()
                .Where(e => e.CNPJ == model.CNPJ && e.Id != model.Id)
                .Select(e => new { e.Id, e.CNPJ, e.IsEnabled }).FirstOrDefaultAsync();

            if(query != null)
            {
                if (query.IsEnabled)
                    throw new Exception("CNPJ já existe");
                else if (!query.IsEnabled && query.Id != Guid.Empty)
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
                .Where(e => e.CPF == model.CPF && e.Id != model.Id)
                .Select(e => new { e.Id, e.CNPJ, e.IsEnabled }).FirstOrDefaultAsync();

            if (query != null)
            {
                if (query.IsEnabled)
                    throw new Exception("CPF já existe");
                else if (!query.IsEnabled && query.Id != Guid.Empty)
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
