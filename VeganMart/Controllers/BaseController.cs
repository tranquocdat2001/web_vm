
using System.Web.Mvc;
using Utilities;

namespace VeganMart.Controllers
{
    public class BaseController : Controller
    {
        protected ActionResult Redirect301(string standardUrl)
        {
            string destinationUrl = standardUrl;

            //destinationUrl = destinationUrl.Replace(Const.BaseUrlNoSlash, string.Empty);

            if (!destinationUrl.StartsWith("/")) destinationUrl = string.Concat("/", destinationUrl);

            destinationUrl = string.Concat(Const.BaseUrlNoSlash, destinationUrl);

            return RedirectPermanent(destinationUrl);
        }

        protected bool IsMobile()
        {
            return System.Web.HttpContext.Current.Request != null && System.Web.HttpContext.Current.Request.Browser.IsMobileDevice || DetectDevice.Instance.BrowserIsMobile();
        }
    }
}