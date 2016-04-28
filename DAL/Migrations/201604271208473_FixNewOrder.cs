namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixNewOrder : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderExes", "DriverId", "dbo.Users");
            DropIndex("dbo.OrderExes", new[] { "DriverId" });
            AlterColumn("dbo.OrderExes", "DriverId", c => c.Int());
            CreateIndex("dbo.OrderExes", "DriverId");
            AddForeignKey("dbo.OrderExes", "DriverId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderExes", "DriverId", "dbo.Users");
            DropIndex("dbo.OrderExes", new[] { "DriverId" });
            AlterColumn("dbo.OrderExes", "DriverId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderExes", "DriverId");
            AddForeignKey("dbo.OrderExes", "DriverId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
