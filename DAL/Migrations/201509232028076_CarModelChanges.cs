namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CarModelChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Cars", "CarName", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Cars", "CarNumber", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.Cars", "CarPetrolType", c => c.Int(nullable: false));
            AlterColumn("dbo.Cars", "CarPetrolConsumption", c => c.Int(nullable: false));
            AlterColumn("dbo.Cars", "CarManufactureDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cars", "CarManufactureDate", c => c.String(nullable: false));
            AlterColumn("dbo.Cars", "CarPetrolConsumption", c => c.String(nullable: false));
            AlterColumn("dbo.Cars", "CarPetrolType", c => c.String(nullable: false));
            AlterColumn("dbo.Cars", "CarNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Cars", "CarName", c => c.String(nullable: false, maxLength: 30));
        }
    }
}
