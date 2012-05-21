using System.ComponentModel.DataAnnotations;
using LHASocialWork.Models.Templates;

namespace LHASocialWork.Models.Account
{
    public class CreateAccountModel
    {
        [Required]
        public SystemInfo SystemInfo { get; set; }
        [Required]
        public PersonalInfo PersonInfo { get; set; }
        [Required]
        public Address Address { get; set; }
    }
}