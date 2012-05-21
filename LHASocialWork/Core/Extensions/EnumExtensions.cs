using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LHASocialWork.Entities.Enumerations;

namespace LHASocialWork.Core.Extensions
{
    public static class EnumExtensions
    {
        public static bool HasSubmittedRequest(this EventMemberStatus status)
        {
            return status == EventMemberStatus.Attended ||
                   status == EventMemberStatus.Confirmed ||
                   status == EventMemberStatus.Declined ||
                   status == EventMemberStatus.Denied ||
                   status == EventMemberStatus.Requested;
        }

        public static bool IsMember(this EventMemberStatus status)
        {
            return status == EventMemberStatus.Attended ||
                   status == EventMemberStatus.Confirmed;
        }

        public static bool IsCoordinator(this EventMemberStatus status)
        {
            return status == EventMemberStatus.Confirmed;
        }

        public static IEnumerable<T> GetTypedValues<T>()
        {
            var values = Enum.GetValues(typeof(T));
            return (from object value in values select (T)value).ToList();
        }

        public static IEnumerable<SelectListItem> GetTypedValuesAsSelectList<T>(string selectedValue = "")
        {
            var list = GetTypedValues<T>().Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
            if (list.Any(x => x.Value == selectedValue))
                list.First(x => x.Value == selectedValue).Selected = true;
            else
                list.First().Selected = true;
            return list;
        }

        public static bool Contains<T>(string value)
        {
            value = value.Replace(".", "").ToLower();
            var values = Enum.GetValues(typeof(T));
            return values.Cast<object>().Any(v => v.ToString().ToLower() == value);
        }
    }
}