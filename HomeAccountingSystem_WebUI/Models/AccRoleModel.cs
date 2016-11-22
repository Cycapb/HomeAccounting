using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HomeAccountingSystem_WebUI.Models
{
    public class AccRoleModel:IdentityRole
    {
        public AccRoleModel() : base() { }
        public AccRoleModel(string name) : base(name) { }
    }
}