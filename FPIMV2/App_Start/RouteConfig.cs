using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FPIMV2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");
            routes.IgnoreRoute("*.js|css|swf");
            routes.IgnoreRoute("{*allashx}", new { allashx = @".*\.ashx(/.*)?" });

            //routes.RouteExistingFiles = true;

            routes.MapRoute(
                name: "FacultyMain",
                url: "faculty/{profileId}",
                defaults: new { controller = "Home", action = "ViewMainFacultyPage", pageId = UrlParameter.Optional, dept = "" }
                );

            routes.MapRoute(
                name: "Faculty",
                url: "faculty/{profileId}/{pageId}",
                defaults: new { controller = "Home", action = "ViewFacultyPage", 
                    pageId = UrlParameter.Optional, 
                    profileId = UrlParameter.Optional, 
                    fname = UrlParameter.Optional,
                    lnamedept  = UrlParameter.Optional },
                constraints: new { pageId = @"\d+" }
                );

            routes.MapRoute(
                  name: "FacultyAdmin",
                  url: "faculty/admin/{action}/{profileId}",
                  defaults: new { controller = "Home", action = "AdminIndex", profileId = UrlParameter.Optional, dept = "" });

            routes.MapRoute(
                  name: "FPIMIndex",
                  url: "faculty/admin/{action}/{id}",
                  defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, dept = "" });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, dept = "" }
            );



        }
    }
}