namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarName = c.String(nullable: false, maxLength: 30),
                        CarNumber = c.String(nullable: false),
                        CarOccupation = c.Int(nullable: false),
                        CarClass = c.Int(nullable: false),
                        CarPetrolType = c.String(nullable: false),
                        CarPetrolConsumption = c.String(nullable: false),
                        CarManufactureDate = c.String(nullable: false),
                        CarState = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "UserId", "dbo.Users");
            DropIndex("dbo.Cars", new[] { "UserId" });
            DropTable("dbo.Cars");
        }
    }
}
