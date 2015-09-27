namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkshiftHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkshiftHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkStarted = c.DateTime(nullable: false),
                        WorkEnded = c.DateTime(nullable: false),
                        DriverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkshiftHistories", "DriverId", "dbo.Users");
            DropIndex("dbo.WorkshiftHistories", new[] { "DriverId" });
            DropTable("dbo.WorkshiftHistories");
        }
    }
}
