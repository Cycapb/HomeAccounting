using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Core.Infrastructure.Migrators
{
    public static class DatabaseMigrator
    {
        public static void MigrateAndSeed(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<AccountingContextCore>();
            context.Database.Migrate();

            InitializeNotificationMailBox(context);
            InitializeTypeOfFlow(context);

            context.SaveChanges();
        }

        private static void InitializeNotificationMailBox(AccountingContextCore context)
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

        private static void InitializeTypeOfFlow(AccountingContextCore context)
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
