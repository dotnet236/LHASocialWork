using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LHASocialWork.Controllers;
using LHASocialWork.Core;
using LHASocialWork.Core.Routing;
using MvcSiteMapProvider.Web;
using StructureMap;
using Elmah;

namespace LHASocialWork
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        private readonly IDictionary<int, string> _httpExceptionRoutes = new Dictionary<int, string>
                                                                   {
                                                                       {404, "~/Home/PageNotFound"}
                                                                   };

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{folder}", new { folder = "Script" });
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new { controller = new EventRouteContraint() },
                new[] { typeof(HomeController).Namespace });

            routes.MapRoute(
                "Events",
                "events/{action}",
                new {controller = "events", action = "index", area = "", eventName = ""},
                new {action = new EventActionRouteContraint() },
                new[] {typeof (HomeController).Namespace});

            routes.MapRoute(
                "EventAction",
                "events/{eventName}/{action}",
                new {controller = "events", action = "event", area = ""},
                new[] {typeof (HomeController).Namespace});

            routes.MapRoute(
                "Event",
                "events/{eventName}",
                new {controller = "events", action = "event", area = "", eventName = ""},
                new[] {typeof (HomeController).Namespace});

        }

        protected void Application_Start()
        {
            Bootstrapper.Initialize();

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
            RegisterRoutes(RouteTable.Routes);
            //RouteDebug.RouteDebugger.RewriteRoutesForTesting(RouteTable.Routes);
        }

        protected void Application_EndRequest()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();
            var httpException = exception as HttpException;

            if (httpException == null) return;

            var logger = ErrorLog.GetDefault(Context);
            if (logger != null)
                logger.Log(new Error(exception));

            var redirectUrl = "~/Home/UnhandledError";
            if (_httpExceptionRoutes.ContainsKey(httpException.ErrorCode))
                redirectUrl = _httpExceptionRoutes[httpException.ErrorCode];
            Response.Redirect(redirectUrl);
        }
    }
}