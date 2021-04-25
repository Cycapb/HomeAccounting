using DomainModels.EntityORM.Core.Infrastructure;
using DomainModels.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.Core.Infrastructure.Identity;
using WebUI.Core.Infrastructure.Identity.Models;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Migrators
{
    public static class DatabaseMigrator
    {
        private const string AdminRole = "Administrators";
        private const string UserRole = "Users";

        public static void MigrateDatabaseAndSeed(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<AccountingContextCore>();
            context.Database.Migrate();

            InitializeNotificationMailBox(context);
            InitializeTypeOfFlow(context);

            context.SaveChanges();
        }

        public static async Task MigrateIdentityDatabaseAndSeed(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetRequiredService<AccountingIdentityDbContext>();
            context.Database.Migrate();

            await CreateUser(app, AdminRole, "Admin", "admin@local.com", "23we45rt");
            await CreateUser(app, UserRole, "Demo", "demo@mail.ru", "12qw34er");
        }

        private static async Task CreateUser(IApplicationBuilder applicationBuilder, string roleName, string userName, string email, string password)
        {
            var userManager = applicationBuilder.ApplicationServices.GetRequiredService<UserManager<AccountingUserModel>>();
            var user = await userManager.FindByNameAsync(userName);

            if (user == null)
            {
                await CreateRole(applicationBuilder, roleName);

                var newUser = new AccountingUserModel() { UserName = userName, Email = email };
                var result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, roleName).Wait();
                }
            }
        }

        private static async Task CreateRole(IApplicationBuilder applicationBuilder, string roleName)
        {
            var roleManager = applicationBuilder.ApplicationServices.GetRequiredService<RoleManager<IdentityRole>>();

            if (!roleManager.Roles.Any(x => x.Name == roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
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
                    Password = "Cb.cP^.t3P[8CWAz@",
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
