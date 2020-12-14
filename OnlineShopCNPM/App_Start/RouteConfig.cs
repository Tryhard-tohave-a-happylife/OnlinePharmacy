using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineShopCNPM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
              name: "Product Category",
              url: "san-pham/{categoryID}/{metaTitle}/{order}",
              defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShopCNPM.Controllers" }
          );

            routes.MapRoute(
             name: "Sign up",
             url: "dang-ky",
             defaults: new { controller = "User", action = "SignUp", id = UrlParameter.Optional },
             namespaces: new[] { "OnlineShopCNPM.Controllers" }
         );
            routes.MapRoute(
            name: "Product Details",
            url: "chi-tiet/{productCode}/{metaTitle}/{order}",
            defaults: new { controller = "Product", action = "ProductDetail", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShopCNPM.Controllers" }
         );
            routes.MapRoute(
            name: "Search",
            url: "search",
            defaults: new { controller = "Product", action = "ListSearch", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShopCNPM.Controllers" }
         );
            routes.MapRoute(
            name: "Cart",
            url: "gio-hang",
            defaults: new { controller = "Cart", action = "CartIndex", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShopCNPM.Controllers" }
         );
            routes.MapRoute(
            name: "Payment",
            url: "thanh-toan",
            defaults: new { controller = "Payment", action = "Payment", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShopCNPM.Controllers" }
         );
            routes.MapRoute(
            name: "Order Details",
            url: "chi-tiet-don-hang/{orderID}",
            defaults: new { controller = "Order", action = "OrderDetail", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShopCNPM.Controllers" }
        );
            routes.MapRoute(
            name: "Orders Diary",
            url: "orders-diary/{userID}",
            defaults: new { controller = "Order", action = "DiaryOrder", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShopCNPM.Controllers" }
        );
            routes.MapRoute(
            name: "Shopping Speed",
            url: "shopping-speed",
            defaults: new { controller = "Order", action = "ListShoppingSpeed", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShopCNPM.Controllers" }
        );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "OnlineShop.Controllers" }
            );
        }
    }
}
