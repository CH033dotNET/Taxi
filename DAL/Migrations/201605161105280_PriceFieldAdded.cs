namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PriceFieldAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderExes", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderExes", "Price");
        }
    }
}
