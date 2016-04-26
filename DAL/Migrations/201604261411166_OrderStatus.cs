namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderExes", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderExes", "Status");
        }
    }
}
