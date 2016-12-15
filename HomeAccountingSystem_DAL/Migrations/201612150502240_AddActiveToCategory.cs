namespace HomeAccountingSystem_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActiveToCategory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Category", "Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Category", "Active");
        }
    }
}
