using Model.Dao;
using OnlineShopCNPM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopCNPM.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult OrderDetail(Guid orderID)
        {
            var ordDao = new OrderDao();
            var order = ordDao.FindByID(orderID);
            var checkStatus = ordDao.SwitchStatus(orderID);
            ViewBag.Status = order.Status; 
            if(order.Status == "Complete")
            {
                ViewBag.ShippedDate = order.ShippedDate;
            }
            ViewBag.ID = order.ID.ToString().Substring(0,8);
            ViewBag.OrderDate = order.OrderDate;
            string typeOfCost = "";
            if(order.CostBy == 1)
            {
                typeOfCost = "Trả tiền mặt khi nhận hàng";
            }
            else
            {
                typeOfCost = "Chuyển khoản qua ngân hàng";
            }
            ViewBag.TypeOfCost = typeOfCost;
            var model = new OrderDetailDao().ListByID(orderID);
            var sumOfCost = new decimal();
            foreach(var item in model)
            {
                sumOfCost += (item.Price * item.Amount);
            }
            ViewBag.SumOfCost = sumOfCost;
            return View(model);
        }
        public ActionResult DiaryOrder(Guid userID)
        {
            var model = new OrderDao().ListByUser(userID);
            return View(model);
        }
        public ActionResult ListShoppingSpeed()
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var model = new OrderDao().ListCompleteOrderByCustomer(user.UserID);
            return View(model);
        }
        [HttpPost]
        public JsonResult OrderDetailByOrderID(Guid orderID)
        {
            var model = new OrderDetailDao().ListByID(orderID);
            var amount = new decimal();
            foreach(var item in model)
            {
                amount += (item.Price * (decimal)item.Amount);
            }
            return Json(new
            {
                data = model,
                status = true,
                amount = amount
            }); 
        }
    }
}