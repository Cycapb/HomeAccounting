﻿using DomainModels.Model;
using Services.BaseInterfaces;
using System;

namespace Services
{
    public interface IAccountService : IQueryService<Account>, IQueryServiceAsync<Account>, ICommandServiceAsync<Account>, IDisposable
    {
        bool HasAnyDependencies(int accountId);

        bool HasEnoughMoney(Account account, decimal summ);
    }
}