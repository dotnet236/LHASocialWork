using LHASocialWork.Core.MVCContrib;

namespace LHASocialWork.Areas.Admin.Models.Positions
{
    public class PositionViewModel : GridModel<UserPositionModel>
    {
        public PositionModel Position { get; set;}
    }

    public class UserPositionModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}