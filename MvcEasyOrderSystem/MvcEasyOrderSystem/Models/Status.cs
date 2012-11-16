using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    
    public partial class Status
    {
        public Status()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int StatusId { get; set; }
        [DisplayName("­q³æª¬ªp")]
        public string StatusName { get; set; }
    
        public virtual ICollection<Order> Order { get; set; }
    }
}
