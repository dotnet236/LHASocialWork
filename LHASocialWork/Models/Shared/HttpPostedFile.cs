using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace LHASocialWork.Models.Shared
{
    public interface IHttpPostedFile
    {
        int ContentLength { get; set; }
        string FileName { get; set; }
        string ContentType { get; set; }
        Stream InputStream { get; set; }
    }

    public class HttpPostedFile : IHttpPostedFile
    {
        public int ContentLength { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Stream InputStream { get; set; }
    }
}