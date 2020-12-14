using Microsoft.Ajax.Utilities;
using Model.Dao;
using Newtonsoft.Json;
using OnlineShopCNPM.Common;
using OnlineShopCNPM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace OnlineShopCNPM.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        public ActionResult CartIndex()
        {
            var model = new List<CartItemModel>();
            if(Session[CommonConstants.CartSession] != null)
            {
                model = (List<CartItemModel>)Session[CommonConstants.CartSession];
                var sumOfPrice = new decimal();
                foreach (var item in model)
                {
                    sumOfPrice += (decimal)item.Amount * item.Price;
                }
                ViewBag.SumOfPrice = sumOfPrice;
            }
            return View(model);
        }
        public ActionResult CartIndexSub()
        {
            var model = new List<CartItemModel>();
            if (Session[CommonConstants.CartSession] != null)
            {
                model = (List<CartItemModel>)Session[CommonConstants.CartSession];
                var sum = 0;
                var sumOfPrice = new decimal();
                foreach(var item in model)
                {
                    sum += item.Amount;
                    sumOfPrice += (decimal)item.Amount * item.Price;
                }
                ViewBag.SumAmount = sum;
                ViewBag.SumOfPrice = sumOfPrice;
            }
            return PartialView(model);
        }
        [HttpPost]
        public JsonResult AddItem(int productCode, int Amount)
        {
            if(Session[CommonConstants.USER_SESSION] != null)
            {
                var product = new ProductDao().GetByProductCode(productCode);
                var cartItem = new CartItemModel();
                cartItem.ProductCode = productCode;
                cartItem.Image = product.ImageFirst;
                cartItem.MetaTitle = product.MetaTitle;
                cartItem.Name= product.Name;
                if (product.Promotion.Value)
                {
                    cartItem.Price = product.Price - (product.Price *(decimal) product.PercentSale.Value / 100);
                }
                else
                {
                    cartItem.Price = product.Price;
                }
                cartItem.Amount = Amount;
                if(Session[CommonConstants.CartSession] != null)
                {
                    var listItem = (List<CartItemModel>)Session[CommonConstants.CartSession];
                    bool exist = false;
                    foreach(var item in listItem)
                    {
                        if(item.ProductCode == cartItem.ProductCode)
                        {
                            item.Amount += cartItem.Amount;
                            exist = true;
                            break;
                        }
                    }
                    if (!exist)
                    {
                        listItem.Add(cartItem);
                    }
                    Session[CommonConstants.CartSession] = listItem;
                }
                else
                {
                    var listItem = new List<CartItemModel>();
                    listItem.Add(cartItem);
                    Session[CommonConstants.CartSession] = listItem;
                }
                Session[CommonConstants.ADD_ITEM] = cartItem.Name;
                return Json(new
                {
                    status = true
                });
            }
            return Json(new
            {
                status = false
            });
        }
        [HttpPost]
        public JsonResult Delete(int productCode)
        {
            var sessionCart = (List<CartItemModel>)Session[CommonConstants.CartSession];
            var minusAmount = 0;
            var nameItem = "";
            foreach (var item in sessionCart)
            {
                if (item.ProductCode == productCode)
                {
                    Session[CommonConstants.RESTORE_ITEM] = item;
                    minusAmount = item.Amount;
                    nameItem = item.Name;
                    sessionCart.Remove(item);
                    break;
                }
            }
            Session[CommonConstants.CartSession] = sessionCart;
            return Json(new
            {
                status = true,
                minusAmount = minusAmount,
                nameItem = nameItem
            });
        }
        [HttpPost]
        public JsonResult UpdateCart(string cartModel)
        {
            var jsonCart = JsonConvert.DeserializeObject<List<CartItemModel>>(cartModel);
            var listItem = new List<CartItemModel>();
            foreach(var item in jsonCart)
            {
                listItem.Add(item);
            }
            Session[CommonConstants.CartSession] = listItem;
            return Json(new
            {
                status = true,
            });
        }
        public ActionResult Restore()
        {
            var cartItem = (CartItemModel)Session[CommonConstants.RESTORE_ITEM];
            var listItem = (List<CartItemModel>)Session[CommonConstants.CartSession];
            listItem.Add(cartItem);
            Session[CommonConstants.CartSession] = listItem;
            return RedirectToAction("CartIndex");
        }
    }
}