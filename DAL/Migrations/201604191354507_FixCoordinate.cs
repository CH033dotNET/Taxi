namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCoordinate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DistrictCoordinates", "District_Id", "dbo.Districts");
            DropIndex("dbo.DistrictCoordinates", new[] { "District_Id" });
            RenameColumn(table: "dbo.DistrictCoordinates", name: "District_Id", newName: "DistrictId");
            AddColumn("dbo.DistrictCoordinates", "Longitude", c => c.Double(nullable: false));
            AlterColumn("dbo.DistrictCoordinates", "DistrictId", c => c.Int(nullable: false));
            CreateIndex("dbo.DistrictCoordinates", "DistrictId");
            AddForeignKey("dbo.DistrictCoordinates", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
            DropColumn("dbo.DistrictCoordinates", "Longitide");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DistrictCoordinates", "Longitide", c => c.Double(nullable: false));
            DropForeignKey("dbo.DistrictCoordinates", "DistrictId", "dbo.Districts");
            DropIndex("dbo.DistrictCoordinates", new[] { "DistrictId" });
            AlterColumn("dbo.DistrictCoordinates", "DistrictId", c => c.Int());
            DropColumn("dbo.DistrictCoordinates", "Longitude");
            RenameColumn(table: "dbo.DistrictCoordinates", name: "DistrictId", newName: "District_Id");
            CreateIndex("dbo.DistrictCoordinates", "District_Id");
            AddForeignKey("dbo.DistrictCoordinates", "District_Id", "dbo.Districts", "Id");
        }
    }
}
