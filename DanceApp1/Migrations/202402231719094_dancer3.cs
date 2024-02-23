namespace DanceApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dancer3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Dancers", "DancerHasPic");
            DropColumn("dbo.Dancers", "PicExtension");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Dancers", "PicExtension", c => c.String());
            AddColumn("dbo.Dancers", "DancerHasPic", c => c.Boolean(nullable: false));
        }
    }
}
