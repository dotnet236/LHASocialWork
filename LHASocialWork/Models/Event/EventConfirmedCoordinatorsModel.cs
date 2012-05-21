using LHASocialWork.Core.MVCContrib;

namespace LHASocialWork.Models.Event
{
    public class EventConfirmedCoordinatorsModel : GridModel<EventCoordinatorModel>
    {
        public bool UserIsAdmin { get; set; }
    }
}