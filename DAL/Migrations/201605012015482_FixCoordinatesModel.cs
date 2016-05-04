namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixCoordinatesModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CoordinatesExes", "OrderId", "dbo.OrderExes");
            DropIndex("dbo.CoordinatesExes", new[] { "OrderId" });
            AlterColumn("dbo.CoordinatesExes", "OrderId", c => c.Int());
            CreateIndex("dbo.CoordinatesExes", "OrderId");
            AddForeignKey("dbo.CoordinatesExes", "OrderId", "dbo.OrderExes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoordinatesExes", "OrderId", "dbo.OrderExes");
            DropIndex("dbo.CoordinatesExes", new[] { "OrderId" });
            AlterColumn("dbo.CoordinatesExes", "OrderId", c => c.Int(nullable: false));
            CreateIndex("dbo.CoordinatesExes", "OrderId");
            AddForeignKey("dbo.CoordinatesExes", "OrderId", "dbo.OrderExes", "Id", cascadeDelete: true);
        }
    }
}
