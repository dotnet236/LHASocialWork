using LHASocialWork.Models.Templates;

namespace LHASocialWork.Models.Event
{
    public class InviteEventUserModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DisplayImage ProfilePicture { get; set; }
    }
}