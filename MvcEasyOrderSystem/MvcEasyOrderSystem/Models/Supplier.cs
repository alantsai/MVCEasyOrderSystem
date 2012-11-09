using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MvcEasyOrderSystem.Models
{
    

    public class Supplier
    {
        public Supplier()
        {
            this.Meal = new HashSet<Meal>();
            //this.Address = new Address();
        }

        public int SupplierId { get; set; }

        [DisplayName("餐館名稱")]
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactTele { get; set; }
        public Address Address { get; set; }

        public virtual ICollection<Meal> Meal { get; set; }
    }
}
