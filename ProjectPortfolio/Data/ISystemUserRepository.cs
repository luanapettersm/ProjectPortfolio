﻿using ProjectPortfolio.Models;

namespace ProjectPortfolio.Data
{
    public interface ISystemUserRepository
    {
        IQueryable<SystemUserModel> GetAll();
        Task<SystemUserModel> InsertAsync(SystemUserModel model);
        Task<SystemUserModel> UpdateAsync(SystemUserModel model);
        Task DeleteAsync(Guid id);
    }
}