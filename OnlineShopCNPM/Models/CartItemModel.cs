using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShopCNPM.Models
{
    public class CartItemModel
    {
        public int ProductCode { set; get; }
        public string MetaTitle { set; get; }
        public string Name { set; get; }
        public string Image { set; get; }
        public decimal Price { set; get; }
        public int Amount { set; get; }

    }
}