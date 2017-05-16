namespace ChatBot.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OneToMany : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Attachments", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Messages", name: "User_Id", newName: "UserId");
            RenameColumn(table: "dbo.Notifications", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Attachments", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Messages", name: "IX_User_Id", newName: "IX_UserId");
            RenameIndex(table: "dbo.Notifications", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Notifications", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Messages", name: "IX_UserId", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Attachments", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Notifications", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Messages", name: "UserId", newName: "User_Id");
            RenameColumn(table: "dbo.Attachments", name: "UserId", newName: "User_Id");
        }
    }
}
