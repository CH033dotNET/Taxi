namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoordinatesToDistrict : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DistrictCoordinates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.DistrictId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistrictCoordinates", "DistrictId", "dbo.Districts");
            DropIndex("dbo.DistrictCoordinates", new[] { "DistrictId" });
            DropTable("dbo.DistrictCoordinates");
        }
    }
}
