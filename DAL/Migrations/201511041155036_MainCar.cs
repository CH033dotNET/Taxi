namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MainCar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cars", "isMain", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Cars", "CarName", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Cars", "CarNumber", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Cars", "CarNickName", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Cars", "CarNickName", c => c.String(nullable: false, maxLength: 16));
            AlterColumn("dbo.Cars", "CarNumber", c => c.String(nullable: false, maxLength: 12));
            AlterColumn("dbo.Cars", "CarName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Cars", "isMain");
        }
    }
}
