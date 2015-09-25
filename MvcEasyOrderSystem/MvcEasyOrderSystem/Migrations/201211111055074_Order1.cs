namespace MvcEasyOrderSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order1 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Order", "ReadyDateTime", c => c.DateTime(nullable: false));
            //AddColumn("dbo.Order", "CollectedDateTime", c => c.DateTime());
            //AlterColumn("dbo.Order", "OrderId", c => c.Int(nullable: false));
            //DropColumn("dbo.Order", "CollectionDateTime");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Order", "CollectionDateTime", c => c.DateTime());
            //AlterColumn("dbo.Order", "OrderId", c => c.Int(nullable: false, identity: true));
            //DropColumn("dbo.Order", "CollectedDateTime");
            //DropColumn("dbo.Order", "ReadyDateTime");
        }
    }
}
