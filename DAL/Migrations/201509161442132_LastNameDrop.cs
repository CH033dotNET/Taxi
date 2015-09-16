namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LastNameDrop : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Districts", "Lastname");
            DropColumn("dbo.Users", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.Districts", "Lastname", c => c.String());
        }
    }
}
