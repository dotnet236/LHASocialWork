using System.Collections.Generic;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Repositories;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Base;
using NHibernate.Criterion;
using NHibernate.Validator.Engine;
using System.Linq;

namespace LHASocialWork.Services.Position
{
    public class PositionService : BaseService, IPositionService
    {
        public PositionService(IBaseRepository baseRepository) : base(baseRepository) { }

        public Entities.Position SavePosition(Entities.Position position)
        {
            return ValidatePositionUniqueness(position).InvalidValues.Any() ? position : ValidateAndSave(position);
        }

        private Entities.Position ValidatePositionUniqueness(Entities.Position position)
        {
            var criteria = DetachedCriteria.For<Entities.Position>().Add(Restrictions.Eq("Name", position.Name));
            if (Repository.List<Entities.Position>(criteria).Any())
                position.AddInvalidValue(new InvalidValue("Email address already exists", typeof(Entities.Position), "Name", position.Name, position, null));
            return position;
        }

        public IEnumerable<Entities.Position> GetPositions()
        {
            var criteria = DetachedCriteria.For<Entities.Position>();
            return Repository.List<Entities.Position>(criteria);
        }

        public Entities.Position GetPosition(long positionId)
        {
            return Repository.Get<Entities.Position>(positionId);
        }

        public IEnumerable<Entities.Position> FindPositions(PositionsSearchCriteria searchCriteria)
        {
            return Repository.List<Entities.Position>(searchCriteria.BuildCriteria());
        }

        public UserPosition GetUserPosition(long userPositionId)
        {
            return Repository.Get<UserPosition>(userPositionId);
        }

        public IEnumerable<UserPosition> FindUserPositions(UserPositionsSearchCriteria searchCriteria)
        {
            return Repository.List<UserPosition>(searchCriteria.BuildCriteria());
        }

        public IEnumerable<UserPosition> SaveUserPositions(IEnumerable<UserPosition> userPositions)
        {
            return ValidateAndSave<UserPosition>(userPositions.ToList());
        }

        public UserPosition DeleteUserPosition(UserPosition userPosition)
        {
            if (userPosition.Position.SystemPosition && userPosition.User.SystemUser)
                userPosition.AddInvalidValue(new InvalidValue("Cannot delete a system user's, system position", typeof(UserPosition), "", null, null, null));
            else
                Repository.Delete(userPosition);
            return userPosition;
        }

        public Entities.Position MemberPosition
        {
            get
            {
                var positionsCriteria = new PositionsSearchCriteria
                                            {
                                                WithRoles = new List<SystemRole> {SystemRole.Member},
                                                OnlySystemPositions = true
                                            };
                return Repository.List<Entities.Position>(positionsCriteria.BuildCriteria()).FirstOrDefault();
            }
        }

        public UserPosition SaveUserPosition(UserPosition userPosition)
        {
            return ValidateAndSave(userPosition);
        }
    }
}