using System.Collections.Generic;
using System.Web.Mvc;

namespace LHASocialWork.Core.HtmlHelpers
{
    public static class RadioButtonListForExtension
    {
        //TODO: Make RadioButtonListFor use Lambda Expression 
        public static MvcHtmlString RadioButtonListFor(this HtmlHelper htmlHelper, IEnumerable<SelectListItem> list, string name = "")
        {
            var radioButtonListContainer = new TagBuilder("DIV");
            radioButtonListContainer.AddCssClass("radioButtonListContainer");
            var first = true;
            foreach (var item in list)
            {
                var radioButtonContainer = new TagBuilder("DIV");
                radioButtonContainer.AddCssClass("radioButtonContainer");
                radioButtonContainer.InnerHtml += GetRadioButton(item, name, first);
                radioButtonListContainer.InnerHtml += radioButtonContainer.ToString();
                first = false;
            }

            return new MvcHtmlString(radioButtonListContainer.ToString());
        }

        private static string GetRadioButton(SelectListItem item, string name, bool selected)
        {
            var radioButton = new TagBuilder("INPUT");
            radioButton.Attributes.Add("type", "radio");
            radioButton.Attributes.Add("name", name);
            radioButton.Attributes.Add("value", item.Value);
            if(selected)
                radioButton.Attributes.Add("checked","checked");
            radioButton.InnerHtml = item.Text;
            return radioButton.ToString();
        }
    }
}
