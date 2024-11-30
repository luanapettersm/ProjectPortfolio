﻿using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Data;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Services
{
    public class ClientProjectService(IClientProjectRepository repository) : IClientProjectService
    {
        public async Task<ClientProjectModel> CreateAsync(ClientProjectModel model)
        {
           List<string> msgs = Validator(model);

            model.Client = null;
            return await repository.InsertAsync(model);
        }

        public async Task<ClientProjectModel> UpdateAsync(ClientProjectModel model)
        {
            var db = await repository.GetAll().AsNoTracking().Where(e => e.Id == model.Id).FirstOrDefaultAsync();
            if(db.Address != model.Address || db.Number != model.Number || db.City != model.City || db.ZipCode != model.ZipCode)
                throw new Exception("Detalhes do endereço não podem ser alterados.");

            return model;
        }

        public List<string> Validator(ClientProjectModel model)
        {
            var messages = new List<string>();
            if (string.IsNullOrEmpty(model.Title) && model.Title.Length < 3 || model.Title.Length > 50)
                messages.Add("O título deve ter entre 3 e 50 caracteres.");
            if (string.IsNullOrEmpty(model.Address))
                messages.Add("Necessário informar o endereço da obra.");
            if (model.Number.ToString() == null)
                messages.Add("Necessário informar o número.");
            if (string.IsNullOrEmpty(model.City))
                messages.Add("Necessário informar a cidade em que acontecerá a obra.");
            if (string.IsNullOrEmpty(model.ZipCode))
                messages.Add("Necessário informar o CEP.");

            return messages;
        }
    }
}