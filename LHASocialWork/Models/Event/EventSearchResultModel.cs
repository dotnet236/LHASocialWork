using LHASocialWork.Core.MVCContrib;

namespace LHASocialWork.Models.Event
{
    public class EventSearchResultModel : GridModel<EventModel>
    {
        public EventSearchResultModel()
        {
            EventSearchCategory = "Events";
        }

        public string EventSearchCategory { get; set; }
    }
}