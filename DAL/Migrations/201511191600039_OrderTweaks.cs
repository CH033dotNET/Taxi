namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderTweaks : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Orders", new[] { "DistrictId" });
            AlterColumn("dbo.Orders", "DistrictId", c => c.Int());
            CreateIndex("dbo.Orders", "DistrictId");
            AddForeignKey("dbo.Orders", "DistrictId", "dbo.Districts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Orders", new[] { "DistrictId" });
            AlterColumn("dbo.Orders", "DistrictId", c => c.Int(nullable: false));
            CreateIndex("dbo.Orders", "DistrictId");
            AddForeignKey("dbo.Orders", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
        }
    }
}
