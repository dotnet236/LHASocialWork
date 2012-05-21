using System.Web;

namespace LHASocialWork.Tests.Controllers
{
    public class MockHttpResponse : HttpResponseBase
    {
        private static HttpCookieCollection _cookieCollection;
        public MockHttpResponse()
        {
            if(_cookieCollection  == null)
                _cookieCollection = new HttpCookieCollection();

        }

        public override HttpCookieCollection Cookies
        {
            get { return _cookieCollection; }
        }

        public override void SetCookie(HttpCookie cookie)
        {
            _cookieCollection.Add(cookie);
        }
    }
}