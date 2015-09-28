namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addimagenameforperson : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.People", "ImageName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.People", "ImageName");
        }
    }
}
