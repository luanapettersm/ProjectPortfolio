﻿using Microsoft.EntityFrameworkCore;
using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    internal class ClientRepository(IDbContextFactory<Repository> dbContextFactory) : IClientRepository
    {
        public async Task<FilterResponseModel<ClientModel>> FilterAsync(FilterRequestModel filter)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var query = ct.Set<ClientModel>().AsQueryable();

            if (!string.IsNullOrEmpty(filter.Search))
            {
                query = query.Where(e => e.Name.Contains(filter.Search));
            }

            var totalRecords = await query.CountAsync();
            var filteredRecords = await query.CountAsync();

            var result = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize).ToList();

            return new FilterResponseModel<ClientModel>
            {
                Total = totalRecords,
                FilteredRecords = filteredRecords,
                Data = result
            };
        }

        public async Task DeleteAsync(Guid id)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            await ct.Set<ClientModel>().Where(e => e.Id == id).ExecuteDeleteAsync();
            await ct.SaveChangesAsync();
        }

        public IQueryable<ClientModel> GetAll()
        {
            var ct = dbContextFactory.CreateDbContext();
            return ct.Set<ClientModel>();
        }

        public async Task<ClientModel> InsertAsync(ClientModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = await ct.AddAsync(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<ClientModel> UpdateAsync(ClientModel model)
        {
            var ct = await dbContextFactory.CreateDbContextAsync();
            var result = ct.Update(model);
            await ct.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<IEnumerable<ClientModel>> GetListAsync()
        {
            var ct = await dbContextFactory.CreateDbContextAsync();

            return await ct.Set<ClientModel>().Where(e => e.IsEnabled).OrderBy(e => e.Name).Select(e => new ClientModel
            {
                Id = e.Id,
                Name = e.Name
            }).ToListAsync();
        } 
    }
}
