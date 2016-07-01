namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Coleta31 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuditCenter", "result_Id", "dbo.Result");
            DropIndex("dbo.AuditCenter", new[] { "result_Id" });
            DropColumn("dbo.AuditCenter", "result_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuditCenter", "result_Id", c => c.Int());
            CreateIndex("dbo.AuditCenter", "result_Id");
            AddForeignKey("dbo.AuditCenter", "result_Id", "dbo.Result", "Id");
        }
    }
}
