using System.ComponentModel;
using LHASocialWork.Core.MVCContrib;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;

namespace LHASocialWork.Areas.Admin.Models.Positions
{
    public class PositionsModel : GridModel<PositionModel> { }

    public class PositionModel
    {
        public int Id { get; set; }
        [DisplayName("Position Name")]
        public string Name { get; set; }
        [DisplayName("System Role")]
        public virtual SystemRole SystemRole { get; set; }
        [DisplayName("Accounts")]
        public int AccountCount { get; set; }
    }
}