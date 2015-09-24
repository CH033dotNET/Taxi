namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetVIP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VIPClients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SetDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VIPClients", "UserId", "dbo.Users");
            DropIndex("dbo.VIPClients", new[] { "UserId" });
            DropTable("dbo.VIPClients");
        }
    }
}
