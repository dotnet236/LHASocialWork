namespace LHASocialWork.Models.Event
{
    public class InviteEventMembersModel
    {
        public long EventId { get; set; }
        public bool FacebookEnabled { get; set; }
        public bool IsCoordinator { get; set; }
    }
}