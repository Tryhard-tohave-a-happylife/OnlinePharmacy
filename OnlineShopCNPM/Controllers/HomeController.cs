using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineShopCNPM.Common;

namespace OnlineShopCNPM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var a = new UserLogin();
            return View();
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600*24)]
        public ActionResult SearchTop()
        {
            var model = new ProductCategoryDao().ListProductCategory(false);
            return PartialView(model);
        }
        [ChildActionOnly]
        [OutputCache(Duration = 3600 * 24)]
        public ActionResult SearchResponsive()
        {
            var model = new ProductCategoryDao().ListProductCategory(false);
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult CheckLogin()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult InforBox()
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.Amount = new UserDao().returnAmountMiss(user.UserID);
            return PartialView();
        }

    }
}