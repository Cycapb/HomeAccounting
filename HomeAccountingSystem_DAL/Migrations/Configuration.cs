namespace DomainModels.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DomainModels.Model.AccountingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DomainModels.Model.AccountingContext";
        }

        protected override void Seed(DomainModels.Model.AccountingContext context)
        {

        }
    }
}
