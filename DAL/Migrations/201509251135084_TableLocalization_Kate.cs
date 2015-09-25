namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TableLocalization_Kate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Localizations",
                c => new
                    {
                        LocalizationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        DistrictId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocalizationId)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.DistrictId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Localizations", "UserId", "dbo.Users");
            DropForeignKey("dbo.Localizations", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Localizations", new[] { "DistrictId" });
            DropIndex("dbo.Localizations", new[] { "UserId" });
            DropTable("dbo.Localizations");
        }
    }
}
