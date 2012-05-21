
namespace LHASocialWork.Models.Shared
{
    public class SearchModel
    {
        public SearchModel(string field)
        {
            Field = field;
        }

        public string Field { get; private set; }
// ReSharper disable InconsistentNaming, jQuery syntax
        public string term { get; set; }
// ReSharper restore InconsistentNaming
    }
}