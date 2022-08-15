using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DomainModels.EntityORM.Core.Infrastructure
{
    public class AccountingContextCoreDesignTimeFactory : IDesignTimeDbContextFactory<AccountingContextCore>
    {
        public AccountingContextCore CreateDbContext(string[] args)
        {
            var connectionString =
                "Server=localhost;Initial Catalog=accounting;User ID=accounting_user;Password=Sw23456;MultipleActiveResultSets=True;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;";

            var assembly = GetType().Assembly;
            var optionsBuilder = new DbContextOptionsBuilder<AccountingContextCore>()
                .UseSqlServer(connectionString, options =>
                {
                    options.MigrationsAssembly(assembly.GetName().Name);
                    options.MigrationsHistoryTable("__EFMigrationsHistory");
                })
                .EnableSensitiveDataLogging();

            return new AccountingContextCore(optionsBuilder.Options);
        }
    }
}
