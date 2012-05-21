using System.Web.Mvc;
using LHASocialWork.Models.Shared;
using LHASocialWork.Services.Event;
using StructureMap;

namespace LHASocialWork.Core.Filters
{
    public class RequiresEventAttribute : ActionFilterAttribute
    {
        private readonly IEventService _eventService;

        public RequiresEventAttribute()
        {
            _eventService = ObjectFactory.GetInstance<IEventService>();

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var eventName = filterContext.RouteData.Values["eventName"] as string;
            if (!string.IsNullOrEmpty(eventName) && _eventService.GetEventByName(eventName.Replace(" ", "_")) != null)
                return;

            var urlHelper = new UrlHelper(filterContext.RequestContext);
            var redirectUrl = urlHelper.Action("Classes", "Events", new { area = "" });
            filterContext.Controller.TempData["Error"] = new ErrorModel { Message = "Event not found." };
                filterContext.Result = new RedirectResult(redirectUrl);
        }
    }
}