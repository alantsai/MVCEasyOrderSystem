using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    
    public partial class Order
    {
        public Order()
        {
            this.OrderDetial = new HashSet<OrderDetial>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }
        public int UserId { get; set; }

        [DisplayName("訂單時間")]
        public System.DateTime OrderDateTime { get; set; }

        [DisplayName("預定時間")]
        public System.DateTime RequireDateTime { get; set; }

        [DisplayName("完成時間")]
        public System.DateTime ReadyDateTime { get; set; }

        
        [DisplayName("金額")]
        [DataType(DataType.Currency)]
        public decimal TotalPrice { get; set; }


        [DisplayName("備註")]
        public string Comment { get; set; }

        [DisplayName("取消時間")]
        public Nullable<System.DateTime> CancelDateTime { get; set; }
        [DisplayName("取消原因")]
        public string Reason { get; set; }



        [DisplayName("是否取消")]
        public bool IsCanceled { get; set; }


        public int PaymentMethodId { get; set; }
        public int CollectionMethodId { get; set; }
        public int Status { get; set; }

        public virtual CollectionMethod CollectionMethod { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Status Status1 { get; set; }
        public virtual ICollection<OrderDetial> OrderDetial { get; set; }
    }
}
