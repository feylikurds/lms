namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_upload_fields : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Documents", name: "UserId", newName: "UploaderId");
            RenameIndex(table: "dbo.Documents", name: "IX_UserId", newName: "IX_UploaderId");
            AddColumn("dbo.Documents", "FileName", c => c.String(maxLength: 255));
            AddColumn("dbo.Documents", "ContentType", c => c.String(maxLength: 100));
            AddColumn("dbo.Documents", "Content", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Content");
            DropColumn("dbo.Documents", "ContentType");
            DropColumn("dbo.Documents", "FileName");
            RenameIndex(table: "dbo.Documents", name: "IX_UploaderId", newName: "IX_UserId");
            RenameColumn(table: "dbo.Documents", name: "UploaderId", newName: "UserId");
        }
    }
}
