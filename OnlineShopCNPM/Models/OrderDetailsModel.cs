using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopCNPM.Models
{
    public class OrderDetailsModel
    {
        public int ProductCode { set; get; }
        public string ProductName { set; get; }
        public string MetaTitle { set; get; }
        public int Amount { set; get; }
        public decimal Price { set; get; }
    }
}