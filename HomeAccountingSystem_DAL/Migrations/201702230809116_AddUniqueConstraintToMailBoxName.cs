namespace DomainModels.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUniqueConstraintToMailBoxName : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.NotificationMailBox", "MailBoxName", unique: true, name: "UQ_NotificationMailBox_MailBoxName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.NotificationMailBox", "UQ_NotificationMailBox_MailBoxName");
        }
    }
}
