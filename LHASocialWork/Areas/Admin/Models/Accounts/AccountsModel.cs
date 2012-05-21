using System.ComponentModel;
using LHASocialWork.Core.MVCContrib;

namespace LHASocialWork.Areas.Admin.Models.Accounts
{
    public class AccountsModel : GridModel<AccountModel> { }

    public class AccountModel
    {
        [DisplayName("Email Address")]
        public virtual string Email { get; set; }
        [DisplayName("First Name")]
        public virtual string FirstName { get; set; }
        [DisplayName("Last Name")]
        public virtual string LastName { get; set; }
        [DisplayName("Phone Number")]
        public virtual long PhoneNumber { get; set; }
    }
}