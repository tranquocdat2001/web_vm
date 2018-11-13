using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VeganMart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Single Page

            routes.MapRoute(
              name: "Guide",
              url: "huong-dan-mua-hang",
              defaults: new { controller = "Home", action = "Guide" }
            );
            routes.MapRoute(
              name: "TradingAccount",
              url: "tai-khoan-giao-dich",
              defaults: new { controller = "Home", action = "TradingAccount" }
            );

            routes.MapRoute(
              name: "PrivacyPolicy",
              url: "chinh-sach-bao-mat",
              defaults: new { controller = "Home", action = "PrivacyPolicy" }
            );
            routes.MapRoute(
              name: "AboutUs",
              url: "gioi-thieu",
              defaults: new { controller = "Home", action = "AboutUs" }
            );
            routes.MapRoute(
              name: "Contact",
              url: "lien-he",
              defaults: new { controller = "Home", action = "Contact" }
            );
            routes.MapRoute(
              name: "Map",
              url: "ban-do",
              defaults: new { controller = "Home", action = "Map" }
            );

            #endregion

            #region Cart


            routes.MapRoute(
              name: "ShoppingCart",
              url: "gio-hang",
              defaults: new { controller = "ShoppingCart", action = "Cart" }
            );

            routes.MapRoute(
              name: "CheckOut",
              url: "thanh-toan",
              defaults: new { controller = "ShoppingCart", action = "CheckOut" }
            );

            routes.MapRoute(
             name: "CheckOrder",
             url: "kiem-tra-don-hang",
             defaults: new { controller = "ShoppingCart", action = "CheckOrder" }
            );

            routes.MapRoute(
             name: "PaymentEnd",
             url: "hoan-tat",
             defaults: new { controller = "ShoppingCart", action = "PaymentEnd" }
            );

            #endregion

            #region Account

            routes.MapRoute(
              name: "Login",
              url: "dang-nhap",
              defaults: new { controller = "Account", action = "Login" }
            );
            routes.MapRoute(
              name: "Register",
              url: "dang-ky",
              defaults: new { controller = "Account", action = "Register" }
            );
            routes.MapRoute(
              name: "ForgotPassword",
              url: "quen-mat-khau",
              defaults: new { controller = "Account", action = "ForgotPassword" }
            );

            #endregion

            #region Member   

            routes.MapRoute(
              name: "AccountInformation",
              url: "thong-tin-tai-khoan",
              defaults: new { controller = "Member", action = "AccountInformation" }
            );

            routes.MapRoute(
              name: "ChangePassword",
              url: "thay-doi-mat-khau",
              defaults: new { controller = "Member", action = "ChangePassword" }
            );

            routes.MapRoute(
              name: "MyOrder",
              url: "don-hang",
              defaults: new { controller = "Member", action = "MyOrder" }
            );
            routes.MapRoute(
              name: "MyOrderDetail",
              url: "don-hang/{id}",
              defaults: new { controller = "Member", action = "MyOrderDetail" },
               constraints: new { id = @"\d+" }
            );

            #endregion

            #region Article

            routes.MapRoute(
              name: "ArticleDetail",
              url: "{alias}/{title}-ar{articleId}",
              defaults: new { controller = "Article", action = "ArticleDetail" },
              constraints: new { articleId = @"\d+" }
            );

            routes.MapRoute(
              name: "Article",
              url: "tin-tuc",
              defaults: new { controller = "Article", action = "Index" }
            );

            #endregion

            #region Product

            routes.MapRoute(
              name: "ProductDetail",
              url: "{alias}/{title}-id{productId}",
              defaults: new { controller = "Product", action = "ProductDetail" },
              constraints: new { productId = @"\d+" }
            );

            routes.MapRoute(
              name: "ProductListPaing",
              url: "{alias}/p{pageIndex}",
              defaults: new { controller = "Product", action = "Index" },
              constraints: new { pageIndex = @"\d+" }
            );

            routes.MapRoute(
              name: "ProductList",
              url: "{alias}",
              defaults: new { controller = "Product", action = "Index" }
            );

            #endregion

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
