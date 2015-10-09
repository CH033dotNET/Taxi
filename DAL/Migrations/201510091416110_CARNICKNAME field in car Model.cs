namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CARNICKNAMEfieldincarModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "CarNickName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cars", "CarNickName");
        }
    }
}
