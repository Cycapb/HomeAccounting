namespace DomainModels.Migrations
{
    using DomainModels.Model;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AccountingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DomainModels.Model.AccountingContext";
        }

        protected override void Seed(AccountingContext context)
        {
            if (!context.TypeOfFlow.Any())
            {
                var typesOfFlow = new List<TypeOfFlow>()
                {
                    new TypeOfFlow()
                    {
                        TypeName = "Доход"
                    },
                     new TypeOfFlow()
                     {
                         TypeName = "Расход"
                     }
                };
                context.TypeOfFlow.AddRange(typesOfFlow);
            }
            
            base.Seed(context);
        }
    }
}
