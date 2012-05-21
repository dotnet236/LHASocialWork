using System.ComponentModel.DataAnnotations;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;

namespace LHASocialWork.Areas.Admin.Models.Positions
{
    public class CreatePositionResponseModel : CreatePositionModel
    {
        [Required]
        public SystemRole SystemRole { get; set; }
    }
}