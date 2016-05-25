namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EngleshfieldaddedtoOrderExmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdditionallyRequirements", "English", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdditionallyRequirements", "English");
        }
    }
}
