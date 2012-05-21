using System.Collections.Generic;
using System.Web.Mvc;

namespace LHASocialWork.Models.Templates
{
    public class DropDown : EditorTemplate
    {
        public IList<SelectListItem> Items { get; set; }
    }
}