using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public class EOSystemContex : DbContext
    {


        public DbSet<ShoppingCart> ShoppingCart { get; set; }


        public DbSet<Category> Category { get; set; }
        public DbSet<CollectionMethod> CollectionMethod { get; set; }
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Meal> Meal { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetial> OrderDetial { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Supplier> Supplier { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //本來要做TBH，但是不知道爲什麽失敗，只好以後在研究
            //modelBuilder.Entity<Order>()
            //    .Map<DeliveryOrder>(s => s.Requires("DiscriminatorCollectionId").HasValue(1))
            //    .Map<CollectionOrder>(s => s.Requires("DiscriminatorCollectionId").HasValue(2))
            //    .ToTable("Order");

        }


    }
}
