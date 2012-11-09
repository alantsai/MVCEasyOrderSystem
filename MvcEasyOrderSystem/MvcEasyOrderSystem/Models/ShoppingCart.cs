using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public partial class ShoppingCart
    {
        public int ShoppingCartId { get; set; }
        public int MealId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string MealName { get; set; }
        public int UserId { get; set; }
    }
}