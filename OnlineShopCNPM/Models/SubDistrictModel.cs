using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Models
{
    public class SubDistrictModel
    {
        public int ID { set; get; }
        public string Name { set; get; }
        public int DistrictID { set; get; }
        public int ProvinceID { set; get; }
    }
}