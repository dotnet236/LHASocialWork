using System.Web.Configuration;
using System.Web.Security;
using LHASocialWork.Core;

namespace LHASocialWork.Tests
{
    public class BaseTest
    {
        public static bool Initialized;
        public static string DomainName = "localhost";

        public BaseTest()
        {
            if (Initialized) return;

            Bootstrapper.Initialize();
            Initialized = true;
        }

        protected static string HashString(string unhashedString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(unhashedString, FormsAuthPasswordFormat.SHA1.ToString());
        }
    }
}