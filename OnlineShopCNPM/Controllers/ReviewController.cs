using Model.Dao;
using Model.EF;
using OnlineShopCNPM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopCNPM.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddReview(int productCode, string textReview, double reviewPoint) {
            var sessionUser = (UserLogin)Session[CommonConstants.USER_SESSION];
            if(sessionUser == null)
            {
                return Json(new
                {
                    status = false
                });
            }
            var updateReviewProduct = new ProductDao().UpdateReview(productCode, reviewPoint);
            var review = new Review();
            review.UserID = sessionUser.UserID;
            review.ProductCode = productCode;
            review.CreatedDate = DateTime.Now;
            review.Status = "Censorship";
            review.ReviewPoint = reviewPoint;
            review.Text = textReview;
            review.NameUser = new UserDao().ReturnUserFullName(sessionUser.UserID);
            var addReview = new ReviewDao().AddReview(review);
            return Json(new
            {
                status = true,
                nameUser = sessionUser.FullName,
                date = review.CreatedDate.ToString()
            }); ;
        }
        public ActionResult ListReview()
        {
            var model = new ReviewDao().ListReview();
            var listImage = new List<String>();
            var prd = new ProductDao();
            foreach(var item in model)
            {
                listImage.Add(prd.ImageProduct(item.ProductCode));
            }
            ViewBag.ListImage = listImage;
            return PartialView(model);
        }
    }
}