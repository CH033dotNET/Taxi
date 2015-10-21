namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatetableOrders : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropIndex("dbo.Orders", new[] { "UserId" });
            AddColumn("dbo.Orders", "Accuracy", c => c.Single(nullable: false));
            AddColumn("dbo.Orders", "StartWork", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "EndWork", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "DriverId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "WaitingTime", c => c.String());
            AddColumn("dbo.Orders", "PersonId", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "RunTime", c => c.String(maxLength: 120));
            CreateIndex("dbo.Orders", "PersonId");
            AddForeignKey("dbo.Orders", "PersonId", "dbo.People", "Id", cascadeDelete: true);
            DropColumn("dbo.Orders", "LongitudeAccuracy");
            DropColumn("dbo.Orders", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "LongitudeAccuracy", c => c.Single(nullable: false));
            DropForeignKey("dbo.Orders", "PersonId", "dbo.People");
            DropIndex("dbo.Orders", new[] { "PersonId" });
            AlterColumn("dbo.Orders", "RunTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Orders", "PersonId");
            DropColumn("dbo.Orders", "WaitingTime");
            DropColumn("dbo.Orders", "DriverId");
            DropColumn("dbo.Orders", "EndWork");
            DropColumn("dbo.Orders", "StartWork");
            DropColumn("dbo.Orders", "Accuracy");
            CreateIndex("dbo.Orders", "UserId");
            AddForeignKey("dbo.Orders", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
