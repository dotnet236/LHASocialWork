using System.Security.Principal;
using System.Web;

namespace LHASocialWork.Tests.Controllers
{
    public class MockHttpContextBase : HttpContextBase
    {
        public override IPrincipal User { get; set;}

        public override System.Web.Caching.Cache Cache
        {
            get
            {
                return new System.Web.Caching.Cache();
            }
        }

        public override HttpRequestBase Request
        {
            get
            {
                return new MockHttpRequest();
            }
        }

        public override HttpResponseBase Response
        {
            get { return new MockHttpResponse(); }
        }
    }

}