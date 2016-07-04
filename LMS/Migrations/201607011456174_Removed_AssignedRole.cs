namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_AssignedRole : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "AssignedRole");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "AssignedRole", c => c.String());
        }
    }
}
