namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedNewsStatus : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.News", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.News", "Status");
        }
    }
}
