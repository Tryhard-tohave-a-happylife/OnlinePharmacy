using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopCNPM.Controllers
{
    public class ProductCategoryController : Controller
    {
        // GET: ProductCategory
        public ActionResult ListProductCategory()
        {
            var model = new ProductCategoryDao().ListProductCategory(true);
            return PartialView(model);
        }
    }
}