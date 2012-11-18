using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcEasyOrderSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MvcEasyOrderSystem.ViewModels
{
    public class CreateOrderViewModel
    {

        [DisplayName("預定時間")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public System.DateTime RequireDateTime { get; set; }

        [DisplayName("備註")]
        public string Comment { get; set; }

        [DisplayName("付費方式")]
        public int PaymentMethodId { get; set; }
        [DisplayName("領取方式")]
        public int CollectionMethodId { get; set; }

        
        [DisplayName("市/縣")]
        public string AddCity { get; set; }

        [DisplayName("區")]
        public string AddDistrict { get; set; }

        [DisplayName("完整地址")]
        public string AddFull { get; set; }

        [DisplayName("使用註冊時候輸入的地址")]
        public bool IsUserRegAddress { get; set; }

        
        [DisplayName("郵遞區號")]
        [Range(0, 999, ErrorMessage="0-99")]
        public Nullable<int> PostCode { get; set; }


        public virtual IEnumerable<CollectionMethod> CollectionMethods { get; set; }
        public virtual IEnumerable<PaymentMethod> PaymentMethods { get; set; }
    }
}