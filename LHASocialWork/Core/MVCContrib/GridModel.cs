using LHASocialWork.Models.Shared;
using MvcContrib.Pagination;

namespace LHASocialWork.Core.MVCContrib
{
    public class GridModel<T> where T : class
    {
        public GridModel()
        {
            Options = new GridOptionsModel { Column = "", Direction = MvcContrib.Sorting.SortDirection.Ascending, Page = 0 };
        }
        public IPagination<T> Data { get; set; }
        public GridOptionsModel Options { get; set; }
    }
}