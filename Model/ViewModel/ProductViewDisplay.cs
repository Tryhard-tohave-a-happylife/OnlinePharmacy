using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductViewDisplay
    {
        public int ID { set; get; }
        public string Image { set; get; }
        public string Name { set; get; }
        public decimal? Price { set; get; }
        public string MetaTitle { set; get; }
        public string CategoryTitle { set; get; }
        public double PercentSale { set; get; }
        public string ParentTitleCate { set; get; }
        public string ChildTitleCate { set; get; }
        public int BuyCount { set; get; }
        public double ReviewPoint { set; get; }
    }
}
