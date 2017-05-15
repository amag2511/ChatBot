namespace ChatBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpgradeDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.Time(nullable: false, precision: 7),
                        Description = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Helps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Command = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Attachments", "User_Id", c => c.Int());
            AddColumn("dbo.Messages", "User_Id", c => c.Int());
            AddColumn("dbo.Users", "ClientId", c => c.String());
            CreateIndex("dbo.Attachments", "User_Id");
            CreateIndex("dbo.Messages", "User_Id");
            AddForeignKey("dbo.Attachments", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Messages", "User_Id", "dbo.Users", "Id");
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "Phone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Phone", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String());
            DropForeignKey("dbo.Notifications", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Messages", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Attachments", "User_Id", "dbo.Users");
            DropIndex("dbo.Notifications", new[] { "User_Id" });
            DropIndex("dbo.Messages", new[] { "User_Id" });
            DropIndex("dbo.Attachments", new[] { "User_Id" });
            DropColumn("dbo.Users", "ClientId");
            DropColumn("dbo.Messages", "User_Id");
            DropColumn("dbo.Attachments", "User_Id");
            DropTable("dbo.Helps");
            DropTable("dbo.Notifications");
        }
    }
}
