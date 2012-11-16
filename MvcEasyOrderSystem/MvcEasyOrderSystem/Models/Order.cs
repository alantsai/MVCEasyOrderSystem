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
            this.StatusId = 1;
        }

        [DisplayName("訂單編號")]
        public int OrderId { get; set; }
        public string UserId { get; set; }

        [DisplayName("訂單時間")]
        [DataType( System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public System.DateTime OrderDateTime { get; set; }

        [DisplayName("預定時間")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public Nullable<System.DateTime> RequireDateTime { get; set; }

        [DisplayName("完成時間")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
        public Nullable<System.DateTime> ReadyDateTime { get; set; }

        
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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool IsCanceled { get; set; }


        [DisplayName("付費方式")]
        public int PaymentMethodId { get; set; }
        [DisplayName("領取方式")]
        public int CollectionMethodId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("訂單狀況")]
        public int StatusId { get; set; }


        //public Address? Address { get; set; }

        [DisplayName("送貨時間")]
        public Nullable<System.DateTime> DeliveryStartTime { get; set; }
        [DisplayName("送到時間")]
        public Nullable<System.DateTime> DeliveryEndTime { get; set; }

        public string Address_AddCity { get; set; }
        public string Address_AddDistrict { get; set; }

        [DisplayName("地址")]
        public string Address_AddFull { get; set; }
        public Nullable<int> Address_PostCode { get; set; }


        [DisplayName("取餐時間")]
        public Nullable<System.DateTime> CollectDateTime { get; set; }

        public virtual CollectionMethod CollectionMethod { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<OrderDetial> OrderDetial { get; set; }
    }
}
