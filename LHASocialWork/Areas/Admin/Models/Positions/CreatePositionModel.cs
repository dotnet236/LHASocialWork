using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LHASocialWork.Areas.Admin.Models.Positions
{
    public class CreatePositionModel
    {
        [Required, DisplayName("Position Name"), StringLength(255)]
        public string Name { get; set; }
    }
}