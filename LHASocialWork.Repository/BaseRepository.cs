using System.Collections.Generic;
using FluentNHibernate.Data;
using NHibernate;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly ISession _session;

        public BaseRepository(ISession session)
        {
            _session = session;
        }

        public virtual T SaveOrUpdate<T>(T entity, ITransaction trans) where T : Entity
        {
            var boolDispose = trans == null;
            if (boolDispose) trans = _session.BeginTransaction();

            _session.SaveOrUpdate(entity);
            trans.Commit();

            if (boolDispose) trans.Dispose();

            return entity;
        }

        public IList<T> SaveOrUpdate<T>(IList<T> entities, ITransaction trans) where T : Entity
        {
            var newTransaction = trans == null;
            if (newTransaction) 
                trans = _session.BeginTransaction();

            foreach (var entity in entities)
                _session.SaveOrUpdate(entity);

            if (newTransaction)
            {
                trans.Commit();
                trans.Dispose();
            }

            return entities;
        }

        public virtual T Get<T>(int id) where T : Entity
        {
            return _session.Get<T>(id);
        }

        public T Get<T>(DetachedCriteria userSearchCriteria) where T : Entity
        {
            return userSearchCriteria.GetExecutableCriteria(_session).UniqueResult<T>();
        }

        public virtual T Get<T>(long positionId) where T : Entity
        {
            return _session.Get<T>(positionId);
        }

        public void Delete(Entity entity, ITransaction trans)
        {
            var newTransaction = trans == null;
            if (newTransaction) 
                trans = _session.BeginTransaction();

            _session.Delete(entity);

            if (newTransaction)
            {
                trans.Commit();
                trans.Dispose();
            }
        }

        public virtual IEnumerable<T> List<T>(DetachedCriteria detachedCriteria) where T : Entity
        {
            return detachedCriteria.GetExecutableCriteria(_session).List<T>();
        }
    }
}