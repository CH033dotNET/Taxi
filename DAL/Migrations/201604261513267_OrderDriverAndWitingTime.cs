namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDriverAndWitingTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderExes", "WaitingTime", c => c.Int(nullable: false));
            AddColumn("dbo.OrderExes", "Driver_Id", c => c.Int());
            CreateIndex("dbo.OrderExes", "Driver_Id");
            AddForeignKey("dbo.OrderExes", "Driver_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderExes", "Driver_Id", "dbo.Users");
            DropIndex("dbo.OrderExes", new[] { "Driver_Id" });
            DropColumn("dbo.OrderExes", "Driver_Id");
            DropColumn("dbo.OrderExes", "WaitingTime");
        }
    }
}
