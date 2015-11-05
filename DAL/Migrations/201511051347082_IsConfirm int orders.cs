namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsConfirmintorders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsConfirm", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Accepted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "Accepted", c => c.Boolean());
            DropColumn("dbo.Orders", "IsConfirm");
        }
    }
}
