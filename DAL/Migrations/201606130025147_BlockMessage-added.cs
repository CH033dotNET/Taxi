namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BlockMessageadded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WorkerStatus", "BlockMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WorkerStatus", "BlockMessage");
        }
    }
}
