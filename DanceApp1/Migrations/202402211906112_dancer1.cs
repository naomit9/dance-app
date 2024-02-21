namespace DanceApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dancer1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dancers", "Showcase_showcaseId", c => c.Int());
            CreateIndex("dbo.Dancers", "Showcase_showcaseId");
            AddForeignKey("dbo.Dancers", "Showcase_showcaseId", "dbo.Showcases", "showcaseId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dancers", "Showcase_showcaseId", "dbo.Showcases");
            DropIndex("dbo.Dancers", new[] { "Showcase_showcaseId" });
            DropColumn("dbo.Dancers", "Showcase_showcaseId");
        }
    }
}
