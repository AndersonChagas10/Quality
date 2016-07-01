namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Coleta : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AuditCenter", newName: "Operacao");
            DropForeignKey("dbo.Result", "Id_AuditCenter", "dbo.AuditCenter");
            DropIndex("dbo.Result", new[] { "Id_AuditCenter" });
            CreateTable(
                "dbo.ResultOld",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Id_Tarefa = c.Int(nullable: false),
                        Id_Operacao = c.Int(nullable: false),
                        Id_Monitoramento = c.Int(nullable: false),
                        Evaluate = c.Decimal(precision: 18, scale: 2),
                        NotConform = c.Decimal(precision: 18, scale: 2),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Monitoramento",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 70, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tarefa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 70, unicode: false),
                        Evaluate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NotConform = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Operacao", "AlterDate", c => c.DateTime(nullable: false));
            DropTable("dbo.Result");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.Operacao", "AlterDate", c => c.DateTime());
            DropTable("dbo.Tarefa");
            DropTable("dbo.Monitoramento");
            DropTable("dbo.ResultOld");
            CreateIndex("dbo.Result", "Id_AuditCenter");
            AddForeignKey("dbo.Result", "Id_AuditCenter", "dbo.AuditCenter", "Id");
            RenameTable(name: "dbo.Operacao", newName: "AuditCenter");
        }
    }
}
