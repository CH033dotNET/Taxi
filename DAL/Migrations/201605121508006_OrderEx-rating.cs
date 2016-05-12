namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderExrating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderExes", "Feedback", c => c.String());
            AddColumn("dbo.OrderExes", "Rating", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderExes", "Rating");
            DropColumn("dbo.OrderExes", "Feedback");
        }
    }
}
