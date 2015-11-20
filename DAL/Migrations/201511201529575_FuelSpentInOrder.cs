namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FuelSpentInOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "FuelSpent", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "FuelSpent");
        }
    }
}
