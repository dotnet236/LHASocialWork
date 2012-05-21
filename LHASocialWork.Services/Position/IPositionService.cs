using System.Collections.Generic;
using LHASocialWork.Entities;
using LHASocialWork.Repositories.Criteria;

namespace LHASocialWork.Services.Position
{
    public interface IPositionService
    {
        Entities.Position SavePosition(Entities.Position position);
        IEnumerable<Entities.Position> GetPositions();
        Entities.Position GetPosition(long positionId);
        IEnumerable<Entities.Position> FindPositions(PositionsSearchCriteria searchCriteria);
        UserPosition GetUserPosition(long userPositionId);
        IEnumerable<UserPosition> FindUserPositions(UserPositionsSearchCriteria searchCriteria);
        IEnumerable<UserPosition> SaveUserPositions(IEnumerable<UserPosition> userPositions);
        UserPosition SaveUserPosition(UserPosition userPosition);
        UserPosition DeleteUserPosition(UserPosition userPosition);
        Entities.Position MemberPosition { get; }
    }
}
