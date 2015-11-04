namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkersStatuses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WorkerStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WorkingStatus = c.Int(nullable: false),
                        WorkerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.WorkerId, cascadeDelete: true)
                .Index(t => t.WorkerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WorkerStatus", "WorkerId", "dbo.Users");
            DropIndex("dbo.WorkerStatus", new[] { "WorkerId" });
            DropTable("dbo.WorkerStatus");
        }
    }
}
