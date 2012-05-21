using System;
using System.Collections.Generic;
using System.Linq;
using LHASocialWork.Entities;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories.Criteria
{
    public class EventsSearchCriteria : EntitySearchCriteria
    {
        public bool OnlyCurrent { get; set; }

        public IEnumerable<SearchFilter<Event>> Filters { get; set; }

        public new DetachedCriteria BuildCriteria()
        {
            Criteria = DetachedCriteria.For<Event>();

            if (OnlyCurrent)
                Criteria.Add(Restrictions.Ge("EndDate", DateTime.Now));
            if (Filters != null && Filters.Count() > 0)
                Criteria.Add(Filters.JoinWithAnd());

            return base.BuildCriteria();
        }
    }
}
