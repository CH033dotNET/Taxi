namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCoordinatesExModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CoordinatesExes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                        Accuracy = c.Double(nullable: false),
                        AddedTime = c.DateTime(nullable: false),
                        OrderId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DriverId, cascadeDelete: true)
                .ForeignKey("dbo.OrderExes", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CoordinatesExes", "OrderId", "dbo.OrderExes");
            DropForeignKey("dbo.CoordinatesExes", "DriverId", "dbo.Users");
            DropIndex("dbo.CoordinatesExes", new[] { "DriverId" });
            DropIndex("dbo.CoordinatesExes", new[] { "OrderId" });
            DropTable("dbo.CoordinatesExes");
        }
    }
}
