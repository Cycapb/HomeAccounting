namespace DomainModels.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class NotificationMailBox : DbMigration
    {
        public override void Up()
        {
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
            DropTable("dbo.NotificationMailBox");
        }
    }
}
