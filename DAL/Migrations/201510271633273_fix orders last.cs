namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixorderslast : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Accuracy", c => c.Double(nullable: false));
            DropColumn("dbo.Orders", "Acuracy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Acuracy", c => c.Double(nullable: false));
            DropColumn("dbo.Orders", "Accuracy");
        }
    }
}
