using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace LHASocialWork.Tests.Controllers
{
    public class MockUser : IPrincipal
    {
        public IIdentity Identity { get; set; }
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}
