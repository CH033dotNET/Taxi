namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Accepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Accepted");
        }
    }
}
