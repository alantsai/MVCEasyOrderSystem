using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [DisplayName("名")]
        [Required(ErrorMessage = "{0} 欄位是必填的")]
        public string FirstName { get; set; }

        [DisplayName("姓")]
        [Required(ErrorMessage = "{0} 欄位是必填的")]
        public string LastName { get; set; }

        [DisplayName("電話")]
        [Required(ErrorMessage = "{0} 欄位是必填的")]
        [DataType(DataType.PhoneNumber)]
        public string HomeNo { get; set; }

        [DisplayName("手機")]
        [Required(ErrorMessage = "{0} 欄位是必填的")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        public Address Address { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} 欄位是必填的")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}