namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removed_Done_from_Activity : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Activities", "Done");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Activities", "Done", c => c.Boolean(nullable: false));
        }
    }
}
