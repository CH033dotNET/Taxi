namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tarifes3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tarifs", "IsStandart", c => c.Boolean(nullable: false, defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tarifs", "IsStandart");
        }
    }
}
