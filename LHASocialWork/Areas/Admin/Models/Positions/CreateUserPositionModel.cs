using LHASocialWork.Core.DataAnnotations;

namespace LHASocialWork.Areas.Admin.Models.Positions
{
    public class CreateUserPositionModel
    {
        [RequiredComplex]
        public long PositionId { get; set; }
        [RequiredComplex(ErrorMessage="You must select at least one user.")]
        public long[] UserIds { get; set; }
    }
}