namespace DanceApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dancer2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Dancers", "DancerHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Dancers", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Dancers", "PicExtension");
            DropColumn("dbo.Dancers", "DancerHasPic");
        }
    }
}
