namespace MvcEasyOrderSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Order : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.CollectionMethod",
            //    c => new
            //        {
            //            CollectionMethodId = c.Int(nullable: false, identity: true),
            //            CollectionMethodName = c.String(),
            //        })
            //    .PrimaryKey(t => t.CollectionMethodId);
            
            //CreateTable(
            //    "dbo.Order",
            //    c => new
            //        {
            //            OrderId = c.Int(nullable: false, identity: true),
            //            UserId = c.Int(nullable: false),
            //            OrderDateTime = c.DateTime(nullable: false),
            //            RequireDateTime = c.DateTime(nullable: false),
            //            TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PaymentMethodId = c.Int(nullable: false),
            //            CollectionMethodId = c.Int(nullable: false),
            //            Status = c.Int(nullable: false),
            //            Comment = c.String(),
            //            CancelDateTime = c.DateTime(),
            //            Reason = c.String(),
            //            CollectionDateTime = c.DateTime(),
            //            DeliveryCityAdd = c.String(),
            //            DeliveryDistricAdd = c.String(),
            //            DeliveryFullAdd = c.String(),
            //            DeliveryStartTime = c.DateTime(),
            //            DeliveryEndTime = c.DateTime(),
            //            IsCanceled = c.Boolean(nullable: false),
            //            Status1_StatusId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.OrderId)
            //    .ForeignKey("dbo.CollectionMethod", t => t.CollectionMethodId, cascadeDelete: true)
            //    .ForeignKey("dbo.Customer", t => t.UserId, cascadeDelete: true)
            //    .ForeignKey("dbo.PaymentMethod", t => t.PaymentMethodId, cascadeDelete: true)
            //    .ForeignKey("dbo.Status", t => t.Status1_StatusId)
            //    .Index(t => t.CollectionMethodId)
            //    .Index(t => t.UserId)
            //    .Index(t => t.PaymentMethodId)
            //    .Index(t => t.Status1_StatusId);
            
            //CreateTable(
            //    "dbo.PaymentMethod",
            //    c => new
            //        {
            //            PaymentMethodId = c.Int(nullable: false, identity: true),
            //            PaymentMethodName = c.String(),
            //        })
            //    .PrimaryKey(t => t.PaymentMethodId);
            
            //CreateTable(
            //    "dbo.Status",
            //    c => new
            //        {
            //            StatusId = c.Int(nullable: false, identity: true),
            //            StatusName = c.String(),
            //        })
            //    .PrimaryKey(t => t.StatusId);
            
            //CreateTable(
            //    "dbo.OrderDetial",
            //    c => new
            //        {
            //            OrderId = c.Int(nullable: false),
            //            MealId = c.Int(nullable: false),
            //            Quantity = c.Int(nullable: false),
            //            TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => new { t.OrderId, t.MealId })
            //    .ForeignKey("dbo.Meal", t => t.MealId, cascadeDelete: true)
            //    .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
            //    .Index(t => t.MealId)
            //    .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            //DropIndex("dbo.OrderDetial", new[] { "OrderId" });
            //DropIndex("dbo.OrderDetial", new[] { "MealId" });
            //DropIndex("dbo.Order", new[] { "Status1_StatusId" });
            //DropIndex("dbo.Order", new[] { "PaymentMethodId" });
            //DropIndex("dbo.Order", new[] { "UserId" });
            //DropIndex("dbo.Order", new[] { "CollectionMethodId" });
            //DropForeignKey("dbo.OrderDetial", "OrderId", "dbo.Order");
            //DropForeignKey("dbo.OrderDetial", "MealId", "dbo.Meal");
            //DropForeignKey("dbo.Order", "Status1_StatusId", "dbo.Status");
            //DropForeignKey("dbo.Order", "PaymentMethodId", "dbo.PaymentMethod");
            //DropForeignKey("dbo.Order", "UserId", "dbo.Customer");
            //DropForeignKey("dbo.Order", "CollectionMethodId", "dbo.CollectionMethod");
            //DropTable("dbo.OrderDetial");
            //DropTable("dbo.Status");
            //DropTable("dbo.PaymentMethod");
            //DropTable("dbo.Order");
            //DropTable("dbo.CollectionMethod");
        }
    }
}
