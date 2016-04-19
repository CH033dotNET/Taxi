namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDistrictAddCoordinate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Coordinates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitide = c.Double(nullable: false),
                        District_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Districts", t => t.District_Id)
                .Index(t => t.District_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coordinates", "District_Id", "dbo.Districts");
            DropIndex("dbo.Coordinates", new[] { "District_Id" });
            DropTable("dbo.Coordinates");
        }
    }
}
