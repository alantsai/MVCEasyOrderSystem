using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [DisplayName("使用者帳戶")]
        public string UserName { get; set; }

        [DisplayName("註冊所用郵箱")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}