namespace ChatBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveMessages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "UserId", "dbo.Users");
            DropIndex("dbo.Messages", new[] { "UserId" });
            AddColumn("dbo.Users", "ConversationId", c => c.String());
            DropColumn("dbo.Users", "ClientId");
            DropTable("dbo.Messages");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tag = c.String(),
                        BotsMessage = c.String(),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Users", "ClientId", c => c.String());
            DropColumn("dbo.Users", "ConversationId");
            CreateIndex("dbo.Messages", "UserId");
            AddForeignKey("dbo.Messages", "UserId", "dbo.Users", "Id");
        }
    }
}
