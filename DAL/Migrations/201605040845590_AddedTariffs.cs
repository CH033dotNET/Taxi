namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTariffs : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TariffExes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        PriceInCity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceOutCity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePreOrder = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceRegularCar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceMinivanCar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceLuxCar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceCourierOption = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePlateOption = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceClientCarOption = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceSpeakEnglishOption = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PricePassengerSmokerOption = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TariffExes");
        }
    }
}
