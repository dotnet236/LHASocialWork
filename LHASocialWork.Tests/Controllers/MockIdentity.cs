using System.Security.Principal;

namespace LHASocialWork.Tests.Controllers
{
    public class MockIdentity : IIdentity
    {
        public string Name { get; private set; }
        public string AuthenticationType { get; private set; }
        public bool IsAuthenticated { get; private set; }

        public MockIdentity(string name, string authenticationType, bool isAuthenticated)
        {
            Name = name;
            AuthenticationType = authenticationType;
            IsAuthenticated = isAuthenticated;
        }
    }
}