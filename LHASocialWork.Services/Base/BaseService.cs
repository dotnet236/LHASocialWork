using System.Collections.Generic;
using System.Linq;
using LHASocialWork.Entities.Core;
using LHASocialWork.Repositories;
using NHibernate.Validator.Engine;

namespace LHASocialWork.Services.Base
{
    public class BaseService : IBaseService
    {
        protected readonly IBaseRepository Repository;
        private readonly ValidatorEngine _validatorEnginer;

        public BaseService(IBaseRepository repository)
        {
            Repository = repository;
            _validatorEnginer = new ValidatorEngine();
        }

        public T ValidateAndSave<T>(T entity) where T : ValidationRequiredEntity
        {
            entity.AddInvalidValues(_validatorEnginer.Validate(entity));
            if(entity.InvalidValues.Length == 0)
                Repository.SaveOrUpdate(entity, null);
            return entity;
        }

        public IList<T> ValidateAndSave<T>(IList<T> entities) where T : ValidationRequiredEntity
        {
            foreach (var entity in entities)
                entity.AddInvalidValues(_validatorEnginer.Validate(entity));
            if (!entities.Any(x => x.InvalidValues.Any()))
                Repository.SaveOrUpdate(entities, null);
            return entities;
        }
    }
}