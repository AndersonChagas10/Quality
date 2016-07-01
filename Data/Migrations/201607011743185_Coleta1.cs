namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Coleta1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tarefa", "Evaluate");
            DropColumn("dbo.Tarefa", "NotConform");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tarefa", "NotConform", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Tarefa", "Evaluate", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
