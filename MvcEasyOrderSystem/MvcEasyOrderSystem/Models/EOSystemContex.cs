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

        public DbSet<Category> Category { get; set; }
        public DbSet<Meal> Meal { get; set; }
        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Customer> Customer { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            
        }


    }
}