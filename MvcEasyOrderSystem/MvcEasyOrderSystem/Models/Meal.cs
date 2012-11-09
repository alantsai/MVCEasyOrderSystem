using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public class Meal
    {
    
        public int MealId { get; set; }

        [DisplayName("餐名")]
        public string MealName { get; set; }

        [DisplayName("價錢")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DisplayName("餐的類別")]
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }

        [DisplayName("圖片")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [DisplayName("餐的類別")]
        public virtual Category Category { get; set; }
        public virtual Supplier Supplier { get; set; }

    }
}