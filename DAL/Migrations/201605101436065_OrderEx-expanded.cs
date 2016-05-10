namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderExexpanded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionallyRequirements",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Urgently = c.Boolean(nullable: false),
                        Time = c.DateTime(nullable: false),
                        Passengers = c.Int(nullable: false),
                        Car = c.Int(nullable: false),
                        Courier = c.Boolean(nullable: false),
                        WithPlate = c.Boolean(nullable: false),
                        MyCar = c.Boolean(nullable: false),
                        Pets = c.Boolean(nullable: false),
                        Bag = c.Boolean(nullable: false),
                        Conditioner = c.Boolean(nullable: false),
                        NoSmoking = c.Boolean(nullable: false),
                        Smoking = c.Boolean(nullable: false),
                        Check = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderExes", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AddressToes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Address = c.String(),
                        Building = c.String(),
                        OrderExId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderExes", t => t.OrderExId, cascadeDelete: true)
                .Index(t => t.OrderExId);
            
            CreateTable(
                "dbo.AddressFroms",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Address = c.String(nullable: false),
                        Building = c.String(),
                        Entrance = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderExes", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.OrderExes", "Route", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderExes", "UserId", c => c.Int());
            AddColumn("dbo.OrderExes", "Name", c => c.String());
            AddColumn("dbo.OrderExes", "Phone", c => c.String());
            AddColumn("dbo.OrderExes", "Perquisite", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AddressFroms", "Id", "dbo.OrderExes");
            DropForeignKey("dbo.AddressToes", "OrderExId", "dbo.OrderExes");
            DropForeignKey("dbo.AdditionallyRequirements", "Id", "dbo.OrderExes");
            DropIndex("dbo.AddressFroms", new[] { "Id" });
            DropIndex("dbo.AddressToes", new[] { "OrderExId" });
            DropIndex("dbo.AdditionallyRequirements", new[] { "Id" });
            DropColumn("dbo.OrderExes", "Perquisite");
            DropColumn("dbo.OrderExes", "Phone");
            DropColumn("dbo.OrderExes", "Name");
            DropColumn("dbo.OrderExes", "UserId");
            DropColumn("dbo.OrderExes", "Route");
            DropTable("dbo.AddressFroms");
            DropTable("dbo.AddressToes");
            DropTable("dbo.AdditionallyRequirements");
        }
    }
}
