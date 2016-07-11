namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Operacao",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 270, unicode: false),
            //            AddDate = c.DateTime(nullable: false),
            //            AlterDate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Monitoramento",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 270, unicode: false),
            //            AddDate = c.DateTime(nullable: false),
            //            AlterDate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.ResultOld",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Id_Tarefa = c.Int(nullable: false),
            //            Id_Operacao = c.Int(nullable: false),
            //            Id_Monitoramento = c.Int(nullable: false),
            //            numero1 = c.Int(nullable: false),
            //            numero2 = c.Int(nullable: false),
            //            Evaluate = c.Decimal(precision: 18, scale: 2),
            //            NotConform = c.Decimal(precision: 18, scale: 2),
            //            AddDate = c.DateTime(nullable: false),
            //            AlterDate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tarefa",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250, unicode: false),
                        AddDate = c.DateTime(nullable: false),
                        AlterDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.User",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 50, unicode: false),
            //            Password = c.String(nullable: false, maxLength: 50, unicode: false),
            //            AcessDate = c.DateTime(),
            //            AddDate = c.DateTime(nullable: false),
            //            AlterDate = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.User");
            DropTable("dbo.Tarefa");
            DropTable("dbo.ResultOld");
            DropTable("dbo.Monitoramento");
            DropTable("dbo.Operacao");
        }
    }
}
