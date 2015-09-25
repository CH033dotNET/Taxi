namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateAddress_Kate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAddresses", "Comment", c => c.String(maxLength: 500));
            DropColumn("dbo.UserAddresses", "Country");
            DropColumn("dbo.UserAddresses", "PostalCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserAddresses", "PostalCode", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.UserAddresses", "Country", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.UserAddresses", "Comment");
        }
    }
}
