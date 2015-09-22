namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class districtchange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Districts", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Districts", "Name", c => c.String());
        }
    }
}
