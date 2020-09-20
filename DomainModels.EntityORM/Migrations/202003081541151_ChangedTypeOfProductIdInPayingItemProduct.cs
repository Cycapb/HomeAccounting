namespace DomainModels.EntityORM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTypeOfProductIdInPayingItemProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("PayingItemProduct", "ProductId", "Product");
            DropIndex("PayingItemProduct", new[] { "ProductId" });
            AlterColumn("PayingItemProduct", "ProductId", c => c.Int());
            CreateIndex("PayingItemProduct", "ProductId");
            AddForeignKey("PayingItemProduct", "ProductId", "Product", "ProductID");
        }
        
        public override void Down()
        {
            DropForeignKey("PayingItemProduct", "ProductId", "Product");
            DropIndex("PayingItemProduct", new[] { "ProductId" });
            AlterColumn("PayingItemProduct", "ProductId", c => c.Int(nullable: false));
            CreateIndex("PayingItemProduct", "ProductId");
            AddForeignKey("PayingItemProduct", "ProductId", "Product", "ProductID", cascadeDelete: true);
        }
    }
}
