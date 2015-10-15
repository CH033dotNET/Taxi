namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class districtChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Districts", "Deleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Districts", "Deleted");
        }
    }
}
