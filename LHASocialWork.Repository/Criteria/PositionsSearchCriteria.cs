using System.Collections.Generic;
using System.Linq;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories.Criteria
{
    public class PositionsSearchCriteria : EntitySearchCriteria
    {
        public IList<SystemRole> WithRoles { get; set; }
        public bool OnlySystemPositions { get; set; }

        public new DetachedCriteria BuildCriteria()
        {
            Criteria = DetachedCriteria.For<Position>();
            if (WithRoles != null)
                Criteria.Add(Restrictions.In("SystemRole", WithRoles.ToArray()));
            if (OnlySystemPositions)
                Criteria.Add(Restrictions.Eq("SystemPosition", OnlySystemPositions));
            return base.BuildCriteria();
        }
    }
}
