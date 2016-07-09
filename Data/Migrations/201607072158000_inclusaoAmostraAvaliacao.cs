namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inclusaoAmostraAvaliacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ResultOld", "numero1", c => c.Int(nullable: false));
            AddColumn("dbo.ResultOld", "numero2", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ResultOld", "numero2");
            DropColumn("dbo.ResultOld", "numero1");
        }
    }
}
