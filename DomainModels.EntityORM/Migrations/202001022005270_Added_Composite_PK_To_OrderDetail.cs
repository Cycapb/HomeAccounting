namespace DomainModels.EntityORM.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Added_Composite_PK_To_OrderDetail : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("OrderDetail");
            AddPrimaryKey("OrderDetail", new[] { "ID", "OrderId" });
        }

        public override void Down()
        {
            DropPrimaryKey("OrderDetail");
            AddPrimaryKey("OrderDetail", "ID");
        }
    }
}
