using Model.EF;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ProductDao
    {
        OnlShopCNPM db = null;
        public ProductDao()
        {
            db = new OnlShopCNPM();
        }
        public List<ProductViewDisplay> ListViewProductByCategory(string categoryCode, int order, ref ProductCategory pr, ref ProductCategory cr, int pageIndex, int pageSize, ref int total, string orderBy, string search)
        {
            if(search == null)
            {
                search = "";
            }
            search = convertToUnSign3(search);
            string[] ss = search.Split(' ');
            search = ss[0];
            for (int i = 1; i < ss.Length; i++)
            {
                search += "-" + ss[i];
            }
            total = db.Products.Count(x => (x.CategoryIDParent == categoryCode || x.CategoryIDChild == categoryCode) && x.MetaTitle.Contains(search));
            cr = db.ProductCategories.Find(categoryCode);
            if(order == 2)
            {
                pr = db.ProductCategories.Find(cr.ParentID);
            }
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryIDParent equals b.ID
                         where (a.CategoryIDParent == categoryCode || a.CategoryIDChild == categoryCode) 
                                && a.MetaTitle.Contains(search)
                         select new
                         {
                             CategoryTitle = b.MetaTitle.ToUpper(),
                             ID = a.ProductCode,
                             Images = a.ImageFirst,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PercentSale = a.PercentSale.Value,
                             BuyCount = a.BuyCount,
                             ReviewPoint = a.ReviewPoint
                         }).AsEnumerable().Select(x => new ProductViewDisplay()
                         {
                             CategoryTitle = x.CategoryTitle,
                             ID = x.ID,
                             Image = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PercentSale = x.PercentSale,
                             BuyCount = x.BuyCount.Value,
                             ReviewPoint = x.ReviewPoint.Value
                         });
            if (orderBy == "default")
            {
                model = model.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if(orderBy == "popular")
            {
                model = model.OrderByDescending(x => x.BuyCount).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if(orderBy == "rate")
            {
                model = model.OrderByDescending(x => x.ReviewPoint).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if(orderBy == "new")
            {
                model = model.OrderByDescending(x => x.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if(orderBy == "price")
            {
                model = model.OrderBy(x => x.Price).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if(orderBy == "priceDecs")
            {
                model = model.OrderByDescending(x => x.Price).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return model.ToList();
        }
        public List<ProductViewDisplay> ListViewProductBySearch(int pageIndex, int pageSize, ref int total, string orderBy, string search)
        {
            if (search == null)
            {
                search = "";
            }
            search = convertToUnSign3(search);
            string[] ss = search.Split(' ');
            search = ss[0];
            for (int i = 1; i < ss.Length; i++)
            {
                search += "-" + ss[i];
            }
            total = db.Products.Count(x => x.MetaTitle.Contains(search));
            var model = (from a in db.Products
                         join b in db.ProductCategories
                         on a.CategoryIDParent equals b.ID
                         where a.MetaTitle.Contains(search)
                         select new
                         {
                             CategoryTitle = b.MetaTitle.ToUpper(),
                             ID = a.ProductCode,
                             Images = a.ImageFirst,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PercentSale = a.PercentSale.Value,
                             BuyCount = a.BuyCount,
                             ReviewPoint = a.ReviewPoint
                         }).AsEnumerable().Select(x => new ProductViewDisplay()
                         {
                             CategoryTitle = x.CategoryTitle,
                             ID = x.ID,
                             Image = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PercentSale = x.PercentSale,
                             BuyCount = x.BuyCount.Value,
                             ReviewPoint = x.ReviewPoint.Value
                         });
            if (orderBy == "default")
            {
                model = model.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if (orderBy == "popular")
            {
                model = model.OrderByDescending(x => x.BuyCount).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if (orderBy == "rate")
            {
                model = model.OrderByDescending(x => x.ReviewPoint).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if (orderBy == "new")
            {
                model = model.OrderByDescending(x => x.ID).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if (orderBy == "price")
            {
                model = model.OrderBy(x => x.Price).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            else if (orderBy == "priceDecs")
            {
                model = model.OrderByDescending(x => x.Price).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            return model.ToList();
        }
        public Product FindProductByCode(int productCode, ref ProductCategory pr, ref ProductCategory cr)
        {
            var product = db.Products.Find(productCode);
            pr = db.ProductCategories.Find(product.CategoryIDParent);
            if(product.CategoryIDChild != null)
            {
                cr = db.ProductCategories.Find(product.CategoryIDChild);
            }
            return product;
        }
        private string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        public List<ProductViewDisplay> ListProductViewSearch(string q, string category)
        {
            q = convertToUnSign3(q);
            string[] ss = q.Split(' ');
            q = ss[0];
            for(int i = 1; i<ss.Length; i++)
            {
                q += "-" + ss[i];
            }
            if(category == "default")
            {
                category = "";
            }
            var model = (from a in db.Products
                         where a.MetaTitle.Contains(q)
                         && (a.CategoryIDParent == category || a.CategoryIDParent.Contains(category))
                         select new
                         {
                             ID = a.ProductCode,
                             Images = a.ImageFirst,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PercentSale = a.PercentSale.Value
                         }).AsEnumerable().Select(x => new ProductViewDisplay()
                         {
                             ID = x.ID,
                             Image = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PercentSale = x.PercentSale
                         });
            return model.Take(20).ToList();
        }
        public List<ProductViewDisplay> ListProductNotInCategoryBefore(string catrgoryID)
        {
            var model = (from a in db.Products
                         where a.CategoryIDParent == catrgoryID
                         select new
                         {
                             ID = a.ProductCode,
                             Images = a.ImageFirst,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price,
                             PercentSale = a.PercentSale.Value
                         }).AsEnumerable().Select(x => new ProductViewDisplay()
                         {
                             ID = x.ID,
                             Image = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price,
                             PercentSale = x.PercentSale
                         });
            return model.ToList();
        }
        public Product GetByProductCode(int productCode)
        {
            return db.Products.Find(productCode);
        }
        public bool IncreaseBuyCount(int productCode, int amount)
        {
            var model = db.Products.Find(productCode);
            model.BuyCount += amount;
            db.SaveChanges();
            return true;
        }
        public bool IncreaseViewCount(int productCode)
        {
            var model = db.Products.Find(productCode);
            model.ViewCount += 1;
            db.SaveChanges();
            return true;
        }
        public bool UpdateReview(int productCode, double reviewStar)
        {
            var model = db.Products.Find(productCode);
            model.ReviewPoint = (model.ReviewPoint + reviewStar) / 2;
            return true;
        }
        public List<Product> TopBuyProduct(int amount)
        {
            var model = db.Products.OrderByDescending(x => x.BuyCount).Take(amount).ToList();
            return model;
        }
        public List<Product> TopViewProduct(int amount)
        {
            var model = db.Products.OrderByDescending(x => x.ViewCount).Take(amount).ToList();
            return model;
        }
        public List<Product> TopReviewProduct()
        {
            var model = db.Products.OrderByDescending(x => x.ReviewPoint).Take(5).ToList();
            return model;
        }
        public List<Product> TopNewProduct()
        {
            var model = db.Products.OrderBy(x => x.CreatedDate).Take(5).ToList();
            return model;
        }
        public List<Product> TopPromotionProduct()
        {
            var model = db.Products.Where(x => x.PercentSale.Value != 0).OrderBy(x => x.PercentSale.Value).Take(10).ToList();
            return model;
        }
        public string ImageProduct(int productCode)
        {
            return db.Products.Find(productCode).ImageFirst;
        }

    }
}
