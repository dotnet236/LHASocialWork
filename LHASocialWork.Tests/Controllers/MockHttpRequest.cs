using System;
using System.Net;
using System.Web;
using System.Collections.Specialized;

namespace LHASocialWork.Tests.Controllers
{
    public class MockHttpRequest : HttpRequestBase
    {
        private readonly Uri _url = new Uri("http://mysite.example.com/");

        public override bool IsSecureConnection
        {
            get
            {
                return true;
            }
        }

        public override Uri Url
        {
            get
            {
                return _url;
            }
        }

        public bool AjaxRequest { get; set; }

        public override NameValueCollection Headers
        {
            get
            {
                var collection = new WebHeaderCollection();
                if (AjaxRequest)
                    collection.Add("X-Requested-With", "XMLHttpRequest");
                return collection;
            }
        }

        public bool IsAjaxRequest()
        {
            return AjaxRequest;
        }
    }
}