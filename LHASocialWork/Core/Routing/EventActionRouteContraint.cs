using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace LHASocialWork.Core.Routing
{
    public class EventActionRouteContraint : IRouteConstraint
    {
        private static IList<string> _eventActions;
        public static IList<string> EventActions
        {
            get {
                return _eventActions ?? (_eventActions = typeof (Controllers.EventsController).GetMethods().Select(x => x.Name.ToLower()).ToList());
            }
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var action = values["action"] as string;
            return action == null ? false : EventActions.Contains(action.ToLower());
        }
    }
}