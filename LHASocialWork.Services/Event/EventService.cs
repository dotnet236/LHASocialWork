using System.Collections.Generic;
using System.Linq;
using LHASocialWork.Entities;
using LHASocialWork.Repositories;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Base;
using LHASocialWork.Services.Image;

namespace LHASocialWork.Services.Event
{
    public class EventService : BaseService, IEventService
    {
        private readonly IImageService _imageService;

        public EventService(IBaseRepository baseRepository, IImageService imageService) : base(baseRepository)
        {
            _imageService = imageService;
        }

        public IEnumerable<Entities.Event> FindEvents(EventsSearchCriteria searchCriteria)
        {
            return Repository.List<Entities.Event>(searchCriteria.BuildCriteria());
        }

        public Entities.Event SaveEvent(Entities.Event @event)
        {
            if (@event.Flyer == null)
                @event.Flyer = _imageService.DefaultEventFlyerImage;
            return ValidateAndSave(@event);
        }

        public Entities.Event GetEventById(long eventId)
        {
            return Repository.Get<Entities.Event>(eventId);
        }

        public Entities.Event GetEventByName(string eventName)
        {
            var eventNameFilter = new SearchFilter<Entities.Event>
                                      {
                                          Conditional = SearchConditional.Equals,
                                          PropertyName = "Name",
                                          PropertyValue = eventName
                                      };

            var eventSearchCriteria = new EventsSearchCriteria
                                          {
                                              Filters = new List<SearchFilter<Entities.Event>> {eventNameFilter}
                                          };
            return Repository.List<Entities.Event>(eventSearchCriteria.BuildCriteria()).FirstOrDefault();
        }

        public EventMember SaveEventMember(EventMember validEventMember)
        {
            return ValidateAndSave(validEventMember);
        }

        public EventMember GetEventMemberById(long eventMemberId)
        {
            return Repository.Get<EventMember>(eventMemberId);
        }

        public EventCoordinator SaveCoordinator(EventCoordinator validCoordinator)
        {
            return ValidateAndSave(validCoordinator);
        }

        public EventCoordinator GetEventCoordinatorById(long eventCoordinatorId)
        {
            return Repository.Get<EventCoordinator>(eventCoordinatorId);
        }
    }
}