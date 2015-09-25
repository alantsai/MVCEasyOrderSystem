namespace MvcEasyOrderSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order_TPH1 : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Order", "CollectionMethodId", "dbo.CollectionMethod");
            //DropIndex("dbo.Order", new[] { "CollectionMethodId" });
            //RenameColumn(table: "dbo.Order", name: "CollectionMethodId", newName: "DiscriminatorCollectionId");
            //AddForeignKey("dbo.Order", "CollectionMethodId", "dbo.CollectionMethod", "CollectionMethodId", cascadeDelete: true);
            //CreateIndex("dbo.Order", "CollectionMethodId");
        }
        
        public override void Down()
        {
            //DropIndex("dbo.Order", new[] { "CollectionMethodId" });
            //DropForeignKey("dbo.Order", "CollectionMethodId", "dbo.CollectionMethod");
            //RenameColumn(table: "dbo.Order", name: "DiscriminatorCollectionId", newName: "CollectionMethodId");
            //CreateIndex("dbo.Order", "CollectionMethodId");
            //AddForeignKey("dbo.Order", "CollectionMethodId", "dbo.CollectionMethod", "CollectionMethodId", cascadeDelete: true);
        }
    }
}
