namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student_to_Activity : DbMigration
    {
        public override void Up()
        {
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
            AddColumn("dbo.AspNetUsers", "StudentActivity_StudentId", c => c.String(maxLength: 128));
            AddColumn("dbo.AspNetUsers", "StudentActivity_ActivityId", c => c.Int());
            CreateIndex("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            CreateIndex("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            AddForeignKey("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities", new[] { "StudentId", "ActivityId" });
            AddForeignKey("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities", new[] { "StudentId", "ActivityId" });
            DropColumn("dbo.Documents", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "Status", c => c.Int(nullable: false));
            DropForeignKey("dbo.StudentActivities", "ActivityId", "dbo.Activities");
            DropForeignKey("dbo.StudentActivities", "StudentId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities");
            DropForeignKey("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" }, "dbo.StudentActivities");
            DropIndex("dbo.StudentActivities", new[] { "ActivityId" });
            DropIndex("dbo.StudentActivities", new[] { "StudentId" });
            DropIndex("dbo.AspNetUsers", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            DropIndex("dbo.Activities", new[] { "StudentActivity_StudentId", "StudentActivity_ActivityId" });
            DropColumn("dbo.AspNetUsers", "StudentActivity_ActivityId");
            DropColumn("dbo.AspNetUsers", "StudentActivity_StudentId");
            DropColumn("dbo.Activities", "StudentActivity_ActivityId");
            DropColumn("dbo.Activities", "StudentActivity_StudentId");
            DropTable("dbo.StudentActivities");
        }
    }
}
