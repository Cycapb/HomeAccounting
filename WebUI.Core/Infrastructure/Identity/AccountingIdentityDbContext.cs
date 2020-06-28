using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebUI.Core.Infrastructure.Identity.Models;

namespace WebUI.Core.Infrastructure.Identity
{
    public class AccountingIdentityDbContext : IdentityDbContext<AccountingUserModel>
    {
        public AccountingIdentityDbContext(DbContextOptions<AccountingIdentityDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
    }
}
