using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Storage.App_Start;

namespace Storage
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("ImagesRoute",
                new Route(
                    url: "crop/{width}x{height}/{*path}",
                    defaults: new RouteValueDictionary(),
                    constraints: new RouteValueDictionary(new { width = @"[0-9-]+", height = @"[0-9-]+", path = @".+" }),
                    routeHandler: new ImageRouteHandler()
                    )
            );

            routes.Add("ImagesResize",
                new Route(
                    url: "resize/{resizeWidth}x{resizeHeight}/{*path}",
                    defaults: new RouteValueDictionary(),
                    constraints: new RouteValueDictionary(new { resizeWidth = @"[0-9-]+", resizeHeight = @"[0-9-]+", path = @".+" }),
                    routeHandler: new ImageRouteHandler()
                    )
            );
        }
    }
}
