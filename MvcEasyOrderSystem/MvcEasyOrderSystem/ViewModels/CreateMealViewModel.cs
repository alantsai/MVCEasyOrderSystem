using MvcEasyOrderSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.ViewModels
{
    public class CreateMealViewModel
    {
        [DisplayName("餐名")]
        [Required(ErrorMessage = "{0}是必須要填寫的")]
        public string MealName { get; set; }

        public int MealId { get; set; }

        [DisplayName("價錢(元)")]
        [DataType(DataType.Currency)]
        [Range(1, 2000,ErrorMessage = "{0}最低為{1},最高為{2}")]
        public decimal Price { get; set; }

        [DisplayName("餐的類別")]
        public int CategoryId { get; set; }

        [DisplayName("圖片")]
        [DataType(DataType.ImageUrl)]
        public string Image { get; set; }

        [DisplayName("餐的類別")]
        public virtual IEnumerable<Category> Categories { get; set; }
    }
}