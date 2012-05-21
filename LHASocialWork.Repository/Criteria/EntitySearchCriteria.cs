using System;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace LHASocialWork.Repositories.Criteria
{
    public class EntitySearchCriteria
    {
        public string OrderByProperty { get; set; }
        public bool Ascending { get; set; }
        public bool DistinctRootEntity { get; set; }
        public long[] WithId { get; set; }
        public long[] WithoutId { get; set; }
        protected DetachedCriteria Criteria { get; set; }
        protected DetachedCriteria BuildCriteria()
        {
            if (Criteria == null)
                throw new Exception("Criteria must be defined by inherited class. Verify base 'Criteria' property was defined in build criteria method of your search criteria.");

            var alias = Criteria.Alias;
            if (WithId != null)
                Criteria.Add(Restrictions.In(string.Format("{0}.{1}",alias,"Id"), WithId));
            if (WithoutId != null)
                Criteria.Add(Restrictions.Not(Restrictions.In(string.Format("{0}.{1}", alias, "Id"), WithoutId)));
            if (DistinctRootEntity)
                Criteria.SetResultTransformer(Transformers.DistinctRootEntity);
            if (!string.IsNullOrEmpty(OrderByProperty))
                Criteria.AddOrder(new Order(string.Format("{0}.{1}",alias, OrderByProperty), Ascending));
            return Criteria;
        }
    }
}