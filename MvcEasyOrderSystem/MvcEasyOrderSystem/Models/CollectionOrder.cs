using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public class CollectionOrder : Order
    {
        [DisplayName("取餐時間")]
        public Nullable<System.DateTime> CollectedDateTime { get; set; }
    }
}