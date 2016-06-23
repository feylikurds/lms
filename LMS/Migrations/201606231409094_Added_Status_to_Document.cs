namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Status_to_Document : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Status");
        }
    }
}
