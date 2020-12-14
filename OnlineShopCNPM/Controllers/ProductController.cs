using Model.Dao;
using Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopCNPM.Common;
using Model.EF;

namespace OnlineShopCNPM.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Category(string categoryID, int order, int page = 1, int pageSize = 12, string orderBy = "default", string search = null)
        {
            ProductCategory pr = null;
            ProductCategory cr = null;
            int total = 0;
            List<ProductViewDisplay> model = null;
            if (categoryID != "default") {
                model = new ProductDao().ListViewProductByCategory(categoryID, order, ref pr, ref cr, page, pageSize, ref total, orderBy, search);
            }
            else
            {
                model = new ProductDao().ListViewProductBySearch(page, pageSize, ref total, orderBy, search);
            }
            Session[CommonConstants.LISTPRODUCT_SESSION] = model;
            string[][] ArrayOrderBy = new string[6][];
            ArrayOrderBy[0] = new string[] {"default", "Thứ tự mặc định"};
            ArrayOrderBy[1] = new string[] {"popular", "Thứ tự theo mức độ phổ biến" };
            ArrayOrderBy[2] = new string[] {"rate", "Thứ tự theo điểm đánh giá" };
            ArrayOrderBy[3] = new string[] {"new", "Mới nhất"};
            ArrayOrderBy[4] = new string[] {"price", "Thứ tự theo giá : thấp đến cao"};
            ArrayOrderBy[5] = new string[] {"priceDecs", "Thứ tự theo giá : cao đến thấp"};
            ViewBag.OrderBy = orderBy;
            ViewBag.ArrayOrderBy = ArrayOrderBy;
            if (categoryID != "default")
            {
                ViewBag.CategoryParent = null;
                if (order == 2)
                {
                    ViewBag.CategoryParent = pr;
                }
                ViewBag.Category = cr;
            }
            //ViewBag.Category = categoryID;
            ViewBag.Order = order;
            //Search
            ViewBag.Search = search;
            // Phân trang;
            var lastPage = (total % pageSize == 0) ? (int)(total / pageSize) : (int)(total / pageSize) + 1;
            ViewBag.PageIndex = page;
            ViewBag.TotalPage = lastPage;
            ViewBag.LastPage = lastPage;
            ViewBag.NextPage = (page == lastPage) ? 1 : page + 1;
            ViewBag.PrePage = (page == 1) ? lastPage : page - 1;
            return View(model);
        }
        public ActionResult ProductDetail(int productCode, int order, bool inCategory = false)
        {
            ProductCategory pr = null;
            ProductCategory cr = null;
            var viewCount = new ProductDao().IncreaseViewCount(productCode);
            var model = new ProductDao().FindProductByCode(productCode, ref pr, ref cr);
            var listReview = new ReviewDao().ListReviewByProductCode(productCode);
            ViewBag.Category = pr;
            ViewBag.CategoryChild = null;
            ViewBag.ListReview = listReview;
            if (order == 2)
            {
                ViewBag.CategoryChild = cr;
            }
            ViewBag.Order = order;
            if (!inCategory)
            {
                var listDao = new ProductDao().ListProductNotInCategoryBefore(model.CategoryIDParent);
                Session[CommonConstants.LISTPRODUCT_SESSION] = listDao;
            }
            var listProduct = (List<ProductViewDisplay>)Session[CommonConstants.LISTPRODUCT_SESSION];
            var ind = -1;
            var listLength = listProduct.Count;
            for(int i = 0; i < listLength; i++)
            {
                if(listProduct[i].ID == productCode)
                {
                    ind = i;
                    break;
                }
            }
            var pre_item = (ind == 0) ? listLength - 1 : ind - 1;
            var nextpre_item = (ind == listLength - 1) ? 0 : ind + 1;
            ViewBag.PreItem = listProduct[pre_item];
            ViewBag.NextItem = listProduct[nextpre_item];
            return View(model);
        }
        public JsonResult ListSearch(string q, string category = "default")
        {
            var model = new ProductDao().ListProductViewSearch(q, category);
            return Json(new
            {
                data = model
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ReturnProduct(int productCode)
        {
            var model = new ProductDao().GetByProductCode(productCode);
            var category = new ProductCategoryDao().TakeCategory(model.CategoryIDParent);
            return Json(new
            {
                data = model,
                status = true,
                nameCategory = category.Name,
                metaTitleCate = category.MetaTitle,
                categoryID = category.ID
            }); 
        }
        public ActionResult TopBuyProduct()
        {
            var model = new ProductDao().TopBuyProduct(10);
            ViewBag.ListName = new ProductCategoryDao().NameCategory(model);
            return PartialView(model);
        }
        public ActionResult TopBuyProductLess()
        {
            var model = new ProductDao().TopBuyProduct(5);
            return PartialView(model);
        }
        public ActionResult TopViewProduct()
        {
            var model = new ProductDao().TopViewProduct(10);
            ViewBag.ListName = new ProductCategoryDao().NameCategory(model);
            return PartialView(model);
        }
        public ActionResult TopViewProductLess()
        {
            var model = new ProductDao().TopViewProduct(5);
            return PartialView(model);
        }
        public ActionResult TopReviewProduct()
        {
            var model = new ProductDao().TopReviewProduct();
            return PartialView(model);
        }
        public ActionResult TopNewProduct()
        {
            var model = new ProductDao().TopNewProduct();
            return PartialView(model);
        }
        public ActionResult TopPromotionProduct()
        {
            var model = new ProductDao().TopPromotionProduct();
            ViewBag.ListName = new ProductCategoryDao().NameCategory(model);
            return PartialView(model);
        }
    }
}