using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace LHASocialWork.Core.HtmlHelpers
{
    public static class FieldForHtmlHelpers
    {
        public static MvcHtmlString FieldFor<T>(this HtmlHelper<T> helper, Expression<Func<T, string>> model)
        {
            var container = new TagBuilder("div");
            container.AddCssClass("editor-property");

            var label = new TagBuilder("div");
            label.AddCssClass("editor-label");
            label.InnerHtml = helper.LabelFor(model).ToHtmlString();

            var field = new TagBuilder("div");
            field.AddCssClass("editor-field");
            field.InnerHtml = helper.EditorFor(model).ToHtmlString() + helper.ValidationMessageFor(model).ToHtmlString();

            container.InnerHtml = label.ToString() + field;

            return new MvcHtmlString(container.ToString());
        }

        public static MvcHtmlString InlineFieldFor<T>(this HtmlHelper<T> helper, Expression<Func<T, object>> model)
        {
            var container = new TagBuilder("div");
            container.AddCssClass("editor-property-inline");

            var errorMessage = new TagBuilder("div");
            var helperMessage = helper.ValidationMessageFor(model);
            errorMessage.AddCssClass("editor-errormessage-inline");
            errorMessage.InnerHtml = helperMessage == null ? "" : helperMessage.ToHtmlString();

            var label = new TagBuilder("div");
            label.AddCssClass("editor-label-inline");
            label.InnerHtml = helper.LabelFor(model).ToHtmlString();

            var field = new TagBuilder("div");
            field.AddCssClass("editor-field-inline");
            field.InnerHtml = helper.EditorFor(model).ToHtmlString() + errorMessage;

            var clear = new TagBuilder("div");
            clear.Attributes.Add("style", "clear:both;");

            container.InnerHtml = label.ToString() + field + clear;

            return new MvcHtmlString(container.ToString());
        }
    }

}