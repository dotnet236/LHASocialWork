using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AutoMapper;
using LHASocialWork.Core.Enumerations;
using LHASocialWork.Core.Extensions;
using LHASocialWork.Core.Filters;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Event;
using LHASocialWork.Models.Shared;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Event;
using MvcContrib.Pagination;

namespace LHASocialWork.Controllers
{
    public class EventsController : BaseController
    {
        private readonly IEventService _eventService;
        private Event _event;

        public EventsController(BaseServiceCollection baseServiceCollection, IEventService eventService)
            : base(baseServiceCollection)
        {
            _eventService = eventService;
        }

        #region Index

        public ActionResult Index(GridOptionsModel options)
        {
            options.Column = options.Column == "Id" ? "Name" : options.Column;

            var searchCriteria = new EventsSearchCriteria
                                     {
                                         OnlyCurrent = false,
                                         OrderByProperty = options.Column,
                                         Ascending = options.Ascending,
                                         Filters = options.SearchOptions.Filters
                                     };

            var events = _eventService.FindEvents(searchCriteria);
            var searchResults = new EventSearchResultModel
                                                    {
                                                        Data = Mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events).AsPagination(options.Page, 4),
                                                        Options = options
                                                    };
            if (Request != null && Request.IsAjaxRequest())
                return PartialView("SearchResults", searchResults);

            return View(new EventIndexModel { SearchResults = searchResults });
        }

        #endregion

        #region Classes

        public ActionResult Classes(GridOptionsModel options)
        {
            var classFilter = new SearchFilter<Event> { Conditional = SearchConditional.Equals, PropertyName = "EventType", PropertyValue = EventType.Class };
            var filtersList = options.SearchOptions.Filters.ToList();
            filtersList.Add(classFilter);
            options.Column = options.Column == "Id" ? "Name" : options.Column;
            options.SearchOptions.Filters = filtersList;

            var searchCriteria = new EventsSearchCriteria
                                     {
                                         OnlyCurrent = false,
                                         OrderByProperty = options.Column,
                                         Ascending = options.Ascending,
                                         Filters = options.SearchOptions.Filters
                                     };

            var events = _eventService.FindEvents(searchCriteria);
            var eventsModel = new List<EventModel>();
            var usersMemberEvents = User == null ? new List<EventMember>() : User.MemberEvents;
            var usersCoodinatorEvents = User == null ? new List<EventCoordinator>() : User.CoordinatorEvents;
            foreach (var evnt in events)
            {
                var eventModel = Mapper.Map<Event, EventModel>(evnt);
                var evnt1 = evnt;
                var eventMember = usersMemberEvents.FirstOrDefault(x => x.Event == evnt1);
                var eventCoordinator = usersCoodinatorEvents.FirstOrDefault(x => x.Event == evnt1);
                eventModel.MemberStatus = eventMember != null ? eventMember.Status : EventMemberStatus.Null;
                eventModel.CoordinatorStatus = eventCoordinator != null ? eventCoordinator.Status : EventMemberStatus.Null;
                eventsModel.Add(eventModel);
            }

            var searchResults = new EventSearchResultModel
                                                    {
                                                        Data = eventsModel.AsPagination(options.Page, 4),
                                                        Options = options,
                                                        EventSearchCategory = "Classes"
                                                    };
            if (Request != null && Request.IsAjaxRequest())
                return PartialView("SearchResults", searchResults);

            return View(new EventClassesModel { SearchResults = searchResults });
        }

        #endregion

        #region Programs

        public ActionResult Programs(GridOptionsModel options)
        {
            var classFilter = new SearchFilter<Event> { Conditional = SearchConditional.Equals, PropertyName = "EventType", PropertyValue = EventType.Program };
            var filtersList = options.SearchOptions.Filters.ToList();
            filtersList.Add(classFilter);
            options.Column = options.Column == "Id" ? "Name" : options.Column;
            options.SearchOptions.Filters = filtersList;

            var searchCriteria = new EventsSearchCriteria
                                     {
                                         OnlyCurrent = false,
                                         OrderByProperty = options.Column,
                                         Ascending = options.Ascending,
                                         Filters = options.SearchOptions.Filters
                                     };

            var events = _eventService.FindEvents(searchCriteria);
            var searchResults = new EventSearchResultModel
                                                    {
                                                        Data = Mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events).AsPagination(options.Page, 4),
                                                        Options = options,
                                                        EventSearchCategory = "Programs"
                                                    };
            if (Request != null && Request.IsAjaxRequest())
                return PartialView("SearchResults", searchResults);

            return View(new EventProgramsModel { SearchResults = searchResults });
        }

        #endregion

        #region View

        [RequiresEvent]
        public ActionResult Event()
        {
            var model = Mapper.Map<Event, EventModel>(_event);
            var usersMemberEvents = User == null ? new List<EventMember>() : User.MemberEvents;
            var usersCoodinatorEvents = User == null ? new List<EventCoordinator>() : User.CoordinatorEvents;
            var evnt1 = _event;
            var eventMember = usersMemberEvents.FirstOrDefault(x => x.Event == evnt1);
            var eventCoordinator = usersCoodinatorEvents.FirstOrDefault(x => x.Event == evnt1);
            model.MemberStatus = eventMember != null ? eventMember.Status : EventMemberStatus.Null;
            model.CoordinatorStatus = eventCoordinator != null ? eventCoordinator.Status : EventMemberStatus.Null;
            model.Flyer.ImageSize = ImageSizes.W180xL200;
            return View(model);
        }

        #endregion


        #region Members

        [RequiresEvent]
        public ActionResult Members()
        {
            var userIsAdmin = User == null ? false : User.IsAdmin;
            var model = new EventMembersModel
                            {
                                Event = Mapper.Map<Event, EventModel>(_event),
                                UserIsAdmin = userIsAdmin
                            };
            return View(model);
        }

        [RequiresEvent]
        public ActionResult AttendingMembers(GridOptionsModel options)
        {
            return View(new EventAttendingMembersModel
                       {
                           Data = Mapper.Map<IEnumerable<EventMember>, IEnumerable<EventMemberModel>>(_event.AttendingMembers).AsPagination(options.Page, 4),
                           Options = options,
                           UserIsAdmin = User == null ? false : User.IsAdmin
                       });
        }

        [RequiresEvent, RequiresRole(SystemRole.Admin, SystemRole.Volunteer, SystemRole.Staff)]
        public ActionResult RequestedMembers(GridOptionsModel options)
        {
            return View(new EventRequestedMembersModel
                       {
                           Data = Mapper.Map<IEnumerable<EventMember>, IEnumerable<EventMemberModel>>(_event.RequestedMembers).AsPagination(options.Page, 4),
                           Options = options
                       });
        }

        [RequiresEvent, RequiresRole(SystemRole.Admin, SystemRole.Volunteer, SystemRole.Staff)]
        public ActionResult InvitedMembers(GridOptionsModel options)
        {
            return View(new EventInvitedMembersModel
                       {
                           Data = Mapper.Map<IEnumerable<EventMember>, IEnumerable<EventMemberModel>>(_event.InvitedMembers).AsPagination(options.Page, 4),
                           Options = options
                       });
        }
        #endregion

        #region Join

        [RequiresEvent, RequiresAuthentication("Please login or register before joining this event.")]
        public ActionResult Join()
        {
            var eventMember = User.MemberEvents.FirstOrDefault(x => x.Event == _event) ??
                              new EventMember { Event = _event, User = User };

            eventMember.Status = EventMemberStatus.Requested;
            eventMember.LastStatusChange = DateTime.Now;
            eventMember = _eventService.SaveEventMember(eventMember);
            if (!eventMember.IsValid)
                DisplayAlert("Your request has been sent to the event leader.");
            else
                AddValidationResults(eventMember.InvalidValues);

            return RedirectToAction("event", new { eventName = _event.Name });
        }

        #endregion

        #region Cancel Join Request

        [RequiresAuthentication, RequiresEvent]
        public ActionResult CancelJoinRequest()
        {
            var eventMember = User.MemberEvents.FirstOrDefault(x => x.Event == _event);
            if (eventMember == null)
                DisplayError("You have not requested to join this event.");
            else
            {
                eventMember.Status = EventMemberStatus.RequestCanceled;
                eventMember = _eventService.SaveEventMember(eventMember);
                if (eventMember.InvalidValues.Any())
                    AddValidationResults(eventMember.InvalidValues);
                else
                    DisplayMessage("Your request has been canceled.");
            }
            return RedirectToAction("Classes");
        }

        #endregion

        #region Approve Member

        [HttpPost, ValidateAntiForgeryToken, RequiresRole(SystemRole.Staff, SystemRole.Admin), RequiresEvent]
        public ActionResult ApproveMember(long id)
        {
            var eventMember = _eventService.GetEventMemberById(id);
            if (eventMember == null)
                DisplayError("Event member not found.");
            else
            {
                eventMember.Status = EventMemberStatus.Confirmed;
                eventMember.LastStatusChange = DateTime.Now;
                eventMember = _eventService.SaveEventMember(eventMember);
                if (eventMember.InvalidValues.Any())
                {
                    DisplayError("Member approval failed.");
                    AddValidationResults(eventMember.InvalidValues);
                }
                else
                    DisplayMessage("Member approved.");
            }

            return RedirectToAction("Members", new { eventName = _event.Name });
        }

        #endregion

        #region Remove Member

        [HttpPost, ValidateAntiForgeryToken, RequiresRole(SystemRole.Staff, SystemRole.Admin), RequiresEvent]
        public ActionResult RemoveMember(int id)
        {
            var eventMember = _eventService.GetEventMemberById(id);
            if (eventMember == null)
                DisplayError("Event member not found.");
            else
            {
                eventMember.Status = EventMemberStatus.Remove;
                eventMember.LastStatusChange = DateTime.Now;
                eventMember = _eventService.SaveEventMember(eventMember);
                if (eventMember.InvalidValues.Any())
                {
                    DisplayError("Member removal failed.");
                    AddValidationResults(eventMember.InvalidValues);
                }
                else
                    DisplayMessage("Member Removed.");
            }

            return RedirectToAction("Members", new { eventName = _event.Name });
        }

        #endregion

        #region Deny Member

        [HttpPost, ValidateAntiForgeryToken, RequiresRole(SystemRole.Staff, SystemRole.Admin), RequiresEvent]
        public ActionResult DenyMember(long id)
        {
            var eventMember = _eventService.GetEventMemberById(id);
            eventMember.Status = EventMemberStatus.Denied;
            eventMember.LastStatusChange = DateTime.Now;
            eventMember = _eventService.SaveEventMember(eventMember);
            if (eventMember.InvalidValues.Any())
            {
                DisplayError("Member denial failed.");
                AddValidationResults(eventMember.InvalidValues);
            }
            else
                DisplayMessage("Member Denied.");

            return RedirectToAction("Members", new { eventName = _event.Name });
        }

        #endregion

        #region Invite Members

        [RequiresEvent, RequiresEventPosition(EventPosition.Member)]
        public ActionResult InviteMembers()
        {
            var isCoordinator = User.IsAdmin || User.IsStaff || User.CoordinatorEvents.Any(x => x.Event == _event && x.Status.IsCoordinator());
            var model = new InviteEventMembersModel
                            {
                                EventId = _event.Id,
                                IsCoordinator = isCoordinator,
                                FacebookEnabled = false
                            };
            return View(model);
        }

        [RequiresEvent, RequiresEventPosition(EventPosition.Coordinator)]
        public ActionResult InviteExistingUsers(GridOptionsModel options)
        {
            var userSearchCriteria = new UsersSearchCriteria()
                                         {
                                             DistinctRootEntity = true,
                                             OrderByProperty = "FirstName",
                                             WithoutId = _event.AllMembers.Where(x => x.User != null && (x.Status.IsMember() || x.Status == EventMemberStatus.Invited))
                                                                            .Select(x => x.User.Id).ToArray()
                                         };
            var users = AccountService.FindUsers(userSearchCriteria);
            var model = new ThumbnailsDisplayModel
                            {
                                Data =
                                    Mapper.Map<IEnumerable<User>, IEnumerable<ThumbnailDisplayModel>>(users).ToList().
                                    AsPagination(options.Page, 12),
                                Options = options
                            };
            return View("ThumbnailsDisplay", model);
        }

        [HttpPost, ValidateAntiForgeryToken, RequiresEvent, RequiresEventPosition(EventPosition.Member)]
        public ActionResult InviteMembers(List<string> invites)
        {
            if (invites.Select(ParseInvitedEventMember).Where(eventMember => eventMember != null)
                       .Select(_eventService.SaveEventMember).Where(eventMember => eventMember.InvalidValues.Any()).Any())
                DisplayError("One or more invitations were not sent successfully.");
            else
                DisplayMessage("All invitations sent successfully.");
            return RedirectToAction("Members", new { eventName = _event.Name });
        }

        private EventMember ParseInvitedEventMember(string identifier)
        {
            string emailAddress = null;
            long facebookId = 0;
            long existingUserId;
            if (!Int64.TryParse(identifier, out existingUserId))
            {
                if (identifier.StartsWith("fb_"))
                {
                    if (!Int64.TryParse(identifier, out facebookId))
                    {
                        AddValidationResult(facebookId + " is not a valid facebook ID.", null);
                        return null;
                    }
                }
                else if (!IsEmail(identifier))
                {
                    AddValidationResult(identifier + " is not a valid email address.", null);
                    return null;
                }
                else
                    emailAddress = identifier;
            }

            long? nullableFacebokId = null;
            if (facebookId != 0) nullableFacebokId = facebookId;

            var user = existingUserId == 0 ? null : AccountService.GetUserById(existingUserId);
            EventMember eventMember;
            if (user != null && user.MemberEvents.Any(x => x.Event == _event))
            {
                eventMember = user.MemberEvents.FirstOrDefault(x => x.Event == _event);
                eventMember.Status = EventMemberStatus.Invited;
                eventMember.LastStatusChange = DateTime.Now;
            }
            else
                eventMember = new EventMember
                                       {
                                           EmailAddress = emailAddress,
                                           Event = _event,
                                           FacebookUser = nullableFacebokId,
                                           LastStatusChange = DateTime.Now,
                                           Status = EventMemberStatus.Invited,
                                           User = user
                                       };

            return eventMember;
        }

        #endregion

        #region Accept Member Invitation 

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AcceptMemberInvitation()
        {
            var eventMember = User.MemberEvents.First(x => x.Event == _event && x.Status == EventMemberStatus.Invited);
            if (eventMember == null)
                DisplayError("No invitation found.");
            else
            {
                eventMember.Status = EventMemberStatus.Confirmed;
                eventMember = _eventService.SaveEventMember(eventMember);
                if (!eventMember.IsValid)
                    DisplayError("Event membership invitation was not accepted.");
                else
                    DisplayMessage("Welcome to " + _event.Name + "!");
            }
            return RedirectToAction("Event", new { eventName = _event.Name });
        }

        #endregion


        #region Coordinators

        public ActionResult Coordinators()
        {
            var userIsAdmin = User == null ? false : User.IsAdmin;
            var model = new EventCoordinatorsModel
                            {
                                Event = Mapper.Map<Event, EventModel>(_event),
                                UserIsAdmin = userIsAdmin
                            };
            return View(model);
        }

        [RequiresEvent]
        public ActionResult ConfirmedCoordinators(GridOptionsModel options)
        {
            return View(new EventConfirmedCoordinatorsModel
                       {
                           Data = Mapper.Map<IEnumerable<EventCoordinator>, IEnumerable<EventCoordinatorModel>>(_event.ConfirmedCoordinators).AsPagination(options.Page, 4),
                           Options = options,
                           UserIsAdmin = User == null ? false : User.IsAdmin
                       });
        }

        [RequiresEvent, RequiresRole(SystemRole.Admin, SystemRole.Staff)]
        public ActionResult RequestedCoordinators(GridOptionsModel options)
        {
            return View(new EventRequestedCoordinatorsModel
                       {
                           Data = Mapper.Map<IEnumerable<EventCoordinator>, IEnumerable<EventCoordinatorModel>>(_event.RequestedCoordinators).AsPagination(options.Page, 4),
                           Options = options
                       });
        }

        #endregion

        #region Volunteer

        [RequiresEvent, RequiresAuthentication("Please login or register before volunteering for this event.")]
        public ActionResult Volunteer(DateTime startDate, DateTime endDate)
        {
            var eventCoordinator = User.CoordinatorEvents.FirstOrDefault(x => x.Event == _event) ??
                              new EventCoordinator { Event = _event, Coordinator = User };

            eventCoordinator.Status = EventMemberStatus.Requested;
            eventCoordinator.LastStatusChange = DateTime.Now;
            eventCoordinator.StartDate = startDate;
            eventCoordinator.EndDate = endDate;
            eventCoordinator = _eventService.SaveCoordinator(eventCoordinator);
            if (eventCoordinator.IsValid)
                DisplayAlert("Your request has been sent to the event leader.");
            else
                AddValidationResults(eventCoordinator.InvalidValues);

            return RedirectToAction("event", new { eventName = _event.Name });
        }

        #endregion

        #region Cancel Volunteer Request

        [RequiresAuthentication, RequiresEvent]
        public ActionResult CancelVolunteerRequest()
        {
            var eventCoordinator = User.CoordinatorEvents.FirstOrDefault(x => x.Event == _event);
            if (eventCoordinator == null)
                DisplayError("You have not requested to volunteer for this event.");
            else
            {
                eventCoordinator.Status = EventMemberStatus.RequestCanceled;
                eventCoordinator = _eventService.SaveCoordinator(eventCoordinator);
                if (eventCoordinator.InvalidValues.Any())
                    AddValidationResults(eventCoordinator.InvalidValues);
            }
            DisplayMessage("Your request has been canceled.");
            return RedirectToAction("Classes");
        }

        #endregion

        #region Approve Coordinator

        [HttpPost, ValidateAntiForgeryToken, RequiresRole(SystemRole.Staff, SystemRole.Admin), RequiresEvent]
        public ActionResult ApproveCoordinator(long id)
        {
            var eventCoordinator = _eventService.GetEventCoordinatorById(id);
            if (eventCoordinator == null)
                DisplayError("Event coordinator not found.");
            else
            {
                eventCoordinator.Status = EventMemberStatus.Confirmed;
                eventCoordinator.LastStatusChange = DateTime.Now;
                eventCoordinator = _eventService.SaveCoordinator(eventCoordinator);
                if (eventCoordinator.InvalidValues.Any())
                {
                    DisplayError("Coordinator approval failed.");
                    AddValidationResults(eventCoordinator.InvalidValues);
                }
                else
                    DisplayMessage("Coordinator approved.");
            }

            return RedirectToAction("Coordinators", new { eventName = _event.Name });
        }

        #endregion

        #region Deny Coordinator

        [HttpPost, ValidateAntiForgeryToken, RequiresRole(SystemRole.Staff, SystemRole.Admin), RequiresEvent]
        public ActionResult DenyCoordinator(long id)
        {
            var eventCoordinator = _eventService.GetEventCoordinatorById(id);
            if (eventCoordinator == null)
                DisplayError("Event coordinator not found.");
            else
            {
                eventCoordinator.Status = EventMemberStatus.Denied;
                eventCoordinator.LastStatusChange = DateTime.Now;
                eventCoordinator = _eventService.SaveCoordinator(eventCoordinator);
                if (eventCoordinator.InvalidValues.Any())
                {
                    DisplayError("Coordinator denial failed.");
                    AddValidationResults(eventCoordinator.InvalidValues);
                }
                else
                    DisplayMessage("Coordinator denied.");
            }

            return RedirectToAction("Coordinators", new { eventName = _event.Name });
        }

        #endregion


        #region MVCEvent Handlers

        [NonAction]
        public void TriggerActionExecuting()
        {
            OnActionExecuting(new ActionExecutingContext { RouteData = ControllerContext.RouteData }); //TODO: Create method for mocking OnActionExecuting
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var eventName = filterContext.RouteData.Values["eventName"] as string;
            if (!string.IsNullOrEmpty(eventName))
                _event = _eventService.GetEventByName(eventName.Replace(" ", "_"));
            base.OnActionExecuting(filterContext);
        }

        #endregion

    }
}

