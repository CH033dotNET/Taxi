namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrderstable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PeekPlace", c => c.String(nullable: false, maxLength: 120));
            AddColumn("dbo.Orders", "DropPlace", c => c.String(nullable: false, maxLength: 120));
            AddColumn("dbo.Orders", "LatitudeDropPlace", c => c.Double(nullable: false));
            AddColumn("dbo.Orders", "LongitudeDropPlace", c => c.Double(nullable: false));
            AddColumn("dbo.Orders", "LatitudePeekPlace", c => c.Double(nullable: false));
            AddColumn("dbo.Orders", "LongitudePeekPlace", c => c.Double(nullable: false));
            AddColumn("dbo.Orders", "TotalPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Orders", "Accuracy", c => c.Double(nullable: false));
            AlterColumn("dbo.Orders", "StartWork", c => c.DateTime());
            AlterColumn("dbo.Orders", "EndWork", c => c.DateTime());
            DropColumn("dbo.Orders", "ComeOut");
            DropColumn("dbo.Orders", "ComeIn");
            DropColumn("dbo.Orders", "LatitudeComeOut");
            DropColumn("dbo.Orders", "LongitudeComeOut");
            DropColumn("dbo.Orders", "LatitudeComeIn");
            DropColumn("dbo.Orders", "LongitudeComeIn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "LongitudeComeIn", c => c.Single(nullable: false));
            AddColumn("dbo.Orders", "LatitudeComeIn", c => c.Single(nullable: false));
            AddColumn("dbo.Orders", "LongitudeComeOut", c => c.Single(nullable: false));
            AddColumn("dbo.Orders", "LatitudeComeOut", c => c.Single(nullable: false));
            AddColumn("dbo.Orders", "ComeIn", c => c.String(nullable: false, maxLength: 120));
            AddColumn("dbo.Orders", "ComeOut", c => c.String(nullable: false, maxLength: 120));
            AlterColumn("dbo.Orders", "EndWork", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "StartWork", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "Accuracy", c => c.Single(nullable: false));
            DropColumn("dbo.Orders", "TotalPrice");
            DropColumn("dbo.Orders", "LongitudePeekPlace");
            DropColumn("dbo.Orders", "LatitudePeekPlace");
            DropColumn("dbo.Orders", "LongitudeDropPlace");
            DropColumn("dbo.Orders", "LatitudeDropPlace");
            DropColumn("dbo.Orders", "DropPlace");
            DropColumn("dbo.Orders", "PeekPlace");
        }
    }
}
