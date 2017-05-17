namespace ChatBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpMediaEl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MediaElements", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MediaElements", "Description");
        }
    }
}
