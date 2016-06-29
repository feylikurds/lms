namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class User_only_one_course3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses");
            DropIndex("dbo.AspNetUsers", new[] { "Course_Id" });
            RenameColumn(table: "dbo.AspNetUsers", name: "Course_Id", newName: "CourseId");
            AlterColumn("dbo.AspNetUsers", "CourseId", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "CourseId");
            AddForeignKey("dbo.AspNetUsers", "CourseId", "dbo.Courses", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "CourseId", "dbo.Courses");
            DropIndex("dbo.AspNetUsers", new[] { "CourseId" });
            AlterColumn("dbo.AspNetUsers", "CourseId", c => c.Int());
            RenameColumn(table: "dbo.AspNetUsers", name: "CourseId", newName: "Course_Id");
            CreateIndex("dbo.AspNetUsers", "Course_Id");
            AddForeignKey("dbo.AspNetUsers", "Course_Id", "dbo.Courses", "Id");
        }
    }
}
