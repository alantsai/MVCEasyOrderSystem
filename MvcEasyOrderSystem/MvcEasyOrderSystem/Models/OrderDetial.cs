using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{

    public partial class OrderDetial
    {
        [Key, Column(Order = 1)]
        public int OrderId { get; set; }
        [Key, Column(Order = 2)]
        public int MealId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual Meal Meal { get; set; }
        public virtual Order Order { get; set; }
    }
}
