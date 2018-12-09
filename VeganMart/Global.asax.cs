using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Utilities;

namespace VeganMart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_BeginRequest()
        {
            // bắt ip crawler
            KillCrawlerHelper.CrawlerIPDetected();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            bool debugMode = AppSettings.Instance.GetBool("DebugMode", false);
            if (debugMode) return;

            Exception ex = Server.GetLastError().GetBaseException();
            string errorMessage = string.Concat(Environment.NewLine, "IsMobileDevice: ", Request.Browser.IsMobileDevice, " ==> ", Environment.NewLine, Request.RawUrl.ToString(), Environment.NewLine, "Refer: ", Request.UrlReferrer, Environment.NewLine, "From: ", Utils.GetIP(), Environment.NewLine, ex.ToString(), Environment.NewLine, ex.StackTrace, Environment.NewLine, "====================", Environment.NewLine);

            if (ex.Message.IndexOf("was not found") != -1)
            {
                Logger.WriteLog(Logger.LogType.Trace, errorMessage);
                Response.StatusCode = 404;
            }
            else
            {
                Logger.WriteLog(Logger.LogType.Error, errorMessage);
                if (HttpContext.Current.Request.Path.ToLower() != "/")
                    Response.Redirect("/");
                else
                    Response.Redirect("/upgrade.html");
            }
        }
    }
}
