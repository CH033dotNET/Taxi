namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSupportMessage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SupportMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        SendTime = c.DateTime(nullable: false),
                        Receiver_Id = c.Int(),
                        Sender_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Receiver_Id)
                .ForeignKey("dbo.Users", t => t.Sender_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupportMessages", "Sender_Id", "dbo.Users");
            DropForeignKey("dbo.SupportMessages", "Receiver_Id", "dbo.Users");
            DropIndex("dbo.SupportMessages", new[] { "Sender_Id" });
            DropIndex("dbo.SupportMessages", new[] { "Receiver_Id" });
            DropTable("dbo.SupportMessages");
        }
    }
}
