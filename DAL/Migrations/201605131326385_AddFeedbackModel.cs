namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFeedbackModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feedbacks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        Rating = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.OrderExes", "ClientFeedbackId", c => c.Int());
            AddColumn("dbo.OrderExes", "DriverFeedbackId", c => c.Int());
            CreateIndex("dbo.OrderExes", "ClientFeedbackId");
            CreateIndex("dbo.OrderExes", "DriverFeedbackId");
            AddForeignKey("dbo.OrderExes", "ClientFeedbackId", "dbo.Feedbacks", "Id");
            AddForeignKey("dbo.OrderExes", "DriverFeedbackId", "dbo.Feedbacks", "Id");
            DropColumn("dbo.OrderExes", "Feedback");
            DropColumn("dbo.OrderExes", "Rating");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderExes", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.OrderExes", "Feedback", c => c.String());
            DropForeignKey("dbo.OrderExes", "DriverFeedbackId", "dbo.Feedbacks");
            DropForeignKey("dbo.OrderExes", "ClientFeedbackId", "dbo.Feedbacks");
            DropIndex("dbo.OrderExes", new[] { "DriverFeedbackId" });
            DropIndex("dbo.OrderExes", new[] { "ClientFeedbackId" });
            DropColumn("dbo.OrderExes", "DriverFeedbackId");
            DropColumn("dbo.OrderExes", "ClientFeedbackId");
            DropTable("dbo.Feedbacks");
        }
    }
}
