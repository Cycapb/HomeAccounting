using DomainModels.Model.Core;
using Microsoft.EntityFrameworkCore;

namespace DomainModels.EntityORM.Core.Infrastructure
{
    public class AccountingContextCore : DbContext
    {
        public AccountingContextCore(DbContextOptions<AccountingContextCore> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
    }
}
