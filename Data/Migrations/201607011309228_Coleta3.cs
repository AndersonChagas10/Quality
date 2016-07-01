namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Coleta3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditCenter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 70, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                        result_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Result", t => t.result_Id)
                .Index(t => t.result_Id);
            
            CreateTable(
                "dbo.Result",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_AuditCenter = c.Int(nullable: false),
                        Evaluate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NotConform = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuditCenter", t => t.Id_AuditCenter)
                .Index(t => t.Id_AuditCenter);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                        AcessDate = c.DateTime(),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditCenter", "result_Id", "dbo.Result");
            DropForeignKey("dbo.Result", "Id_AuditCenter", "dbo.AuditCenter");
            DropIndex("dbo.Result", new[] { "Id_AuditCenter" });
            DropIndex("dbo.AuditCenter", new[] { "result_Id" });
            DropTable("dbo.User");
            DropTable("dbo.Result");
            DropTable("dbo.AuditCenter");
        }
    }
}
