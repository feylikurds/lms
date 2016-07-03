namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rebuilt_migrations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Courses", new[] { "ApplicationUser_Id" });
            CreateTable(
                "dbo.StudentActivities",
                c => new
                    {
                        StudentId = c.String(nullable: false, maxLength: 128),
                        ActivityId = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.ActivityId })
                .ForeignKey("dbo.AspNetUsers", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Activities", t => t.ActivityId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.ActivityId);
            
            AddColumn("dbo.Activities", "StudentActivity_StudentId", c => c.String(maxLength: 128));
            AddColumn("dbo.Activities", "StudentActivity_ActivityId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "AssignedRole", c => c.String());
            AddColumn("dbo.AspNetUsers", "CourseId", c => c.Int());
            AddColumn("dbo.AspNetUsers", "StudentActivity_StudentId", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "StudentActivity_ActivityId", c => c.Int());
            CreateIndex("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            CreateIndex("dbo.AspNetUsers", "CourseId");
            CreateIndex("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            AddForeignKey("dbo.AspNetUsers", "CourseId", "dbo.Courses", "Id");
            AddForeignKey("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities", new[] { "StudentId", "ActivityId" });
            AddForeignKey("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities", new[] { "StudentId", "ActivityId" });
            DropColumn("dbo.Documents", "Status");
            DropColumn("dbo.Courses", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Courses", "ApplicationUser_Id", c => c.String(maxLength: 128));
            AddColumn("dbo.Documents", "Status", c => c.Int(nullable: false));
            DropForeignKey("dbo.StudentActivities", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StudentActivities", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities");
            DropForeignKey("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities");
            DropForeignKey("dbo.AspNetUsers", "CourseId", "dbo.Courses");
            DropIndex("dbo.StudentActivities", new[] { "ActivityId" });
            DropIndex("dbo.StudentActivities", new[] { "StudentId" });
            DropIndex("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            DropIndex("dbo.AspNetUsers", new[] { "CourseId" });
            DropIndex("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            DropColumn("dbo.AspNetUsers", "StudentActivity_ActivityId");
            DropColumn("dbo.AspNetUsers", "StudentActivity_StudentId");
            DropColumn("dbo.AspNetUsers", "CourseId");
            DropColumn("dbo.AspNetUsers", "AssignedRole");
            DropColumn("dbo.Activities", "StudentActivity_ActivityId");
            DropColumn("dbo.Activities", "StudentActivity_StudentId");
            DropTable("dbo.StudentActivities");
            CreateIndex("dbo.Courses", "ApplicationUser_Id");
            AddForeignKey("dbo.Courses", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
