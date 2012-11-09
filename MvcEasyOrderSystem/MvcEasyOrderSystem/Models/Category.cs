using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public class Category
    {
        public Category()
        {
            this.Meal = new HashSet<Meal>();
        }
    
        public int CategoryId { get; set; }

        [DisplayName("餐的類別")]
        public string CategoryName { get; set; }

        public virtual ICollection<Meal> Meal { get; set; }
    }
}
