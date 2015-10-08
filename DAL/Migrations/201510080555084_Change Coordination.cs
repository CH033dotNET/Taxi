namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCoordination : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Coordinates", "TarifId", c => c.Int(nullable: false));
            AlterColumn("dbo.Coordinates", "Accuracy", c => c.Double(nullable: false));
            CreateIndex("dbo.Coordinates", "TarifId");
            AddForeignKey("dbo.Coordinates", "TarifId", "dbo.Tarifs", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Coordinates", "TarifId", "dbo.Tarifs");
            DropIndex("dbo.Coordinates", new[] { "TarifId" });
            AlterColumn("dbo.Coordinates", "Accuracy", c => c.Int(nullable: false));
            DropColumn("dbo.Coordinates", "TarifId");
        }
    }
}
