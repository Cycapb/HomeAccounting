using System.Data.Entity;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class AccIdentityDbContext:IdentityDbContext<AccUserModel>
    {
        public AccIdentityDbContext() : base("accounting_identity") { }

        static AccIdentityDbContext()
        {
            System.Data.Entity.Database.SetInitializer<AccIdentityDbContext>(new MigrateDatabaseToLatestVersion<AccIdentityDbContext,Migrations.Configuration>());
        }

        public static AccIdentityDbContext Create()
        {
            return new AccIdentityDbContext();
        }
    }

}