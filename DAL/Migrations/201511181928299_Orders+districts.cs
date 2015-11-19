namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ordersdistricts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DistrictId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "DistrictId");
            AddForeignKey("dbo.Orders", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Orders", new[] { "DistrictId" });
            DropColumn("dbo.Orders", "DistrictId");
        }
    }
}
