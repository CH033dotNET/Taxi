namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderExexpanded3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderExes", "CarId", c => c.Int());
            CreateIndex("dbo.OrderExes", "CarId");
            AddForeignKey("dbo.OrderExes", "CarId", "dbo.Cars", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderExes", "CarId", "dbo.Cars");
            DropIndex("dbo.OrderExes", new[] { "CarId" });
            DropColumn("dbo.OrderExes", "CarId");
        }
    }
}
