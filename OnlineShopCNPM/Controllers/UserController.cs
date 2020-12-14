using Model.Dao;
using OnlineShop;
using OnlineShopCNPM.Common;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using OnlineShopCNPM.Models;
using System.Xml.Linq;
using Newtonsoft.Json;
using BotDetect.Web.Mvc;

namespace OnlineShopCNPM.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            if(Session[CommonConstants.USER_SESSION] != null)
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.Username;
                    string[] str = user.Name.Split(' ');
                    userSession.FullName = user.Name;
                    userSession.Name = str[str.Length - 1];
                    userSession.NameFirst = str[0];
                    userSession.UserID = user.ID;
                    userSession.Email = user.Email;
                    userSession.Mobile = user.Mobile;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }
            }
            return View(model);
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.CartSession] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [CaptchaValidationActionFilter("CaptchaCode", "registerCapcha", "Wrong Captcha!")]
        public ActionResult SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ViewBag.Status = "duplicate";
                }
                else
                {
                    var user = new User();
                    user.ID = Guid.NewGuid();
                    user.Username = model.UserName;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Mobile = model.Mobile;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.TypeOfAccount = 1;
                    user.Status = true;
                    if (!string.IsNullOrEmpty(model.ProvinceID))
                    {
                        user.ProvinceID = int.Parse(model.ProvinceID);
                    }
                    if (!string.IsNullOrEmpty(model.DistrictID))
                    {
                        user.DistrictID = int.Parse(model.DistrictID);
                    }
                    if (!string.IsNullOrEmpty(model.SubDistrictID))
                    {
                        user.SubDistrictID = int.Parse(model.SubDistrictID);
                    }

                    var result = dao.Insert(user);
                    if (result != null)
                    {
                        ViewBag.Status = "success";
                        model = new SignUpModel();
                    }
                    else
                    {
                        ViewBag.Status = "fail";
                    }
                }
            }
            return View(model);
        }
        public JsonResult LoadProvince()
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/client/data/Provinces_Data.xml"));
            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public JsonResult LoadDistrict(int provinceID)
        {
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/client/data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item")
               .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElement.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElement.Attribute("id").Value);
                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public JsonResult LoadSubDistrict(string infor)
        {
            var jsonCart = JsonConvert.DeserializeObject<SubDistrictModel>(infor);
            int provinceID = jsonCart.ProvinceID;
            int distinctID = jsonCart.DistrictID;
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/client/data/Provinces_Data.xml"));
            var xElement = xmlDoc.Element("Root").Elements("Item")
               .Single(x => x.Attribute("type").Value == "province" && int.Parse(x.Attribute("id").Value) == provinceID);
            var xElementSecond = xElement.Elements("Item").
                Single(x => x.Attribute("type").Value == "district" && int.Parse(x.Attribute("id").Value) == distinctID);
            var list = new List<SubDistrictModel>();
            SubDistrictModel subDistrictModel = null;
            foreach (var item in xElementSecond.Elements("Item").Where(x => x.Attribute("type").Value == "precinct"))
            {
                subDistrictModel = new SubDistrictModel();
                subDistrictModel.ID = int.Parse(item.Attribute("id").Value);
                subDistrictModel.Name = item.Attribute("value").Value;
                subDistrictModel.ProvinceID = provinceID;
                subDistrictModel.DistrictID = distinctID;
                list.Add(subDistrictModel);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public ActionResult CheckMiss()
        {
            var user = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.Amount = new UserDao().returnAmountMiss(user.UserID);
            return PartialView();
        }
    }
}