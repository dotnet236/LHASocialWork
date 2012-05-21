using System.Collections.Generic;
using LHASocialWork.Core;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Core;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;

namespace LHASocialWork.Tests.Services
{
    [TestClass]
    public class EventServiceTest : BaseServiceTest
    {
        private IEventService _eventService;

        #region Find events

        #region Setup

        private void SetupFindEvents()
        {
            MockBaseRepository.Setup(x => x.List<Event>(It.IsAny<DetachedCriteria>())).Returns(new List<Event>());
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        #endregion

        [TestMethod]
        public void FindEvents()
        {
            SetupFindEvents();
            var events = _eventService.FindEvents(new EventsSearchCriteria());
            Assert.IsNotNull(events, "Events list should not be null.");
        }

        #endregion

        #region Get Event Member By ID

        #region Setup

        private void SetupGetEventMemberById()
        {
            MockBaseRepository.Setup(x => x.Get<EventMember>(It.IsAny<long>())).Returns(new EventMember());
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        #endregion

        [TestMethod]
        public void GetEventMemberById()
        {
            SetupGetEventMemberById();
            var evnt = _eventService.GetEventMemberById(9);
            Assert.IsNotNull(evnt, "Event should not be null");
        }

        #endregion

        #region Get Event Coordinator By ID

        #region Setup

        private void SetupGetEventCoordinatorById()
        {
            MockBaseRepository.Setup(x => x.Get<EventCoordinator>(It.IsAny<long>())).Returns(new EventCoordinator());
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        #endregion

        [TestMethod]
        public void GeteEventCoordinatorById()
        {
            SetupGetEventCoordinatorById();
            var coordinator = _eventService.GetEventCoordinatorById(7);
            Assert.IsNotNull(coordinator, "Event coordinator should not be null");
        }

        #endregion

        #region Get Event By ID

        #region Setup

        public void SetupGetEventById()
        {
            MockBaseRepository.Setup(x => x.Get<Event>(It.IsAny<long>())).Returns(new Event());
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        #endregion

        [TestMethod]
        public void GetEventById()
        {
            SetupGetEventById();
            Assert.IsNotNull(_eventService.GetEventById(2), "Event returned should  be null.");
        }

        #endregion

        #region Get Event By Name

        #region Setup

        private void SetupGetEventByName()
        {
            MockBaseRepository.Setup(x => x.List<Event>(It.IsAny<DetachedCriteria>())).Returns(new List<Event> { new Event() });
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        #endregion

        [TestMethod]
        public void GetEventByName()
        {
            SetupGetEventByName();
            var evt = _eventService.GetEventByName(SystemEntitiesStore.SystemEventTemplate.Name);
            Assert.IsNotNull(evt, "Event should not be null");
        }

        #endregion

        #region Create Event

        #region Setup

        private void SetupCreateEventSuccess()
        {
            MockBaseRepository.Setup(x => x.SaveOrUpdate(It.IsAny<ValidationRequiredEntity>(), null)).Returns<Event>(x => new Event());
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        private void SetupCreateEventFailure()
        {
            _eventService = new EventService(null, null);
        }

        #endregion

        [TestMethod]
        public void CreateEventSuccess()
        {
            SetupCreateEventSuccess();
            var validEvent = SystemEntitiesStore.SystemEventTemplate;
            validEvent.Flyer = new Image();
            validEvent.Owner = new User();
            validEvent.Location = new Address();
            validEvent = _eventService.SaveEvent(validEvent);
            ValidateSuccessfulEntityCreation(validEvent);
        }

        [TestMethod]
        public void CreateEventFailure()
        {
            SetupCreateEventFailure();
            var invalidEvent = SystemEntitiesStore.SystemEventTemplate;
            invalidEvent.Name = "";
            invalidEvent.Flyer = new Image();
            invalidEvent = _eventService.SaveEvent(invalidEvent);
            ValidateUnsuccessfulEntityCreation(invalidEvent);
        }

        #endregion

        #region Save Event Member

        #region Setup

        private void SetupSaveEventMemberSuccess()
        {
            MockBaseRepository.Setup(x => x.SaveOrUpdate(It.IsAny<ValidationRequiredEntity>(), null)).Returns<EventMember>(x => new EventMember());
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        private void SetupSaveEventMemberFailure()
        {
            _eventService = new EventService(null, null);
        }

        #endregion

        [TestMethod]
        public void SaveEventMemberSuccess()
        {
            SetupSaveEventMemberSuccess();
            var validEventMember = new EventMember { User = new User(), Event = new Event(), Status = Entities.Enumerations.EventMemberStatus.Requested };
            ValidateSuccessfulEntityCreation(_eventService.SaveEventMember(validEventMember));
        }

        [TestMethod]
        public void SaveEventMemberFailure()
        {
            SetupSaveEventMemberFailure();
            var invalidEventMember = new EventMember {User = null, Event = null};
            ValidateUnsuccessfulEntityCreation(_eventService.SaveEventMember(invalidEventMember));
        }

        #endregion

        #region Save Event Coordinator

        #region Setup

        private void SetupSaveCoordinatorSuccess()
        {
            MockBaseRepository.Setup(x => x.SaveOrUpdate(It.IsAny<EventCoordinator>(), null)).Returns<EventCoordinator>(x => new EventCoordinator());
            _eventService = new EventService(MockBaseRepository.Object, null);
        }

        private void SetupSaveCoordinatorFailure()
        {
            _eventService = new EventService(null, null);
        }

        #endregion

        [TestMethod]
        public void SaveCoordinatorSuccess()
        {
            SetupSaveCoordinatorSuccess();
            var validCoordinator = new EventCoordinator { Coordinator = new User(), Event = new Event(), Status = Entities.Enumerations.EventMemberStatus.Requested };
            ValidateSuccessfulEntityCreation(_eventService.SaveCoordinator(validCoordinator));
        }

        [TestMethod]
        public void SaveCoordinatorFailure()
        {
            SetupSaveCoordinatorFailure();
            var invalidCoordinator = new EventCoordinator { Coordinator = null, Event = null};
            ValidateUnsuccessfulEntityCreation(_eventService.SaveCoordinator(invalidCoordinator));
        }

        #endregion
    }
}
