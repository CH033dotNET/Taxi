namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderExoptimized : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.OrderExes", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderExes", "Address", c => c.String());
        }
    }
}
