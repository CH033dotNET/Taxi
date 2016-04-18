namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCourdinatesToDistrict : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DistrictCoordinates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitide = c.Double(nullable: false),
                        District_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.District_Id)
                .Index(t => t.District_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DistrictCoordinates", "District_Id", "dbo.Districts");
            DropIndex("dbo.DistrictCoordinates", new[] { "District_Id" });
            DropTable("dbo.DistrictCoordinates");
        }
    }
}
