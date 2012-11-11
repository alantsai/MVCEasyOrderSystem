using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public class DeliveryOrder : Order
    {
        public Address Address { get; set; }

        [DisplayName("送貨時間")]
        public System.DateTime DeliveryStartTime { get; set; }
        [DisplayName("送到時間")]
        public System.DateTime DeliveryEndTime { get; set; }
    }
}