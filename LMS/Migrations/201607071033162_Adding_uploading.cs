namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adding_uploading : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "ObjectId", c => c.Int(nullable: false));
            AddColumn("dbo.Documents", "FileName", c => c.String(maxLength: 255));
            AddColumn("dbo.Documents", "ContentType", c => c.String(maxLength: 100));
            AddColumn("dbo.Documents", "Content", c => c.Binary());
            AlterColumn("dbo.Documents", "UploaderId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Documents", "UploaderId");
            AddForeignKey("dbo.Documents", "UploaderId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Documents", "DocumentName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "DocumentName", c => c.String());
            DropForeignKey("dbo.Documents", "UploaderId", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "UploaderId" });
            AlterColumn("dbo.Documents", "UploaderId", c => c.Int(nullable: false));
            DropColumn("dbo.Documents", "Content");
            DropColumn("dbo.Documents", "ContentType");
            DropColumn("dbo.Documents", "FileName");
            DropColumn("dbo.Documents", "ObjectId");
        }
    }
}
