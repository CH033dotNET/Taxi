namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateLocation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Localizations", "DistrictId", "dbo.Districts");
            DropForeignKey("dbo.Localizations", "UserId", "dbo.Users");
            DropIndex("dbo.Localizations", new[] { "UserId" });
            DropIndex("dbo.Localizations", new[] { "DistrictId" });
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.DistrictId);
            
            DropTable("dbo.Localizations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Localizations",
                c => new
                    {
                        LocalizationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocalizationId);
            
            DropForeignKey("dbo.Locations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Locations", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Locations", new[] { "DistrictId" });
            DropIndex("dbo.Locations", new[] { "UserId" });
            DropTable("dbo.Locations");
            CreateIndex("dbo.Localizations", "DistrictId");
            CreateIndex("dbo.Localizations", "UserId");
            AddForeignKey("dbo.Localizations", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Localizations", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
        }
    }
}
