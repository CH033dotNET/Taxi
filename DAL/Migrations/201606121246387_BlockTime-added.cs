namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlockTimeadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkerStatus", "BlockTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkerStatus", "BlockTime");
        }
    }
}
