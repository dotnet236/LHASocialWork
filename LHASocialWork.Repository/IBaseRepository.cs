using System.Collections.Generic;
using FluentNHibernate.Data;
using NHibernate;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories
{
    public interface IBaseRepository
    {
        T SaveOrUpdate<T>(T entity, ITransaction trans = null) where T : Entity;
        IList<T> SaveOrUpdate<T>(IList<T> entities, ITransaction trans = null) where T : Entity;
        T Get<T>(int id) where T : Entity;
        T Get<T>(DetachedCriteria usersSearchCriteria) where T : Entity;
        IEnumerable<T> List<T>(DetachedCriteria detachedCriteria) where T : Entity;
        T Get<T>(long positionId) where T : Entity;
        void Delete(Entity entity, ITransaction trans = null);

    }
}
