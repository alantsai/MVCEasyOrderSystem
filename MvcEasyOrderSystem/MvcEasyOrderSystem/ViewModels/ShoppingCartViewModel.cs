using MvcEasyOrderSystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCart> CartItems { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }
    }
}