namespace ChatBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAttachments : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Attachments", newName: "MediaElements");
            AddColumn("dbo.MediaElements", "ContentType", c => c.String());
            AddColumn("dbo.MediaElements", "ContentData", c => c.Binary());
            DropColumn("dbo.MediaElements", "UriAttachment");
            DropColumn("dbo.MediaElements", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MediaElements", "Description", c => c.String());
            AddColumn("dbo.MediaElements", "UriAttachment", c => c.String());
            DropColumn("dbo.MediaElements", "ContentData");
            DropColumn("dbo.MediaElements", "ContentType");
            RenameTable(name: "dbo.MediaElements", newName: "Attachments");
        }
    }
}
