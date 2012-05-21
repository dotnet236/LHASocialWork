using System;
using System.Text.RegularExpressions;

namespace LHASocialWork.Core.Extensions
{
    public static class StringExtensions
    {
        private static int _maxWebStringLength = 50;

        public static string ToWebLength(this string str)
        {

            if (str.Length > _maxWebStringLength)
            {
                str = str.Substring(0, _maxWebStringLength);
                var endCharOfLastTag = str.LastIndexOf("</");
                var prevPeriodIndex = str.LastIndexOf('.');
                var prevSpaceIndex = str.LastIndexOf(" ");

                if (endCharOfLastTag > 0)
                    str = str.Substring(0, str.Substring(endCharOfLastTag).IndexOf(">") + 1 + endCharOfLastTag) + "...";
                else if (prevPeriodIndex > 0)
                    str = str.Substring(0, prevPeriodIndex + 1) + "...";
                else if (prevSpaceIndex > 0)
                    str = str.Substring(0, prevSpaceIndex + 1) + "...";
            }
            return str;
        }

        public static string ToWebLength(this string str, int maxLength, bool stripHtml = true)
        {
            if (stripHtml)
                str = Regex.Replace(str, "<(.|\n)*?>", string.Empty);

            _maxWebStringLength = maxLength;
            return ToWebLength(str);
        }
    }
}