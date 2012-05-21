using LHASocialWork.Entities;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories.Criteria
{
    public class UserPositionsSearchCriteria : EntitySearchCriteria
    {
        public long[] Positions { get; set; }
        public new DetachedCriteria BuildCriteria()
        {
            Criteria = DetachedCriteria.For<UserPosition>();
            if (Positions != null && Positions.Length > 0)
                Criteria.Add(Restrictions.In("Position", Positions));
            return base.BuildCriteria();
        }
    }
}
