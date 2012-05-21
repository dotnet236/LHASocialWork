using MvcContrib.UI.Grid;

namespace LHASocialWork.Models.Shared
{
    public class GridOptionsModel : GridSortOptions
    {
        public GridOptionsModel()
        {
            SearchOptions = new SearchOptionsModel();
            Page = 1;
        }

        public int Page { get; set; }
// ReSharper disable InconsistentNaming
        public long? id { get; set; }
// ReSharper restore InconsistentNaming
        public bool Ascending { get { return Direction == MvcContrib.Sorting.SortDirection.Ascending; } }

        public SearchOptionsModel SearchOptions { get; set; }
    }
}