using System.ComponentModel.DataAnnotations;

namespace LHASocialWork.Models.Templates
{
    public class PersonalInfo
    {
        [Required, Display(Name="First Name"), StringLength(255)]
        public string FirstName { get; set; }
        [Required, Display(Name="Last Name"), StringLength(255)]
        public string LastName { get; set; }
        [Required, Display(Name="Phone Number")]
        public long? PhoneNumber { get; set; }
    }
}