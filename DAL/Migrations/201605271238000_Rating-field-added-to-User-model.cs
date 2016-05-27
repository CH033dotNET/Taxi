namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RatingfieldaddedtoUsermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Rating", c => c.Double());
            AddColumn("dbo.Feedbacks", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Feedbacks", "UserId");
            DropColumn("dbo.Users", "Rating");
        }
    }
}
