using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LHASocialWork.Core.Routing
{
    public class EventRouteContraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return values.Keys.Contains("controller") &&  values["controller"].ToString().ToLower() != "events";
            //var eventExpression = new Regex(@"\W*/events/\W*[^/]", RegexOptions.IgnoreCase);
            //return !eventExpression.Match(httpContext.Request.Path.ToLower()).Success;
        }
    }

    public class EventRoute : RouteBase
    {
        private IList<Route> _routes = new List<Route>();

        public EventRoute(string resource)
        {
            // Generate all your routes here
            _routes.Add(
                new Route("some/url/{param1}",
                          new RouteValueDictionary(new
                                                       {
                                                           controller = resource,
                                                           action = "index"
                                                       }), new MvcRouteHandler()
                    ));
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return _routes.Select(route => route.GetRouteData(httpContext)).FirstOrDefault(data => data != null);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            throw new NotImplementedException();
        }
    }
}