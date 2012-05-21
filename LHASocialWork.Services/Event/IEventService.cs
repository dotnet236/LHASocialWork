using System.Collections.Generic;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Core;
using LHASocialWork.Repositories.Criteria;

namespace LHASocialWork.Services.Event
{
    public interface IEventService
    {
        IEnumerable<Entities.Event> FindEvents(EventsSearchCriteria searchCriteria);
        Entities.Event SaveEvent(Entities.Event @event);
        Entities.Event GetEventById(long eventId);
        Entities.Event GetEventByName(string eventName);

        EventMember SaveEventMember(EventMember validEventMember);
        EventMember GetEventMemberById(long eventMemberId);

        EventCoordinator SaveCoordinator(EventCoordinator validCoordinator);
        EventCoordinator GetEventCoordinatorById(long eventCoordinatorId);
    }
}
