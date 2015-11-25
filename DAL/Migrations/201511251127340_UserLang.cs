namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserLang : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Lang", c => c.String(nullable:false, defaultValue:"en-us"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Lang");
        }
    }
}
