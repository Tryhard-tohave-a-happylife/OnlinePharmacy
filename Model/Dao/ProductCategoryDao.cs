using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductCategoryDao
    {
        OnlShopCNPM db = null;
        public ProductCategoryDao()
        {
            db = new OnlShopCNPM();
        }
        public List<ProductCategory> ListProductCategory(bool takeAll)
        {
            if (!takeAll)
            {
                return db.ProductCategories.Where(x => x.Status == true && x.DisplayOrder == 1).OrderBy(x => x.Name).ToList();
            }
            else
            {
                return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.Name).ToList();
            }
        }
        public string NameCategory(string categoryID)
        {
            return db.ProductCategories.Find(categoryID).Name;
        }
        public ProductCategory TakeCategory(string categoryID)
        {
            return db.ProductCategories.Find(categoryID);
        }
        public List<String> NameCategory(List<Product> model)
        {
            var listName = new List<String>();
            foreach(var item in model)
            {
                listName.Add(NameCategory(item.CategoryIDParent));
            }
            return listName;
        }
    }
}
