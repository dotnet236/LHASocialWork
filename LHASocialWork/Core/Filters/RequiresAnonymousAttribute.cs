using System.Web.Mvc;
using LHASocialWork.Models.Shared;

namespace LHASocialWork.Core.Filters
{
    public class RequiresAnonymousAttribute : ActionFilterAttribute
    {
        private readonly string _displayMessage;

        public RequiresAnonymousAttribute(string displayMessage = "")
        {
            _displayMessage = displayMessage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated && !Bootstrapper.TestMode)
            {
                var urlHelper = new UrlHelper(filterContext.RequestContext);
                var redirectUrl = "";
                if (filterContext.HttpContext.Request.Url != null)
                {
                    var redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;
                    redirectUrl = string.Format("?returnUrl={0}", redirectOnSuccess);
                }
                var loginUrl = urlHelper.Action("Index", "Home", new { area = "" }) + redirectUrl;
                filterContext.Controller.TempData["Message"] = new MessageModel { Message = _displayMessage };
                filterContext.Result = new RedirectResult(loginUrl);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}