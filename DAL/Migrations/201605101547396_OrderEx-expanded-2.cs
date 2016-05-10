namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderExexpanded2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AdditionallyRequirements", "Time", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AdditionallyRequirements", "Time", c => c.DateTime(nullable: false));
        }
    }
}
