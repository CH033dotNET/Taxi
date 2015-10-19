namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ordersdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ComeOut = c.String(nullable: false, maxLength: 120),
                        ComeIn = c.String(nullable: false, maxLength: 120),
                        OrderTime = c.DateTime(nullable: false),
                        RunTime = c.DateTime(nullable: false),
                        LatitudeComeOut = c.Single(nullable: false),
                        LongitudeComeOut = c.Single(nullable: false),
                        LongitudeAccuracy = c.Single(nullable: false),
                        LatitudeComeIn = c.Single(nullable: false),
                        LongitudeComeIn = c.Single(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserId", "dbo.Users");
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropTable("dbo.Orders");
        }
    }
}
