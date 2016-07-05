namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Coleta4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "AlterDate", c => c.DateTime());
            AlterColumn("dbo.Operacao", "AlterDate", c => c.DateTime());
            AlterColumn("dbo.Monitoramento", "AlterDate", c => c.DateTime());
            AlterColumn("dbo.Tarefa", "AlterDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tarefa", "AlterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Monitoramento", "AlterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Operacao", "AlterDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.User", "AlterDate", c => c.DateTime(nullable: false));
        }
    }
}
