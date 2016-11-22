using HomeAccountingSystem_WebUI.Infrastructure;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HomeAccountingSystem_WebUI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HomeAccountingSystem_WebUI.Infrastructure.AccIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "HomeAccountingSystem_WebUI.Infrastructure.AccIdentityDbContext";
        }

        protected override void Seed(HomeAccountingSystem_WebUI.Infrastructure.AccIdentityDbContext context)
        {
            AccUserManager userManager = new AccUserManager(new UserStore<AccUserModel>(context));
            AccRoleManager roleManager = new AccRoleManager(new RoleStore<AccRoleModel>(context));

            var roleName = "Administrators";
            var userName = "Admin";
            var email = "admin@local.com";
            var password = "23we45rt";

            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new AccRoleModel(roleName));
            }

            var user = userManager.FindByName(userName);
            if (user == null)
            {
                userManager.Create(new AccUserModel() { UserName = userName, Email = email }, password);
                user = userManager.FindByName(userName);
            }

            if (!userManager.IsInRole(user.Id, roleName))
            {
                userManager.AddToRole(user.Id, roleName);
            }
        }
    }
}
