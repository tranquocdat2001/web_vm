using System.Web.Mvc;
using Utilities;

namespace VeganMart.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            #region Redirect Permanent 301

            var strCurrUrl = string.IsNullOrEmpty(Request.RawUrl) ? "" : "/" + Request.RawUrl.TrimStart('/');
            string standardUrl = Const.BaseUrlNoSlash;
            string currentUrl = Const.BaseUrlNoSlash + (strCurrUrl.Equals("/") ? "" : strCurrUrl);

            if (!currentUrl.Equals(standardUrl))
            {
                return Redirect301(standardUrl);
            }

            #endregion

            string metaTags = SEO.Instance.BindingMeta("", Const.MetaMainTitle, Const.MetaMainDescription, Const.MetaMainKeyword);
            ViewBag.MetaTitle = Const.MetaMainTitle;
            ViewBag.Meta = metaTags;
            ViewBag.IsHome = true;

            return View();
        }

        public ActionResult Guide()
        {
            string metaTags = SEO.Instance.BindingMeta("", Const.MetaMainTitle, Const.MetaMainDescription, Const.MetaMainKeyword);
            ViewBag.MetaTitle = Const.MetaMainTitle;
            ViewBag.Meta = metaTags;

            return View();
        }
        public ActionResult TradingAccount()
        {
            string metaTags = SEO.Instance.BindingMeta("", Const.MetaMainTitle, Const.MetaMainDescription, Const.MetaMainKeyword);
            ViewBag.MetaTitle = Const.MetaMainTitle;
            ViewBag.Meta = metaTags;

            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            string metaTags = SEO.Instance.BindingMeta("", Const.MetaMainTitle, Const.MetaMainDescription, Const.MetaMainKeyword);
            ViewBag.MetaTitle = Const.MetaMainTitle;
            ViewBag.Meta = metaTags;

            return View();
        }
        public ActionResult AboutUs()
        {
            string metaTags = SEO.Instance.BindingMeta("", Const.MetaAboutTitle, Const.MetaAboutDescription);
            ViewBag.MetaTitle = Const.MetaMainTitle;
            ViewBag.Meta = metaTags;

            return View();
        }
        public ActionResult Contact()
        {
            string metaTags = SEO.Instance.BindingMeta("", Const.MetaMainTitle, Const.MetaMainDescription);
            ViewBag.MetaTitle = Const.MetaMainTitle;
            ViewBag.Meta = metaTags;

            return View();
        }
        public ActionResult Map()
        {
            string metaTags = SEO.Instance.BindingMeta("", Const.MetaMainTitle, Const.MetaMainDescription);
            ViewBag.MetaTitle = Const.MetaMainTitle;
            ViewBag.Meta = metaTags;

            return View();
        }
    }
}