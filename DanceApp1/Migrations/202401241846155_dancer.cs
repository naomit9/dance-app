namespace DanceApp1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dancer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dancers",
                c => new
                    {
                        dancerId = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        danceStyle = c.String(),
                        dancerBio = c.String(),
                        groupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.dancerId)
                .ForeignKey("dbo.Groups", t => t.groupId, cascadeDelete: true)
                .Index(t => t.groupId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        groupId = c.Int(nullable: false, identity: true),
                        groupName = c.String(),
                        groupStyle = c.String(),
                        groupBio = c.String(),
                    })
                .PrimaryKey(t => t.groupId);
            
            CreateTable(
                "dbo.Showcases",
                c => new
                    {
                        showcaseId = c.Int(nullable: false, identity: true),
                        showcaseName = c.String(),
                        Date = c.DateTime(nullable: false),
                        showcaseLocation = c.String(),
                    })
                .PrimaryKey(t => t.showcaseId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ShowcaseGroups",
                c => new
                    {
                        Showcase_showcaseId = c.Int(nullable: false),
                        Group_groupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Showcase_showcaseId, t.Group_groupId })
                .ForeignKey("dbo.Showcases", t => t.Showcase_showcaseId, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_groupId, cascadeDelete: true)
                .Index(t => t.Showcase_showcaseId)
                .Index(t => t.Group_groupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Dancers", "groupId", "dbo.Groups");
            DropForeignKey("dbo.ShowcaseGroups", "Group_groupId", "dbo.Groups");
            DropForeignKey("dbo.ShowcaseGroups", "Showcase_showcaseId", "dbo.Showcases");
            DropIndex("dbo.ShowcaseGroups", new[] { "Group_groupId" });
            DropIndex("dbo.ShowcaseGroups", new[] { "Showcase_showcaseId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Dancers", new[] { "groupId" });
            DropTable("dbo.ShowcaseGroups");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Showcases");
            DropTable("dbo.Groups");
            DropTable("dbo.Dancers");
        }
    }
}
