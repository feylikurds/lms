namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fixed_merge : DbMigration
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
