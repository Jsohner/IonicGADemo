namespace ScoreboardServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialDBCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        PlayerId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.PlayerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Players");
        }
    }
}
