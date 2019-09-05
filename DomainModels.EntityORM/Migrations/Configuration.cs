namespace DomainModels.EntityORM.Migrations
{
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections.Generic;
    using DomainModels.Model;

    internal sealed class Configuration : DbMigrationsConfiguration<AccountingContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "DomainModels.Model.AccountingContext";
        }

        protected override void Seed(AccountingContext context)
        {
            InitializeTypeOfFlow(context);
            InitializeNotificationMailBox(context);

            base.Seed(context);
        }

        private void InitializeNotificationMailBox(AccountingContext context)
        {
            if (!context.NotificationMailBoxes.Any())
            {
                var mailBox = new NotificationMailBox()
                {
                    MailBoxName = "Accounting",
                    MailFrom = "home.accounting@list.ru",
                    UserName = "home.accounting@list.ru",
                    Password = "23we45rt",
                    UseSsl = true,
                    Server = "smtp.list.ru",
                    Port = 587
                };

                context.NotificationMailBoxes.Add(mailBox);
            }
        }

        private void InitializeTypeOfFlow(AccountingContext context)
        {
            if (!context.TypeOfFlows.Any())
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
                context.TypeOfFlows.AddRange(typesOfFlow);
            }
        }
    }
}
