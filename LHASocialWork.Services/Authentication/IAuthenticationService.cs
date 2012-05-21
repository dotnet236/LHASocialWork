namespace LHASocialWork.Services.Authentication
{
    public interface IAuthenticationService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string emailAddress, string password);
        bool ChangePassword(string emailAddress, string oldPassword, string newPassword);
    }
}