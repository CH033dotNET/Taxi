namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDriverIdForOrderEx : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderExes", "Driver_Id", "dbo.Users");
            DropIndex("dbo.OrderExes", new[] { "Driver_Id" });
            RenameColumn(table: "dbo.OrderExes", name: "Driver_Id", newName: "DriverId");
            AlterColumn("dbo.OrderExes", "DriverId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderExes", "DriverId");
            AddForeignKey("dbo.OrderExes", "DriverId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderExes", "DriverId", "dbo.Users");
            DropIndex("dbo.OrderExes", new[] { "DriverId" });
            AlterColumn("dbo.OrderExes", "DriverId", c => c.Int());
            RenameColumn(table: "dbo.OrderExes", name: "DriverId", newName: "Driver_Id");
            CreateIndex("dbo.OrderExes", "Driver_Id");
            AddForeignKey("dbo.OrderExes", "Driver_Id", "dbo.Users", "Id");
        }
    }
}
