﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUI.Core.Models;

namespace WebUI.Core.Infrastructure.Identity
{
    public class AccountingIdentityDbContext : IdentityDbContext<AccountingUserModel>
    {
        public AccountingIdentityDbContext(DbContextOptions<AccountingIdentityDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
