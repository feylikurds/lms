namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Done_to_Activity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activities", "Done", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Activities", "Done");
        }
    }
}
