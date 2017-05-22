using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CMS.Mvc;
using System.Web.Routing;

namespace CMS.Mvc
{
    /// <summary>
    /// Class providing manipulation with the application routes.
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Registers the application routes.
        /// </summary>
        /// <param name="routes">The routes collection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            //routes.Ignore("{resource}.axd/{*pathInfo}");
            //routes.Ignore("Scripts/{*pathInfo}"); routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}