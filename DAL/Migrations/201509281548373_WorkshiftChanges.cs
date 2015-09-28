namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WorkshiftChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.WorkshiftHistories", "WorkStarted", c => c.DateTime());
            AlterColumn("dbo.WorkshiftHistories", "WorkEnded", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.WorkshiftHistories", "WorkEnded", c => c.DateTime(nullable: false));
            AlterColumn("dbo.WorkshiftHistories", "WorkStarted", c => c.DateTime(nullable: false));
        }
    }
}
