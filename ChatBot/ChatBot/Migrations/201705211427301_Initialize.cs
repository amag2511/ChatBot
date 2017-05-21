namespace ChatBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MediaElements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        ContentType = c.String(),
                        ContentData = c.Binary(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromId = c.String(),
                        FromName = c.String(),
                        ToId = c.String(),
                        ToName = c.String(),
                        ServiceUrl = c.String(),
                        ChannelId = c.String(),
                        ConversationId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Helps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Command = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "UserId", "dbo.Users");
            DropForeignKey("dbo.MediaElements", "UserId", "dbo.Users");
            DropIndex("dbo.Notifications", new[] { "UserId" });
            DropIndex("dbo.MediaElements", new[] { "UserId" });
            DropTable("dbo.Helps");
            DropTable("dbo.Notifications");
            DropTable("dbo.Users");
            DropTable("dbo.MediaElements");
        }
    }
}
