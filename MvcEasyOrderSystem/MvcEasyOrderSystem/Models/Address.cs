using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    /// <summary>
    /// ComplexType 會自動放入同一個DB Table，但同時給予OOP裏面更好的調用和定義。
    /// 不過ComplexType 不允許為Null， 導致訂單地址有關的部份需要在寫一次。
    /// </summary>
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
        [Range(0,999, ErrorMessage="{0}在{1},{2}之間")]
        public int PostCode { get; set; }


    }
}