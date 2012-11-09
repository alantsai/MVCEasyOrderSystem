using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MvcEasyOrderSystem.Models
{
    [ComplexType]
    public class Address
    {

        public string AddCity { get; set; }
        public string AddDistrict { get; set; }
        public string AddFull { get; set; }
        public int PostCode { get; set; }

    }
}