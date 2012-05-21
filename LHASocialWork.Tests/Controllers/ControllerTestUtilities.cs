using System.Web.Mvc;

namespace LHASocialWork.Tests.Controllers
{
    public static class ControllerTestUtilities
    {
        public static bool IsCorrectRoute(this RedirectToRouteResult result, string action, string controller = null, string area = null)
        {
            var validAction = result.RouteValues["action"].ToString().ToLower() == action.ToLower();
            var validController = controller == null || result.RouteValues["controller"].ToString().ToLower() == controller.ToLower();
            var validArea = area == null || result.RouteValues["area"].ToString().ToLower() == area.ToLower();
            return validAction && validController && validArea;
        }
    }
}