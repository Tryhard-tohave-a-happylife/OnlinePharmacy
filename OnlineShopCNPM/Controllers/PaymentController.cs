using Antlr.Runtime.Misc;
using Common;
using Model.Dao;
using Model.EF;
using Newtonsoft.Json;
using OnlineShopCNPM.Common;
using OnlineShopCNPM.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopCNPM.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        public ActionResult Payment()
        {
            var userSession = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.Name = userSession.Name;
            ViewBag.FirstName = userSession.NameFirst;
            ViewBag.Mobile = userSession.Mobile;
            ViewBag.Email = userSession.Email;
            var model = (List<CartItemModel>)Session[CommonConstants.CartSession];
            return View(model);
        }
        [HttpPost]
        public JsonResult SendOrder(string shipName, string mobile, string address, string email)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var order = new Order();
            //var dataString = putData.Split('-');
            order.ID = Guid.NewGuid();
            order.OrderDate = DateTime.Now;
            order.ChangeDate = DateTime.Now;
            order.Mobile = mobile;
            order.Address = address;
            order.Email = email;
            order.CustomerID = user.UserID;
            order.CostBy = 1;
            order.Status = "Order";
            order.ChangeStatus = false;
            var id = new OrderDao().Insert(order);
            try
            {
                var cart = (List<CartItemModel>)Session[CommonConstants.CartSession];
                var detailDao = new OrderDetailDao();
                var listProduct = "";
                decimal total = 0;
                foreach (var item in cart)
                {
                    var add = new ProductDao().IncreaseBuyCount(item.ProductCode, item.Amount);
                    var orderDetails = new OrderDetail();
                    orderDetails.ProductCode =  item.ProductCode;
                    orderDetails.OrderID = id;
                    orderDetails.Price = item.Price;
                    orderDetails.Amount = item.Amount;
                    orderDetails.MetaTitle = item.MetaTitle;
                    orderDetails.ProductName = item.Name;
                    detailDao.Insert(orderDetails);
                    listProduct += "\n" + item.Name + " x" + item.Amount + "c " + (item.Price * item.Amount) + 'đ';
                    total += (item.Price * item.Amount);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/newOrder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Products}}", listProduct);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ OnlineShop", content);
                var sendRequest = SendAcceptShipper(order.ID, listProduct, shipName, mobile, email, address, total);
                //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = false
                }); ;
                throw;
            }
            return Json(new
            {
                status = true,
                orderID = order.ID
            }); ;
        }
        [HttpPost]
        public JsonResult SendOrderQuick(string cartModel, Guid orderID)
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            var oldOrder = new OrderDao().FindByID(orderID);
            var newOrder = new Order();
            newOrder.ID = Guid.NewGuid();
            newOrder.Email = oldOrder.Email;
            newOrder.CustomerID = oldOrder.CustomerID;
            newOrder.CostBy = oldOrder.CostBy;
            newOrder.Address = oldOrder.Address;
            newOrder.ChangeStatus = false;
            newOrder.Status = "Order";
            newOrder.Mobile = oldOrder.Mobile;
            newOrder.OrderDate = DateTime.Now;
            newOrder.ChangeDate = DateTime.Now;
            var jsonShopping = JsonConvert.DeserializeObject<List<OrderDetailsModel>>(cartModel);
            var id = new OrderDao().Insert(newOrder);
            try
            {
                var detailDao = new OrderDetailDao();
                var listProduct = "";
                decimal total = 0;
                foreach (var item in jsonShopping)
                {
                    var add = new ProductDao().IncreaseBuyCount(item.ProductCode, item.Amount);
                    var orderDetails = new OrderDetail();
                    orderDetails.ProductCode = item.ProductCode;
                    orderDetails.OrderID = id;
                    orderDetails.Price = item.Price;
                    orderDetails.Amount = item.Amount;
                    orderDetails.MetaTitle = item.MetaTitle;
                    orderDetails.ProductName = item.ProductName;
                    detailDao.Insert(orderDetails);
                    listProduct += "\n" + item.ProductName + " x" + item.Amount + "c " + (item.Price * item.Amount) + 'đ';
                    total += (item.Price * item.Amount);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/newOrder.html"));

                content = content.Replace("{{CustomerName}}", user.FullName);
                content = content.Replace("{{Phone}}", newOrder.Mobile);
                content = content.Replace("{{Email}}", newOrder.Email);
                content = content.Replace("{{Address}}", newOrder.Address);
                content = content.Replace("{{Products}}", listProduct);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();

                new MailHelper().SendMail(newOrder.Email, "Đơn hàng mới từ OnlineShop", content);
                var sendRequest = SendAcceptShipper(id, listProduct, user.FullName, newOrder.Mobile, newOrder.Email, newOrder.Address, total);
                //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = false
                }); ;
                throw;
            }
            return Json(new
            {
                status = true,
                orderID = id
            });
        }
        public bool SendAcceptShipper(Guid orderID , string listProduct, string shipName, string mobile, string email, string address, decimal total)
        {
            try
            {
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/SendAcceptShipper.html"));
                content = content.Replace("{{OrderID}}", orderID.ToString().Substring(0, 8));
                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Products}}", listProduct);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var url = "https://localhost:44333/" + "Payment/ConfirmFirst?orderID=" + orderID;
                content = content.Replace("{{Link}}", url);

                new MailHelper().SendMail("pmk200900081098@gmail.com", "Đơn giao hàng mới từ OnlineShop", content);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public bool SendAcceptComplete(Guid orderID)
        {
            try
            {
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/SendAcceptComplete.html"));
                content = content.Replace("{{OrderID}}", orderID.ToString().Substring(0, 8));
                var url = "https://localhost:44333/" + "Payment/ConfirmComplete?orderID=" + orderID;
                content = content.Replace("{{Link}}", url);

                new MailHelper().SendMail("pmk200900081098@gmail.com", "Xác nhận giao hàng OnlineShop", content);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public ActionResult ConfirmFirst(Guid orderID = default(Guid))
        {
            var ordDao = new OrderDao();
            var changeStatus = ordDao.ChangeStatusFirst(orderID);
            var sendRequest = SendAcceptComplete(orderID);
            return RedirectToAction("Index","Home");
        }
        public ActionResult ConfirmComplete(Guid orderID = default(Guid))
        {
            var changeStatus = new OrderDao().ChangeStatusSecond(orderID);
            return RedirectToAction("Index", "Home");
        }
    }
}