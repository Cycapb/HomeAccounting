using System;
using System.Data.Entity.Infrastructure;

namespace DomainModels.EntityORM.Infrastructure
{
    public class AccountingContextCoreFactory : IDbContextFactory<AccountingContextCore>
    {
        private readonly IServiceProvider _serviceProvider;

        public AccountingContextCoreFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public AccountingContextCore Create() => (AccountingContextCore)_serviceProvider.GetService(typeof(AccountingContextCore));
    }
}
