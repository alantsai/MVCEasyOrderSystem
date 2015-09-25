using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CollectionMethod
    {
        public CollectionMethod()
        {
            this.Order = new HashSet<Order>();
        }
    
        public int CollectionMethodId { get; set; }

        [DisplayName("取餐方式")]
        public string CollectionMethodName { get; set; }
    
        public virtual ICollection<Order> Order { get; set; }
    }
}
