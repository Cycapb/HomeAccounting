using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HomeAccountingSystem_WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace HomeAccountingSystem_WebUI.Infrastructure
{
    public class AccRoleManager:RoleManager<AccRoleModel>
    {
        public AccRoleManager(RoleStore<AccRoleModel> store) : base(store)
        {

        }

        public static AccRoleManager Create(IdentityFactoryOptions<AccRoleManager> options, IOwinContext context)
        {
            return new AccRoleManager(new RoleStore<AccRoleModel>(context.Get<AccIdentityDbContext>()));
        }
    }
}