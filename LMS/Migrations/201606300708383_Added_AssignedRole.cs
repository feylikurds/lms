namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_AssignedRole : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AssignedRole", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AssignedRole");
        }
    }
}
