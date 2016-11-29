using HomeAccountingSystem_WebUI.Infrastructure;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HomeAccountingSystem_WebUI.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<HomeAccountingSystem_WebUI.Infrastructure.AccIdentityDbContext>
    {
        private AccUserManager _userManager;
        private AccRoleManager _roleManager;

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "HomeAccountingSystem_WebUI.Infrastructure.AccIdentityDbContext";
        }

        protected override void Seed(HomeAccountingSystem_WebUI.Infrastructure.AccIdentityDbContext context)
        {
            _userManager = new AccUserManager(new UserStore<AccUserModel>(context));
            _roleManager = new AccRoleManager(new RoleStore<AccRoleModel>(context));

            var roleName = "Administrators";
            var userName = "Admin";
            var email = "admin@local.com";
            var password = "23we45rt";

            CreateUser(roleName,userName,email,password);
        }

        private void CreateUser(string roleName, string userName, string email, string password)
        {
            if (!_roleManager.RoleExists(roleName))
            {
                _roleManager.Create(new AccRoleModel(roleName));
            }

            var user = _userManager.FindByName(userName);
            if (user == null)
            {
                _userManager.Create(new AccUserModel() { UserName = userName, Email = email }, password);
                user = _userManager.FindByName(userName);
            }

            if (!_userManager.IsInRole(user.Id, roleName))
            {
                _userManager.AddToRole(user.Id, roleName);
            }
        }
    }
}
