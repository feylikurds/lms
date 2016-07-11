namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Comments : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "Comments", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "Comments");
        }
    }
}
