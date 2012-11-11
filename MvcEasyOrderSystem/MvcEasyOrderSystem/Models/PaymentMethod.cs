using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int PaymentMethodId { get; set; }
        public string PaymentMethodName { get; set; }
    
        public virtual ICollection<Order> Order { get; set; }
    }
}
