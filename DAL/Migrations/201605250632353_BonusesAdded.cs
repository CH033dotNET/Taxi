namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BonusesAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Bonus", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Bonus");
        }
    }
}
