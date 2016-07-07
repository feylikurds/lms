namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changed_Document_to_allow_uploads : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Documents", "UserId");
            AddForeignKey("dbo.Documents", "UserId", "dbo.AspNetUsers", "Id");
            DropColumn("dbo.Documents", "UploaderId");
            DropColumn("dbo.Documents", "DocumentName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "DocumentName", c => c.String());
            AddColumn("dbo.Documents", "UploaderId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Documents", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Documents", new[] { "UserId" });
            DropColumn("dbo.Documents", "UserId");
        }
    }
}
