namespace BTL_LT_UD_WEB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class duc : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.admins", "password", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.posts", "avatar", c => c.String());

            //AddForeignKey("dbo.posts", "poster_id", "dbo.poster", "poster_id");
            //AddForeignKey("dbo.posts", "category_id", "dbo.categories", "category_id");
           // DropColumn("dbo.admins", "reset_password");
           // DropColumn("dbo.comments", "status");
            //DropColumn("dbo.users", "birthaday");
            //DropColumn("dbo.poster", "birthaday");
            DropTable("dbo.sysdiagrams");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.sysdiagrams",
                c => new
                    {
                        diagram_id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 128),
                        principal_id = c.Int(nullable: false),
                        version = c.Int(),
                        definition = c.Binary(),
                    })
                .PrimaryKey(t => t.diagram_id);
            
            AddColumn("dbo.poster", "birthaday", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.users", "birthaday", c => c.DateTime(storeType: "date"));
            AddColumn("dbo.comments", "status", c => c.String(nullable: false, maxLength: 1000));
            AddColumn("dbo.admins", "reset_password", c => c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false));
            DropForeignKey("dbo.posts", "category_id", "dbo.categories");
            DropForeignKey("dbo.posts", "poster_id", "dbo.poster");
            AlterColumn("dbo.posts", "avatar", c => c.String(maxLength: 100, unicode: false));
            AlterColumn("dbo.admins", "password", c => c.String(nullable: false, maxLength: 10, fixedLength: true, unicode: false));
        }
    }
}
