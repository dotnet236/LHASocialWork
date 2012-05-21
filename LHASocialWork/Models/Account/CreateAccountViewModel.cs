using LHASocialWork.Models.Templates;

namespace LHASocialWork.Models.Account
{
    public class CreateAccountViewModel : CreateAccountModel
    {
        public CreateAccountViewModel()
        {
            SystemInfo = new SystemInfo();
            PersonInfo = new PersonalInfo();
            Address = new Address();
        }
        public string ReturnUrl { get; set; }
        public string FormPostAction { get; set; }
    }
}