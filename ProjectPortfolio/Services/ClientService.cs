using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;
using System.Net;
using System.Reflection.Emit;

namespace ProjectPortfolio.Services
{
    public class ClientService(IClientRepository repository, IIssueRepository issueRepository) : IClientService
    {
        public async Task<ClientModel> CreateAsync(ClientModel model)
        {
            if (string.IsNullOrEmpty(model.CPF) && string.IsNullOrEmpty(model.CNPJ))
                throw new Exception("É necessário informar um CPF ou um CNPJ para o cliente.");

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

            List<string> msgs = Validator(model);

            var result = await repository.InsertAsync(model);
            return result;
        }

        public async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            var dbClient = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            if (model.CPF != dbClient.CPF && string.IsNullOrEmpty(model.CNPJ))
                throw new Exception("O CPF do cliente não pode ser alterado.");
            if(model.CNPJ != dbClient.CNPJ && string.IsNullOrEmpty(model.CPF))
                throw new Exception("O CNPJ do cliente não pode ser alterado.");

            List<string> msgs = Validator(model);

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

        public List<string> Validator(ClientModel model)
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(model.ZipCode))
                messages.Add("O CEP é obrigatório.");
            if (string.IsNullOrEmpty(model.Address))
                messages.Add("O endereço é obrigatório.");
            if (string.IsNullOrEmpty(model.PhoneNumber))
                messages.Add("O número é obrigatório.");
            if (string.IsNullOrEmpty(model.City))
                messages.Add("A cidade é obrigatória.");
            if (string.IsNullOrEmpty(model.State))
                messages.Add("O estado é obrigatório.");

            return messages;
        }
    }
}
