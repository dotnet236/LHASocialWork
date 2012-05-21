namespace LHASocialWork.Models.Account
{
    public class AccountLogInModel
    {
        public string EmailAddress { get; set;}
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}