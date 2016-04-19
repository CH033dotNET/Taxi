namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexToCoordinate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DistrictCoordinates", "Index", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DistrictCoordinates", "Index");
        }
    }
}
