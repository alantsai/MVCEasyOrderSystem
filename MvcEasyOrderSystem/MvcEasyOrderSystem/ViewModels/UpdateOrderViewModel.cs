using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcEasyOrderSystem.Models;
using System.ComponentModel;

namespace MvcEasyOrderSystem.ViewModels
{
    public class UpdateOrderViewModel
    {
        public Order Order { get; set; }

        [DisplayName("訂單狀況")]
        public int StatusId { get; set; }
        public IEnumerable<Status> Status { get; set; }
    }
}
