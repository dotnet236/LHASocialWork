using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace LHASocialWork.Areas.Admin.Models.Positions
{
    public class CreatePositionViewModel : CreatePositionModel
    {
        [Required, DisplayName("System Roles")]
        public IEnumerable<SelectListItem> SystemRoles { get; set; }
    }
}