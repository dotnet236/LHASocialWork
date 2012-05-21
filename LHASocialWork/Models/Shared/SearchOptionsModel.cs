using System.Collections.Generic;
using LHASocialWork.Repositories.Criteria;

namespace LHASocialWork.Models.Shared
{
    public class SearchOptionsModel
    {
        public SearchOptionsModel()
        {
            Filters = new List<SearchFilter<Entities.Event>>();
        }

        public IEnumerable<SearchFilter<Entities.Event>> Filters { get; set; }
    }
}