namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addOwnerForCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "OwnerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Cars", "OwnerId");
            AddForeignKey("dbo.Cars", "OwnerId", "dbo.Users", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cars", "OwnerId", "dbo.Users");
            DropIndex("dbo.Cars", new[] { "OwnerId" });
            DropColumn("dbo.Cars", "OwnerId");
        }
    }
}
