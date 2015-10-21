namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtensiontableCoordinates : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Coordinates", newName: "CoordinatesHistory");
            AddColumn("dbo.CoordinatesHistory", "OrderId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CoordinatesHistory", "OrderId");
            RenameTable(name: "dbo.CoordinatesHistory", newName: "Coordinates");
        }
    }
}
