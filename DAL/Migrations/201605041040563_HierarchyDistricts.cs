namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HierarchyDistricts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Districts", "ParentId", c => c.Int());
            AddColumn("dbo.Districts", "IsFolder", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Districts", "ParentId");
            AddForeignKey("dbo.Districts", "ParentId", "dbo.Districts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Districts", "ParentId", "dbo.Districts");
            DropIndex("dbo.Districts", new[] { "ParentId" });
            DropColumn("dbo.Districts", "IsFolder");
            DropColumn("dbo.Districts", "ParentId");
        }
    }
}
