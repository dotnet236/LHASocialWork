using LHASocialWork.Entities;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories.Criteria
{
    public class UsersSearchCriteria : EntitySearchCriteria
    {
        public string NameIsLike { get; set; }
        public string[] WithEmailAddresses { get; set; }

        public new DetachedCriteria BuildCriteria()
        {
            Criteria = DetachedCriteria.For<User>();

            if (NameIsLike != null)
                Criteria.Add(Restrictions.Or(Restrictions.Like("FirstName", NameIsLike, MatchMode.Anywhere),
                                             Restrictions.InsensitiveLike("LastName", NameIsLike, MatchMode.Anywhere)));
            if(WithEmailAddresses != null)
                Criteria.Add(Restrictions.In("Email", WithEmailAddresses));

            return base.BuildCriteria();
        }
    }
}
