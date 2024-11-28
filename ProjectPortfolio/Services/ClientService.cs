using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using ProjectPortfolio.Services.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace ProjectPortfolio.Services
{
    internal class ClientService(IClientRepository repository, IIssueRepository issueRepository) : IClientService
    {
        public async Task<ClientModel> CreateAsync(ClientModel model)
        {
            if (string.IsNullOrEmpty(model.CPF) && string.IsNullOrEmpty(model.CNPJ))
                throw new UserFriendlyException("É necessário informar um CPF ou um CNPJ para o cliente.");

            if(model.CPF != null)
            {
                await ValidateCPFExists(model);
                if (await repository.GetAll().Where(e => e.CPF == model.CPF).AnyAsync())
                    throw new UserFriendlyException("Já existe cliente criado para este CPF.");
            }
            else if(model.CNPJ != null)
            {
                await ValidateCNPJExists(model);
                if (await repository.GetAll().Where(e => e.CNPJ == model.CNPJ).AnyAsync())
                    throw new UserFriendlyException("Já existe cliente criado para este CNPJ.");
            }

            ClientValidadtor(model);

            var result = await repository.InsertAsync(model);
            return result;
        }

        public async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            var dbClient = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            if (model.CPF != dbClient.CPF && string.IsNullOrEmpty(model.CNPJ))
                throw new UserFriendlyException("O CPF do cliente não pode ser alterado.");
            if(model.CNPJ != dbClient.CNPJ && string.IsNullOrEmpty(model.CPF))
                throw new UserFriendlyException("O CNPJ do cliente não pode ser alterado.");

            ClientValidadtor(model);

            var result = await repository.UpdateAsync(model);
            return model;
        }

        public static void ClientValidadtor(ClientModel model)
        {
            if (string.IsNullOrEmpty(model.ZipCode))
                throw new UserFriendlyException("O CEP é obrigatório.");
            if (string.IsNullOrEmpty(model.Address))
                throw new UserFriendlyException("O endereço é obrigatório.");
            if (string.IsNullOrEmpty(model.PhoneNumber))
                throw new UserFriendlyException("O número é obrigatório.");
            if (string.IsNullOrEmpty(model.City))
                throw new UserFriendlyException("A cidadde é obrigatória.");
            if (string.IsNullOrEmpty(model.State))
                throw new UserFriendlyException("O estado é obrigatório.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var issues = await issueRepository.GetAll().AsNoTracking().Where(e => e.ClientId == id && e.Status != Enumerators.IssueStatusEnum.Closed).ToListAsync();
            if (issues.Count > 0)
                throw new UserFriendlyException("Cliente está vinculado a atividade ativa e não pode ser excluído.");

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
                    throw new UserFriendlyException("CNPJ já existe");
                else if (!query.IsEnabled && query.Id != Guid.Empty)
                    throw new UserFriendlyException("CNPJ já existe em um registro desativado");
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
                    throw new UserFriendlyException("CPF já existe");
                else if (!query.IsEnabled && query.Id != Guid.Empty)
                    throw new UserFriendlyException("CPF já existe em um registro desativado");
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
