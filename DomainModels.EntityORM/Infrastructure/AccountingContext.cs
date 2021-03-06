namespace DomainModels.EntityORM
{
    using System.Data.Entity;
    using DomainModels.Model;

    public partial class AccountingContext : DbContext
    {
        public AccountingContext()
            : base("name=AccountingEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AccountingContext, Migrations.Configuration>());
        }

        public AccountingContext(string name) : base(name)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AccountingContext, Migrations.Configuration>());
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
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.AccountName)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.Cash)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Account>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.PayingItems)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.PayingItems)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.PlanItems)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Debt>()
                .Property(e => e.Summ)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Debt>()
                .Property(e => e.Person)
                .IsUnicode(false);

            modelBuilder.Entity<Debt>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.ProductPrice)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayingItem>()
                .Property(e => e.Summ)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PayingItem>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<PayingItem>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.SummPlan)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.SummFact)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.IncomePlan)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.OutgoPlan)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.IncomeOutgoFact)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.BalanceFact)
                .HasPrecision(19, 4);

            modelBuilder.Entity<PlanItem>()
                .Property(e => e.BalancePlan)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.ProductName)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.UserID)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<TypeOfFlow>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<TypeOfFlow>()
                .HasMany(e => e.Categories)
                .WithRequired(e => e.TypeOfFlow)
                .HasForeignKey(e => e.TypeOfFlowID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeOfFlow>()
                .HasMany(e => e.Debts)
                .WithRequired(e => e.TypeOfFlow)
                .HasForeignKey(e => e.TypeOfFlowId);
        }
    }
}
