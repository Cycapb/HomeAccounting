namespace DomainModels.EntityORM.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialMigrationForDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                {
                    AccountID = c.Int(nullable: false, identity: true),
                    AccountName = c.String(nullable: false, maxLength: 50, unicode: false),
                    Cash = c.Decimal(nullable: false, storeType: "money"),
                    Use = c.Boolean(nullable: false),
                    UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                })
                .PrimaryKey(t => t.AccountID);

            CreateTable(
                "dbo.Debt",
                c => new
                {
                    DebtID = c.Int(nullable: false, identity: true),
                    TypeOfFlowId = c.Int(nullable: false),
                    AccountId = c.Int(nullable: false),
                    Summ = c.Decimal(nullable: false, storeType: "money"),
                    Person = c.String(nullable: false, maxLength: 200, unicode: false),
                    DateBegin = c.DateTime(nullable: false),
                    DateEnd = c.DateTime(),
                    UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                })
                .PrimaryKey(t => t.DebtID)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("dbo.TypeOfFlow", t => t.TypeOfFlowId, cascadeDelete: true)
                .Index(t => t.TypeOfFlowId)
                .Index(t => t.AccountId);

            CreateTable(
                "dbo.TypeOfFlow",
                c => new
                {
                    TypeID = c.Int(nullable: false, identity: true),
                    TypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                })
                .PrimaryKey(t => t.TypeID);

            CreateTable(
                "dbo.Category",
                c => new
                {
                    CategoryID = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 50, unicode: false),
                    TypeOfFlowID = c.Int(nullable: false),
                    UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                    ViewInPlan = c.Boolean(nullable: false),
                    Active = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.CategoryID)
                .ForeignKey("dbo.TypeOfFlow", t => t.TypeOfFlowID)
                .Index(t => t.TypeOfFlowID);

            CreateTable(
                "dbo.PayingItem",
                c => new
                {
                    ItemID = c.Int(nullable: false, identity: true),
                    CategoryID = c.Int(nullable: false),
                    Summ = c.Decimal(nullable: false, storeType: "money"),
                    Date = c.DateTime(nullable: false, storeType: "date"),
                    AccountID = c.Int(nullable: false),
                    Comment = c.String(maxLength: 1024, unicode: false),
                    UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                })
                .PrimaryKey(t => t.ItemID)
                .ForeignKey("dbo.Category", t => t.CategoryID)
                .ForeignKey("dbo.Account", t => t.AccountID)
                .Index(t => t.CategoryID)
                .Index(t => t.AccountID);

            CreateTable(
                "dbo.PayingItemProduct",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    PayingItemId = c.Int(nullable: false),
                    ProductId = c.Int(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => new { t.Id, t.PayingItemId })
                .ForeignKey("dbo.PayingItem", t => t.PayingItemId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.PayingItemId)
                .Index(t => t.ProductId);

            CreateTable(
                "dbo.Product",
                c => new
                {
                    ProductID = c.Int(nullable: false, identity: true),
                    ProductName = c.String(nullable: false, maxLength: 50, unicode: false),
                    CategoryID = c.Int(nullable: false),
                    UserID = c.String(nullable: false, maxLength: 50, unicode: false),
                    Description = c.String(maxLength: 50, unicode: false),
                })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);

            CreateTable(
                "dbo.OrderDetail",
                c => new
                {
                    ID = c.Int(nullable: false, identity: true),
                    OrderId = c.Int(nullable: false),
                    ProductId = c.Int(nullable: false),
                    ProductPrice = c.Decimal(storeType: "money"),
                    Quantity = c.Int(),
                })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);

            CreateTable(
                "dbo.Order",
                c => new
                {
                    OrderID = c.Int(nullable: false, identity: true),
                    OrderDate = c.DateTime(nullable: false),
                    UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                    Active = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.OrderID);

            CreateTable(
                "dbo.PlanItem",
                c => new
                {
                    PlanItemID = c.Int(nullable: false, identity: true),
                    CategoryId = c.Int(nullable: false),
                    Month = c.DateTime(nullable: false),
                    SummPlan = c.Decimal(nullable: false, storeType: "money"),
                    SummFact = c.Decimal(nullable: false, storeType: "money"),
                    UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                    Closed = c.Boolean(nullable: false),
                    IncomePlan = c.Decimal(nullable: false, storeType: "money"),
                    OutgoPlan = c.Decimal(nullable: false, storeType: "money"),
                    IncomeOutgoFact = c.Decimal(nullable: false, storeType: "money"),
                    BalanceFact = c.Decimal(nullable: false, storeType: "money"),
                    BalancePlan = c.Decimal(nullable: false, storeType: "money"),
                })
                .PrimaryKey(t => t.PlanItemID)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);

            CreateTable(
                "dbo.NotificationMailBox",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MailBoxName = c.String(nullable: false, maxLength: 50),
                    MailFrom = c.String(nullable: false, maxLength: 50),
                    UserName = c.String(nullable: false, maxLength: 50),
                    Password = c.String(nullable: false, maxLength: 1024),
                    Server = c.String(nullable: false, maxLength: 50),
                    Port = c.Int(nullable: false),
                    UseSsl = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.PayingItem", "AccountID", "dbo.Account");
            DropForeignKey("dbo.Debt", "TypeOfFlowId", "dbo.TypeOfFlow");
            DropForeignKey("dbo.Category", "TypeOfFlowID", "dbo.TypeOfFlow");
            DropForeignKey("dbo.PlanItem", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.PayingItem", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.PayingItemProduct", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderDetail", "ProductId", "dbo.Product");
            DropForeignKey("dbo.OrderDetail", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Product", "CategoryID", "dbo.Category");
            DropForeignKey("dbo.PayingItemProduct", "PayingItemId", "dbo.PayingItem");
            DropForeignKey("dbo.Debt", "AccountId", "dbo.Account");
            DropIndex("dbo.PlanItem", new[] { "CategoryId" });
            DropIndex("dbo.OrderDetail", new[] { "ProductId" });
            DropIndex("dbo.OrderDetail", new[] { "OrderId" });
            DropIndex("dbo.Product", new[] { "CategoryID" });
            DropIndex("dbo.PayingItemProduct", new[] { "ProductId" });
            DropIndex("dbo.PayingItemProduct", new[] { "PayingItemId" });
            DropIndex("dbo.PayingItem", new[] { "AccountID" });
            DropIndex("dbo.PayingItem", new[] { "CategoryID" });
            DropIndex("dbo.Category", new[] { "TypeOfFlowID" });
            DropIndex("dbo.Debt", new[] { "AccountId" });
            DropIndex("dbo.Debt", new[] { "TypeOfFlowId" });
            DropTable("dbo.NotificationMailBox");
            DropTable("dbo.PlanItem");
            DropTable("dbo.Order");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Product");
            DropTable("dbo.PayingItemProduct");
            DropTable("dbo.PayingItem");
            DropTable("dbo.Category");
            DropTable("dbo.TypeOfFlow");
            DropTable("dbo.Debt");
            DropTable("dbo.Account");
        }
    }
}
