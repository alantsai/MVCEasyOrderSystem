using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public partial class ShoppingCart
    {
        public int ShoppingCartId { get; set; }

        [DisplayName("餐名編號")]
        public int MealId { get; set; }

        [DisplayName("數量")]
        public int Quantity { get; set; }

        [DisplayName("單價")]
        public decimal UnitPrice { get; set; }

        [DisplayName("餐名")]
        public string MealName { get; set; }

        [DisplayName("金額")]
        public decimal FullPrice
        {
            get
            {
                return Quantity * UnitPrice;
            }
        }

        public string UserId { get; set; }
    }
}