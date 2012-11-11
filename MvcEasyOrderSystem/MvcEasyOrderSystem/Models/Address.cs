using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    [ComplexType]
    public class Address
    {

        [Required(ErrorMessage = "{0} 欄位為必填。例子:台中市")]
        [DisplayName("市/縣")]
        public string AddCity { get; set; }

        [Required(ErrorMessage = "{0} 欄位為必填。例子:西屯區")]
        [DisplayName("區")]
        public string AddDistrict { get; set; }

        [Required(ErrorMessage = "{0} 欄位為必填。例子:台中市西屯區僑光路20號")]
        [DisplayName("完整地址")]
        public string AddFull { get; set; }


        [DisplayName("郵遞區號")]
        public int PostCode { get; set; }


    }
}