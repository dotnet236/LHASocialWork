using LHASocialWork.Core.DataAnnotations;
using LHASocialWork.Models.Account;
using LHASocialWork.Models.Templates;

namespace LHASocialWork.Areas.Admin.Models.Events
{
    public class CreateEventModel
    {
        [RequiredComplex]
        public string Name { get; set; }
        [RequiredComplex]
        public ComplexDateTime DateTime { get; set; }
        [RequiredComplex]
        public Address Address { get; set; }
    }
}