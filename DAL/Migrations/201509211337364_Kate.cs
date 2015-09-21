namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Kate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserAddresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        Country = c.String(nullable: false, maxLength: 50),
                        City = c.String(nullable: false, maxLength: 50),
                        Street = c.String(nullable: false, maxLength: 100),
                        Number = c.String(nullable: false, maxLength: 10),
                        PostalCode = c.String(nullable: false, maxLength: 20),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAddresses", "UserId", "dbo.Users");
            DropIndex("dbo.UserAddresses", new[] { "UserId" });
            DropTable("dbo.UserAddresses");
        }
    }
}
