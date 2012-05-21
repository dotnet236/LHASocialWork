using System;
using LHASocialWork.Entities;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories.Criteria
{
    public class ImageSearchCriteria : EntitySearchCriteria
    {
        public Guid[] WithFileyKeys { get; set; }

        public new DetachedCriteria BuildCriteria()
        {
            Criteria = DetachedCriteria.For<Image>();
            if (WithFileyKeys != null)
                Criteria.Add(Restrictions.In("FileKey", WithFileyKeys));
            return base.BuildCriteria();
        }
    }
}
