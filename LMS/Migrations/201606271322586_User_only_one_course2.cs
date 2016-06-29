namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_only_one_course2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "CourseId", "dbo.Courses");
            DropIndex("dbo.AspNetUsers", new[] { "CourseId" });
            RenameColumn(table: "dbo.AspNetUsers", name: "CourseId", newName: "Course_Id");
            AlterColumn("dbo.AspNetUsers", "Course_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Course_Id");
            AddForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses");
            DropIndex("dbo.AspNetUsers", new[] { "Course_Id" });
            AlterColumn("dbo.AspNetUsers", "Course_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.AspNetUsers", name: "Course_Id", newName: "CourseId");
            CreateIndex("dbo.AspNetUsers", "CourseId");
            AddForeignKey("dbo.AspNetUsers", "CourseId", "dbo.Courses", "Id", cascadeDelete: true);
        }
    }
}
