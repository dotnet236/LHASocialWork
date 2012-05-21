using LHASocialWork.Services.Account;
using LHASocialWork.Services.Authentication;
using LHASocialWork.Services.Image;

namespace LHASocialWork.Controllers
{
    public class BaseServiceCollection
    {
        public IAccountService AccountService { get; private set; }
        public IImageService ImageService { get; private set; }
        public IAuthenticationService AuthenticationService { get; private set;}

        public BaseServiceCollection(IAccountService accountService, IImageService imageService, IAuthenticationService authenticationService)
        {
            AccountService = accountService;
            ImageService = imageService;
            AuthenticationService = authenticationService;
        }
    }
}