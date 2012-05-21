using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using LHASocialWork.Areas.Admin.Models.Events;
using LHASocialWork.Controllers;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Event;
using LHASocialWork.Models.Shared;
using LHASocialWork.Models.Templates;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;
using Address = LHASocialWork.Models.Templates.Address;
using EventsController = LHASocialWork.Areas.Admin.Controllers.EventsController;

namespace LHASocialWork.Tests.Controllers
{
    [TestClass]
    public class AdminEventsControllerTest : BaseControllerTest
    {
        private LHASocialWork.Areas.Admin.Controllers.EventsController _eventsController;

        #region Index

        #region Setup

        private void SetupGetEvents()
        {

            var service = new Mock<IEventService>();
            _eventsController = new EventsController(null, service.Object);
        }

        #endregion

        [TestMethod]
        public void GetEvents()
        {
            SetupGetEvents();
            var gridModel = ObjectFactory.GetInstance<GridOptionsModel>();
            gridModel.Page = 1;
            var result = _eventsController.Index(gridModel);
            ValidateViewResult<EventsModel, EventsController>(result, true, "", "admin");
        }

        #endregion

        #region View Event

        #region HttpGet

        #region Setup

        public void SetupViewEvent()
        {
            var service = new Mock<IEventService>();
            service.Setup(x => x.GetEventById(It.IsAny<long>())).Returns(new Event { Flyer = new Image { FileKey = Guid.NewGuid(), Title = "" } });
            _eventsController = new EventsController(null, service.Object);
        }

        #endregion

        [TestMethod]
        public void ViewEvent()
        {
            SetupViewEvent();
            var result = _eventsController.Event(1);
            ValidateViewResult<EventModel, EventsController>(result, true, "Event", "admin", new { id = 1 });
        }

        #endregion

        #endregion

        #region Create Event

        private readonly CreateEventResponseModel _invalidCreateEventResponseModel = new CreateEventResponseModel { };

        private static CreateEventResponseModel ValidCreateEventResponseModel
        {
            get
            {
                var postedFile = new Mock<IHttpPostedFile>();
                postedFile.Setup(x => x.ContentLength).Returns(6000);
                postedFile.Setup(x => x.InputStream).Returns(new MemoryStream());
                postedFile.Setup(x => x.FileName).Returns("Test File");

                return new CreateEventResponseModel
                           {
                               DateTime = new ComplexDateTime
                                              {
                                                  StartDate =
                                                      DateTime.Now,
                                                  StartTime =
                                                      DateTime.Now,
                                                  EndDate =
                                                      DateTime.Now,
                                                  EndTime =
                                                      DateTime.Now
                                              },
                               Name = "Test",
                               Description = "Description",
                               Occurrence = EventOccurrence.Everyday,
                               Privacy = PrivacySetting.MembersOnly,
                               Flyer = postedFile.Object,
                               Address = new Address
                                             {
                                                 City = "Test",
                                                 Country = "Test",
                                                 Street = "Test",
                                                 Zip = "Test"

                                             }
                           };
            }
        }

        #region HttpGet

        #region Setup

        private void SetupGetCreateEvent()
        {
            var service = new Mock<IAccountService>();
            service.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns(new User { FirstName = "Test", LastName = "Test" });
            var httpContext = new MockHttpContextBase { User = new MockUser { Identity = new MockIdentity("", "", false) } };
            var controllerContext = new ControllerContext(httpContext, new RouteData(), new BaseController(new BaseServiceCollection(service.Object, null, null)));
            _eventsController = new EventsController(new BaseServiceCollection(service.Object, null, null), null) { ControllerContext = controllerContext };
        }

        #endregion

        [TestMethod]
        public void GetCreateEvent()
        {
            SetupGetCreateEvent();
            var result = _eventsController.Create();
            ValidateViewResult<CreateEventViewModel, EventsController>(result, true, "Create", "admin");
        }


        #endregion

        #region HttpPost

        #region Setup

        private void SetupCreateEventFailure()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns(new User { FirstName = "Test", LastName = "Test" });
            var httpContext = new MockHttpContextBase { User = new MockUser { Identity = new MockIdentity("", "", false) } };
            var controllerContext = new ControllerContext(httpContext, new RouteData(), new BaseController(new BaseServiceCollection(accountService.Object, null, null)));
            _eventsController = new EventsController(new BaseServiceCollection(accountService.Object, null, null), null) { ControllerContext = controllerContext };
        }

        private void SetupCreateEventSuccess()
        {
            var eventService = new Mock<IEventService>();
            eventService.Setup(x => x.SaveEvent(It.IsAny<Event>())).Returns(new Event());
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns(new User { FirstName = "Test", LastName = "Test" });
            var httpContext = new MockHttpContextBase { User = new MockUser { Identity = new MockIdentity("", "", false) } };
            var controllerContext = new ControllerContext(httpContext, new RouteData(), new BaseController(new BaseServiceCollection(accountService.Object, null, null)));
            Event.FlyerSizes = new List<ImageSizes>();
            _eventsController = new EventsController(new BaseServiceCollection(accountService.Object, null, null), eventService.Object) { ControllerContext = controllerContext };
        }

        #endregion

        [TestMethod]
        public void PostCreateEventFailure()
        {
            SetupCreateEventFailure();
            var result = _eventsController.CallWithModelValidation(x => x.Create(_invalidCreateEventResponseModel), _invalidCreateEventResponseModel);
            ValidateModelStateOfUnsuccessfulPostResponse(result);
        }

        [TestMethod]
        public void PostCreateEventSuccess()
        {
            SetupCreateEventSuccess();
            var result = _eventsController.CallWithModelValidation(x => x.Create(ValidCreateEventResponseModel), ValidCreateEventResponseModel);
            ValidateRouteOfRedirectResponse(result, "Index");
        }

        #endregion

        #endregion

        #region Edit Event

        private EditEventResponseModel _invalidEditEventResponseModel = new EditEventResponseModel
                                                                  {
                                                                      Description = "",
                                                                      FileChanged = true,
                                                                      Flyer = null,
                                                                      Id = 7
                                                                  };
        private static EditEventResponseModel ValidEditEventResponseModel
        {
            get
            {
                var postedFile = new Mock<IHttpPostedFile>();
                postedFile.Setup(x => x.ContentLength).Returns(6000);
                postedFile.Setup(x => x.InputStream).Returns(new MemoryStream());
                postedFile.Setup(x => x.FileName).Returns("Test File");

                return new EditEventResponseModel
                           {
                               DateTime = new ComplexDateTime
                                              {
                                                  StartDate =
                                                      DateTime.Now,
                                                  StartTime =
                                                      DateTime.Now,
                                                  EndDate =
                                                      DateTime.Now,
                                                  EndTime =
                                                      DateTime.Now
                                              },
                               Name = "Test",
                               Description = "Description",
                               Occurrence = EventOccurrence.Everyday,
                               Privacy = PrivacySetting.MembersOnly,
                               Flyer = postedFile.Object,
                               Address = new Address
                                             {
                                                 City = "Test",
                                                 Country = "Test",
                                                 Street = "Test",
                                                 Zip = "Test"

                                             }
                           };
            }
        }

        #region HttpGet

        #region Setup

        private void SetupGetEditEvent()
        {
            var eventService = new Mock<IEventService>();
            eventService.Setup(x => x.GetEventById(It.IsAny<long>())).Returns(new Event { Flyer = new Image() });
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns(new User { FirstName = "Test", LastName = "Test" });
            var httpContext = new MockHttpContextBase { User = new MockUser { Identity = new MockIdentity("", "", false) } };
            var baseServices = new BaseServiceCollection(accountService.Object, null, null);
            _eventsController = new EventsController(baseServices, eventService.Object)
                                    {
                                        ControllerContext = new ControllerContext(httpContext, new RouteData(), new BaseController(baseServices))
                                    };
        }

        #endregion

        [TestMethod]
        public void GetEditEvent()
        {
            SetupGetEditEvent();
            var result = _eventsController.Edit(7);
            ValidateViewResult<EditEventViewModel, EventsController>(result, true, "Edit", "admin", new { id = 7 });
        }


        #endregion

        #region HttpPost

        #region Setup

        private void SetupPostEditEventFailure()
        {
            Event.FlyerSizes = new List<ImageSizes>();
            var service = new Mock<IEventService>();
            service.Setup(x => x.SaveEvent(It.IsAny<Event>())).Returns(new Event() { Flyer = new Image() });
            service.Setup(x => x.GetEventById(It.IsAny<long>())).Returns(new Event() {Flyer = new Image()});
            _eventsController = new EventsController(null, service.Object);
        }

        private void SetupPostEditEventSuccess()
        {
            var service = new Mock<IEventService>();
            service.Setup(x => x.SaveEvent(It.IsAny<Event>())).Returns(new Event());
            _eventsController = new EventsController(null, service.Object);
        }
        
        #endregion

        [TestMethod]
        public void PostEditEventFailure()
        {
            SetupPostEditEventFailure();
            var response = _eventsController.Edit(_invalidEditEventResponseModel);
            ValidateModelStateOfUnsuccessfulPostResponse(response);
        }

        [TestMethod]
        public void PostEditEventSuccess()
        {
            SetupPostEditEventSuccess();
            var response = _eventsController.Edit(ValidEditEventResponseModel);
            ValidateRouteOfRedirectResponse(response, "Event");
        }

        #endregion

        #endregion

    }
}