namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange1 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.UserSgq", newName: "User");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.User", newName: "UserSgq");
        }
    }
}
