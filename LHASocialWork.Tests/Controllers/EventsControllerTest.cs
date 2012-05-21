using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using LHASocialWork.Controllers;
using LHASocialWork.Core;
using LHASocialWork.Entities;
using LHASocialWork.Models.Event;
using LHASocialWork.Models.Shared;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LHASocialWork.Tests.Controllers
{
    [TestClass]
    public class EventsControllerTest : BaseControllerTest
    {
        private EventsController _eventsController;

        private Mock<IEventService> EventService
        {
            get
            {
                var service = new Mock<IEventService>();
                service.Setup(x => x.GetEventByName(It.IsAny<string>())).Returns(new Event { Name = "TEstName", Flyer = new Image(), AttendingMembers = new List<EventMember>() });
                return service;
            }
        }

        #region Index

        #region HttpGet

        #region Setup

        public void SetupGetIndex()
        {
            var service = new Mock<IEventService>();
            service.Setup(x => x.FindEvents(It.IsAny<EventsSearchCriteria>())).Returns(new List<Event>());
            _eventsController = new EventsController(null, service.Object);
        }

        #endregion

        [TestMethod]
        public void GetIndex()
        {
            SetupGetIndex();
            var result = _eventsController.Index(new GridOptionsModel());
            ValidateViewResult<EventIndexModel, EventsController>(result, true, "Index");
        }

        #endregion

        #endregion

        #region View

        #region HttpGet

        #region Setup

        private void SetupGetViewEvent()
        {
            _eventsController = new EventsController(null, EventService.Object) { ControllerContext = GetControllerContext(false) };
            _eventsController.AddEventToRoute();
        }

        #endregion

        [TestMethod]
        public void GetViewEvent()
        {
            SetupGetViewEvent();
            var result = _eventsController.Event();
            ValidateViewResult<EventModel, EventsController>(result, true, "Event", null, new { eventName = SystemEntitiesStore.SystemEventTemplate.Id });
        }

        #endregion

        #endregion

        #region View Classes

        #region HttpGet

        #region Setup

        private void SetupGetViewClasses()
        {
            SetupGetIndex();
        }

        #endregion

        [TestMethod]
        public void GetViewClasses()
        {
            SetupGetViewClasses();
            var result = _eventsController.Classes(new GridOptionsModel());
            ValidateViewResult<EventClassesModel, EventsController>(result, true, "Classes");
        }

        #endregion

        #endregion

        #region View Programs

        #region HttpGet

        #region Setup

        private void SetupGetViewPrograms()
        {
            SetupGetIndex();
        }

        #endregion

        [TestMethod]
        public void GetViewPrograms()
        {
            SetupGetViewPrograms();
            var result = _eventsController.Programs(new GridOptionsModel());
            ValidateViewResult<EventProgramsModel, EventsController>(result, true, "Classes");
        }

        #endregion

        #endregion


        #region Join Event

        #region HttpGet

        #region Setup

        private void SetupGetJoinEventForAnonymousUser()
        {
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(false) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        private void SetupGetJoinEventForAuthenticatedUser()
        {
            EventService.Setup(x => x.SaveEventMember(It.IsAny<EventMember>())).Returns(new EventMember());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod, Ignore]
        public void JoinEventForNonAuthenticatedUser()
        {
            SetupGetJoinEventForAnonymousUser();
            var result = _eventsController.Join();
            ValidateRouteOfRedirectResponse(result, "login", "accounts");
        }

        //TODO : Mock eventmembers list for user
        [TestMethod, Ignore]
        public void JoinEventForAuthenticatedUser()
        {
            SetupGetJoinEventForAuthenticatedUser();
            var result = _eventsController.Join();
            ValidateRouteOfRedirectResponse(result, "event");
        }

        #endregion

        #endregion

        #region Cancel Join Request

        #region HttpGet

        #region Setup

        private void SetupCancelJoinRequest()
        {
            EventService.Setup(x => x.SaveEventMember(It.IsAny<EventMember>())).Returns(new EventMember());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void CancelJoinRequest()
        {
            SetupCancelJoinRequest();
            var result = _eventsController.CancelJoinRequest();
            ValidateRouteOfRedirectResponse(result, "classes");
        }

        #endregion

        #endregion

        #region View Event Members

        #region HttpGet

        #region Setup

        private void SetupGetEventMembersAsAdmin()
        {
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        private void SetupGetEventMembersAsNonAdmin()
        {
            _eventsController = new EventsController(GetBaseServiceCollection(false), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void GetEventMembersAsAdmin()
        {
            SetupGetEventMembersAsAdmin();
            var result = _eventsController.Members() as ViewResult;
            ValidateViewResult<EventMembersModel, EventsController>(result, true, "members");
            var model = result.ViewData.Model as EventMembersModel;
            Assert.IsTrue(model.UserIsAdmin, "UserIsAdmin flag should be true");
        }

        [TestMethod]
        public void GetEventMembersAsNonAdmin()
        {
            SetupGetEventMembersAsNonAdmin();
            var result = _eventsController.Members() as ViewResult;
            ValidateViewResult<EventMembersModel, EventsController>(result, true, "members");
            var model = result.ViewData.Model as EventMembersModel;
            Assert.IsFalse(model.UserIsAdmin, "UserIsAdmin flag should be false");
        }

        #endregion

        #endregion

        #region Invite New Members

        #region HttpGet

        #region Setup

        private void SetupGetInviteMembers()
        {
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
        }

        #endregion

        [TestMethod]
        public void GetInviteMembers()
        {
            SetupGetInviteMembers();
            var result = _eventsController.InviteMembers();
            ValidateViewResult<ThumbnailsDisplayModel, EventsController>(result, true, "InviteMembers");
        }

        #endregion

        #region HttpPost

        #region Setup

        private void SetupPostInviteMembers()
        {
            EventService.Setup(x => x.SaveEventMember(It.IsAny<EventMember>())).Returns(new EventMember());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object);
            _eventsController.AddEventToRoute();
        }

        #endregion

        public void PostInviteMembers()
        {
            SetupPostInviteMembers();
            var result = _eventsController.InviteMembers(new List<string> { "Test@test.com" });
            ValidateRouteOfRedirectResponse(result, "members");
        }

        #endregion

        #endregion

        #region Approve Join Request

        #region HttpPost

        #region Setup

        private void SetupPostApproveMember()
        {
            EventService.Setup(x => x.GetEventMemberById(It.IsAny<long>())).Returns(new EventMember());
            EventService.Setup(x => x.SaveEventMember(It.IsAny<EventMember>())).Returns(new EventMember());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void PostApproveMember()
        {
            SetupPostApproveMember();
            var result = _eventsController.ApproveMember(5);
            ValidateRouteOfRedirectResponse(result, "Members");
        }

        #endregion

        #endregion

        #region Deny Join Request

        #region HttpPost

        #region Setup

        private void SetupPostDenyMember()
        {
            EventService.Setup(x => x.GetEventMemberById(It.IsAny<long>())).Returns(new EventMember());
            EventService.Setup(x => x.SaveEventMember(It.IsAny<EventMember>())).Returns(new EventMember());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void PostDenyMember()
        {
            SetupPostDenyMember();
            var result = _eventsController.DenyMember(5);
            ValidateRouteOfRedirectResponse(result, "Members");
        }

        #endregion

        #endregion

        #region Accept Join Invitation

        #region Setup

        private void SetupPostAcceptMemberInvitation()
        {
            EventService.Setup(x => x.GetEventMemberById(It.IsAny<long>())).Returns(new EventMember());
            EventService.Setup(x => x.SaveEventMember(It.IsAny<EventMember>())).Returns(new EventMember());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
        }

        #endregion

        [TestMethod]
        public void PostAcceptMemberInvitation()
        {
            SetupPostAcceptMemberInvitation();
            var result = _eventsController.AcceptMemberInvitation();
            ValidateRouteOfRedirectResponse(result, "event");
        }

        #endregion

        #region Remove Member

        #region HttpPost

        #region Setup

        private void SetupPostRemoveMember()
        {
            EventService.Setup(x => x.GetEventMemberById(It.IsAny<long>())).Returns(new EventMember());
            EventService.Setup(x => x.SaveEventMember(It.IsAny<EventMember>())).Returns(new EventMember());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void PostRemoveMember()
        {
            SetupPostRemoveMember();
            var result = _eventsController.RemoveMember(4);
            ValidateRouteOfRedirectResponse(result, "members");
        }

        #endregion

        #endregion


        #region View Event Coordinators

        #region HttpGet

        #region Setup

        private void SetupGetEventCoordinatorsAsAdmin()
        {
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        private void SetupGetEventCoordinatorsAsNonAdmin()
        {
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void GetEventCoordinatorsAsAdmin()
        {
            SetupGetEventCoordinatorsAsAdmin();
            var result = _eventsController.Coordinators() as ViewResult;
            ValidateViewResult<EventCoordinatorsModel, EventsController>(result, true, "coordinators");
            var model = result.ViewData.Model as EventCoordinatorsModel;
            Assert.IsTrue(model.UserIsAdmin, "UserIsAdmin flag should be true");
        }

        [TestMethod]
        public void GetEventCoordinatorsAsNonAdmin()
        {
            SetupGetEventCoordinatorsAsNonAdmin();
            var result = _eventsController.Coordinators() as ViewResult;
            ValidateViewResult<EventCoordinatorsModel, EventsController>(result, true, "coordinators");
            var model = result.ViewData.Model as EventCoordinatorsModel;
            Assert.IsFalse(model.UserIsAdmin, "UserIsAdmin flag should be false");
        }

        #endregion

        #endregion

        #region Volunteer For Event

        #region HttpGet

        #region Setup

        private void SetupGetVolunteerEventForAnonymousUser()
        {
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(false) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        private void SetupGetVolunteerEventForAuthenticatedUser()
        {
            EventService.Setup(x => x.SaveCoordinator(It.IsAny<EventCoordinator>())).Returns(new EventCoordinator());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod, Ignore]
        public void GetVolunteerEventForNonAuthenticatedUser()
        {
            SetupGetVolunteerEventForAnonymousUser();
            var result = _eventsController.Volunteer(DateTime.Now, DateTime.Now);
            ValidateRouteOfRedirectResponse(result, "login", "accounts");
        }

        [TestMethod, Ignore]
        public void GetVolunteerEventForAuthenticatedUser()
        {
            SetupGetVolunteerEventForAuthenticatedUser();
            var result = _eventsController.Volunteer(DateTime.Now, DateTime.Now);
            ValidateRouteOfRedirectResponse(result, "event");
        }

        #endregion

        #endregion

        #region Cancel Volunteer Request

        #region HttpGet

        #region Setup

        private void SetupGetCancelVolunteerRequest()
        {
            EventService.Setup(x => x.SaveCoordinator(It.IsAny<EventCoordinator>())).Returns(new EventCoordinator());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void CancelVolunteerRequest()
        {
            SetupGetCancelVolunteerRequest();
            var result = _eventsController.CancelJoinRequest();
            ValidateRouteOfRedirectResponse(result, "classes");
        }

        #endregion

        #endregion

        #region Approve Volunteer Request

        #region HttpPost

        #region Setup

        private void SetupPostApproveCoordinator()
        {
            EventService.Setup(x => x.GetEventCoordinatorById(It.IsAny<long>())).Returns(new EventCoordinator());
            EventService.Setup(x => x.SaveCoordinator(It.IsAny<EventCoordinator>())).Returns(new EventCoordinator());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void PostApproveCoordinator()
        {
            SetupPostApproveCoordinator();
            var result = _eventsController.ApproveCoordinator(5);
            ValidateRouteOfRedirectResponse(result, "Members");
        }

        #endregion

        #endregion

        #region Deny Volunteer Request

        #region HttpPost

        #region Setup

        private void SetupPostDenyCoordinator()
        {
            EventService.Setup(x => x.GetEventCoordinatorById(It.IsAny<long>())).Returns(new EventCoordinator());
            EventService.Setup(x => x.SaveCoordinator(It.IsAny<EventCoordinator>())).Returns(new EventCoordinator());
            _eventsController = new EventsController(GetBaseServiceCollection(), EventService.Object) { ControllerContext = GetControllerContext(true) };
            _eventsController.AddEventToRoute();
            _eventsController.TriggerActionExecuting();
        }

        #endregion

        [TestMethod]
        public void PostDenyCoordinator()
        {
            SetupPostDenyCoordinator();
            var result = _eventsController.DenyCoordinator(5);
            ValidateRouteOfRedirectResponse(result, "Members");
        }

        #endregion

        #endregion

    }

    public static class EventControllerExtension
    {
        public static void AddEventToRoute(this EventsController controller, bool triggerActionExecuting = true)
        {
            controller.RouteData.Values.Add("eventName", SystemEntitiesStore.SystemEventTemplate.Name);
            controller.TriggerActionExecuting();
        }
    }
}
