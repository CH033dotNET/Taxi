namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WTF : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserAddresses", "Comment", c => c.String(nullable: false, maxLength: 500));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserAddresses", "Comment", c => c.String(maxLength: 500));
        }
    }
}
