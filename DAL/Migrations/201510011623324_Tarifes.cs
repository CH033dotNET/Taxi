namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tarifes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tarifs",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        IsIntercity = c.Boolean(nullable: false),
                        DistrictId = c.Int(nullable: false),
                        MinimalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OneMinuteCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WaitingCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Districts", t => t.DistrictId, cascadeDelete: true)
                .Index(t => t.DistrictId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tarifs", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Tarifs", new[] { "DistrictId" });
            DropTable("dbo.Tarifs");
        }
    }
}
