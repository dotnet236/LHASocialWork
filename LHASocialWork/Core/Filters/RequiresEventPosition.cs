using System.Linq;
using System.Web.Mvc;
using LHASocialWork.Controllers;
using LHASocialWork.Core.Enumerations;
using LHASocialWork.Core.Extensions;
using LHASocialWork.Models.Shared;
using LHASocialWork.Services.Event;
using StructureMap;
using EnumExtensions = LHASocialWork.Core.Extensions.EnumExtensions;

namespace LHASocialWork.Core.Filters
{
    public class RequiresEventPosition : ActionFilterAttribute
    {
        private readonly EventPosition _eventPosition;
        private readonly IEventService _eventService;

        public RequiresEventPosition(EventPosition eventPosition)
        {
            _eventPosition = eventPosition;
            _eventService = ObjectFactory.GetInstance<IEventService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var eventName = filterContext.RouteData.Values["eventName"] as string;
            var baseController = filterContext.Controller as BaseController;
            var evnt = _eventService.GetEventByName(eventName == null ? "" : eventName.Replace(" ", "_"));
            if (baseController == null || evnt == null) return;

            var user = baseController.User;
            if (!user.IsAdmin && !user.IsStaff)
            {
                var isMember = user.MemberEvents.Any(x => x.Event == evnt && x.Status.IsMember());
                var isCoordinator = user.CoordinatorEvents.Any(x => x.Event == evnt && x.Status.IsCoordinator());
                var urlHelper = new UrlHelper(filterContext.RequestContext);

                var validPosition = false;
                switch (_eventPosition)
                {
                    case EventPosition.Coordinator:
                        validPosition = isCoordinator;
                        break;
                    case EventPosition.Member:
                        validPosition = isCoordinator || isMember;
                        break;
                }
                if (!validPosition)
                {
                    var redirectUrl = urlHelper.Action("Event", "Events", new { area = "", eventName });
                    filterContext.Controller.TempData["Error"] = new ErrorModel
                                                                     {
                                                                         Message =
                                                                             "Must be member of event to view this page."
                                                                     };
                    filterContext.Result = new RedirectResult(redirectUrl);
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}