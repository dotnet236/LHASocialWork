using System.ComponentModel.DataAnnotations;
using LHASocialWork.Validation.Attributes;

namespace LHASocialWork.Models.Templates
{
    public class SystemInfo : EditorTemplate
    {
        [Required(ErrorMessage="Email is required."), Display(Name="Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage="Password is required."), DataType(DataType.Password), StringLength(255)]
        public string Password { get; set; }
        [Required(ErrorMessage="Must confirm your password."), DataType(DataType.Password), Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}