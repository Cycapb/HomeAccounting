namespace DomainModels.Model
{
    using System.Data.Entity;

    public partial class AccountingContext : DbContext
    {
        public AccountingContext()
            : base("name=AccountingEntities")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AccountingContext,Migrations.Configuration>("AccountingEntities"));
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Debt> Debt { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<PaiyngItemProduct> PaiyngItemProduct { get; set; }
        public virtual DbSet<PayingItem> PayingItem { get; set; }
        public virtual DbSet<PlanItem> PlanItem { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<TypeOfFlow> TypeOfFlow { get; set; }
        public virtual DbSet<NotificationMailBox> NotificationMailBox { get; set; }
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
                .HasMany(e => e.PayingItem)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.PayingItem)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

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

            modelBuilder.Entity<PaiyngItemProduct>()
                .Property(e => e.Summ)
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

            modelBuilder.Entity<PayingItem>()
                .HasMany(e => e.PaiyngItemProduct)
                .WithRequired(e => e.PayingItem)
                .HasForeignKey(e => e.PayingItemID);

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

            modelBuilder.Entity<Product>()
                .HasMany(e => e.PaiyngItemProduct)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeOfFlow>()
                .Property(e => e.TypeName)
                .IsUnicode(false);

            modelBuilder.Entity<TypeOfFlow>()
                .HasMany(e => e.Category)
                .WithRequired(e => e.TypeOfFlow)
                .HasForeignKey(e => e.TypeOfFlowID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TypeOfFlow>()
                .HasMany(e => e.Debt)
                .WithRequired(e => e.TypeOfFlow)
                .HasForeignKey(e => e.TypeOfFlowId);
        }
    }
}
