namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tarifes2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Tarifs", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Tarifs", new[] { "DistrictId" });
            AlterColumn("dbo.Tarifs", "DistrictId", c => c.Int());
            CreateIndex("dbo.Tarifs", "DistrictId");
            AddForeignKey("dbo.Tarifs", "DistrictId", "dbo.Districts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tarifs", "DistrictId", "dbo.Districts");
            DropIndex("dbo.Tarifs", new[] { "DistrictId" });
            AlterColumn("dbo.Tarifs", "DistrictId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tarifs", "DistrictId");
            AddForeignKey("dbo.Tarifs", "DistrictId", "dbo.Districts", "Id", cascadeDelete: true);
        }
    }
}
