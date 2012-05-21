using LHASocialWork.Core.MVCContrib;

namespace LHASocialWork.Models.Event
{
    public class EventAttendingMembersModel : GridModel<EventMemberModel>
    {
        public bool UserIsAdmin { get; set; }
    }
}