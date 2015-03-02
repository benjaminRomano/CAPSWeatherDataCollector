namespace CAPSWeatherAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KMLDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StorageUrl = c.String(),
                        UseableUrl = c.String(),
                        FileName = c.String(),
                        Type = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.KMLStreams",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Source = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        Updated = c.Boolean(nullable: false),
                        KMLData_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.KMLDatas", t => t.KMLData_ID)
                .Index(t => t.KMLData_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KMLStreams", "KMLData_ID", "dbo.KMLDatas");
            DropIndex("dbo.KMLStreams", new[] { "KMLData_ID" });
            DropTable("dbo.KMLStreams");
            DropTable("dbo.KMLDatas");
        }
    }
}
