namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedSupporterDBModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SupportMessages", "Receiver_Id", "dbo.Users");
            DropForeignKey("dbo.SupportMessages", "Sender_Id", "dbo.Users");
            DropIndex("dbo.SupportMessages", new[] { "Receiver_Id" });
            DropIndex("dbo.SupportMessages", new[] { "Sender_Id" });
            RenameColumn(table: "dbo.SupportMessages", name: "Receiver_Id", newName: "ReceiverId");
            RenameColumn(table: "dbo.SupportMessages", name: "Sender_Id", newName: "SenderId");
            AlterColumn("dbo.SupportMessages", "ReceiverId", c => c.Int(nullable: false));
            AlterColumn("dbo.SupportMessages", "SenderId", c => c.Int(nullable: false));
            CreateIndex("dbo.SupportMessages", "SenderId");
            CreateIndex("dbo.SupportMessages", "ReceiverId");
            AddForeignKey("dbo.SupportMessages", "ReceiverId", "dbo.Users", "Id", cascadeDelete: false);
            AddForeignKey("dbo.SupportMessages", "SenderId", "dbo.Users", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupportMessages", "SenderId", "dbo.Users");
            DropForeignKey("dbo.SupportMessages", "ReceiverId", "dbo.Users");
            DropIndex("dbo.SupportMessages", new[] { "ReceiverId" });
            DropIndex("dbo.SupportMessages", new[] { "SenderId" });
            AlterColumn("dbo.SupportMessages", "SenderId", c => c.Int());
            AlterColumn("dbo.SupportMessages", "ReceiverId", c => c.Int());
            RenameColumn(table: "dbo.SupportMessages", name: "SenderId", newName: "Sender_Id");
            RenameColumn(table: "dbo.SupportMessages", name: "ReceiverId", newName: "Receiver_Id");
            CreateIndex("dbo.SupportMessages", "Sender_Id");
            CreateIndex("dbo.SupportMessages", "Receiver_Id");
            AddForeignKey("dbo.SupportMessages", "Sender_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.SupportMessages", "Receiver_Id", "dbo.Users", "Id");
        }
    }
}
