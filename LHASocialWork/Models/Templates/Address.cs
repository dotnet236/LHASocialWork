using System.ComponentModel.DataAnnotations;

namespace LHASocialWork.Models.Templates
{
    public class Address 
    {
        [Required, Display(Name="Street"), StringLength(255)]
        public string Street { get; set; }
        [Required, Display(Name="City"), StringLength(255)]
        public string City { get; set; }
        [Display(Name="Province"), StringLength(255)]
        public string Province { get; set; }
        [Display(Name="State"), StringLength(255)]
        public string State { get; set; }
        [Required, Display(Name="Country"), StringLength(255)]
        public string Country { get; set; }
        [Required, Display(Name="Zip"), StringLength(255)]
        public string Zip { get; set; }

        public static Address DefaultAddress
        {
            get
            {
                return new Address
                           {
                               Street = "Lha office Temple Road, McLeod Ganj",
                               City = "Dharamsala",
                               Country = "India",
                               Province = "Himachal Pradesh",
                               Zip = "176219"
                           };
            }
        }
    }
}