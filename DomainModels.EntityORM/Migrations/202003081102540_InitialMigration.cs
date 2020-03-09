namespace DomainModels.EntityORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Account",
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
                "Debt",
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
                .ForeignKey("Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("TypeOfFlow", t => t.TypeOfFlowId, cascadeDelete: true)
                .Index(t => t.TypeOfFlowId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "TypeOfFlow",
                c => new
                    {
                        TypeID = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.TypeID);
            
            CreateTable(
                "Category",
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
                .ForeignKey("TypeOfFlow", t => t.TypeOfFlowID)
                .Index(t => t.TypeOfFlowID);
            
            CreateTable(
                "PayingItem",
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
                .ForeignKey("Category", t => t.CategoryID)
                .ForeignKey("Account", t => t.AccountID)
                .Index(t => t.CategoryID)
                .Index(t => t.AccountID);
            
            CreateTable(
                "PayingItemProduct",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayingItemId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.Id, t.PayingItemId })
                .ForeignKey("PayingItem", t => t.PayingItemId, cascadeDelete: true)
                .ForeignKey("Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.PayingItemId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Product",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false, maxLength: 50, unicode: false),
                        CategoryID = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(maxLength: 50, unicode: false),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("Category", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "OrderDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductPrice = c.Decimal(storeType: "money"),
                        Quantity = c.Int(),
                    })
                .PrimaryKey(t => new { t.ID, t.OrderId })
                .ForeignKey("Order", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "Order",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 50, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID);
            
            CreateTable(
                "PlanItem",
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
                .ForeignKey("Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "NotificationMailBox",
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
            DropForeignKey("PayingItem", "AccountID", "Account");
            DropForeignKey("Debt", "TypeOfFlowId", "TypeOfFlow");
            DropForeignKey("Category", "TypeOfFlowID", "TypeOfFlow");
            DropForeignKey("PlanItem", "CategoryId", "Category");
            DropForeignKey("PayingItem", "CategoryID", "Category");
            DropForeignKey("PayingItemProduct", "ProductId", "Product");
            DropForeignKey("OrderDetail", "ProductId", "Product");
            DropForeignKey("OrderDetail", "OrderId", "Order");
            DropForeignKey("Product", "CategoryID", "Category");
            DropForeignKey("PayingItemProduct", "PayingItemId", "PayingItem");
            DropForeignKey("Debt", "AccountId", "Account");
            DropIndex("PlanItem", new[] { "CategoryId" });
            DropIndex("OrderDetail", new[] { "ProductId" });
            DropIndex("OrderDetail", new[] { "OrderId" });
            DropIndex("Product", new[] { "CategoryID" });
            DropIndex("PayingItemProduct", new[] { "ProductId" });
            DropIndex("PayingItemProduct", new[] { "PayingItemId" });
            DropIndex("PayingItem", new[] { "AccountID" });
            DropIndex("PayingItem", new[] { "CategoryID" });
            DropIndex("Category", new[] { "TypeOfFlowID" });
            DropIndex("Debt", new[] { "AccountId" });
            DropIndex("Debt", new[] { "TypeOfFlowId" });
            DropTable("NotificationMailBox");
            DropTable("PlanItem");
            DropTable("Order");
            DropTable("OrderDetail");
            DropTable("Product");
            DropTable("PayingItemProduct");
            DropTable("PayingItem");
            DropTable("Category");
            DropTable("TypeOfFlow");
            DropTable("Debt");
            DropTable("Account");
        }
    }
}
