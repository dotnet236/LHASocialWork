using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Data;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories.Criteria
{
    public static class SearchFilterExtensions
    {
        public static ICriterion JoinWithAnd<T>(this IEnumerable<SearchFilter<T>> filters)  where T : Entity
        {
            if (filters.Count() == 0) return null;
            return filters.Count() == 1 ? 
                    filters.First().GenerateRestriction() :
                    Restrictions.And(filters.First().GenerateRestriction(), filters.Skip(1).JoinWithAnd());
        }
    }
}