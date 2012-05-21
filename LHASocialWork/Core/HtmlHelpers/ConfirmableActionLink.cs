using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace LHASocialWork.Core.HtmlHelpers
{
    public static class ConfirmableActionLinkHtmlHelper
    {
        public static string ConfirmableActionLink(this HtmlHelper helper, string label, string message, string action)
        {
            return ConfirmableActionLink(helper, label, message, action, new object());
        }

        public static string ConfirmableActionLink(this HtmlHelper helper, string label, string message, string action, object routeValues)
        {
            return ConfirmableActionLink(helper, label, message, action, routeValues, null, FormMethod.Post);
        }

        public static string ConfirmableActionLink(this HtmlHelper helper, string label, string message, string action, object routeValues, string controller, FormMethod formMethod)
        {
            var uniqueId = Guid.NewGuid().ToString().Replace("-", "");

            var scriptTag = new TagBuilder("script");
            scriptTag.Attributes.Add("type", "text/javascript");
            scriptTag.InnerHtml = "$(function(){ $('#" + uniqueId + "').confirmableActionLink('" + message.Replace("'", "\'") + "'); });";

            var submitButton = new TagBuilder("input");
            submitButton.AddCssClass("confirmableActionLink");
            submitButton.Attributes.Add("id", uniqueId);
            submitButton.Attributes.Add("type", "submit");
            submitButton.Attributes.Add("value", label);

            helper.ViewContext.Writer.Write(scriptTag.ToString());
            using (helper.BeginForm(action, controller, routeValues, formMethod))
            {
                helper.ViewContext.Writer.Write(helper.AntiForgeryToken());
                helper.ViewContext.Writer.Write(submitButton.ToString());
            }
            return "";
        }
    }
}