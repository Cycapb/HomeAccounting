using DomainModels.Model.Core;
using Microsoft.EntityFrameworkCore;

namespace DomainModels.EntityORM.Core.Infrastructure
{
    public class AccountingContextCore : DbContext
    {
        public AccountingContextCore(DbContextOptions<AccountingContextCore> options) : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Debt> Debts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<PayingItem> PayingItems { get; set; }
        public virtual DbSet<PlanItem> PlanItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<TypeOfFlow> TypeOfFlows { get; set; }
        public virtual DbSet<NotificationMailBox> NotificationMailBoxes { get; set; }
        public virtual DbSet<PayingItemProduct> PayingItemProducts { get; set; }
    }
}
