namespace HomeAccountingSystem_DAL.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<HomeAccountingSystem_DAL.Model.AccountingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "HomeAccountingSystem_DAL.Model.AccountingContext";
        }

        protected override void Seed(HomeAccountingSystem_DAL.Model.AccountingContext context)
        {

        }
    }
}
