using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using NHibernate.Validator.Constraints;

namespace LHASocialWork.Entities
{
    public class UserPosition : ValidationRequiredEntity
    {
        [NotNull]
        public virtual User User { get; set; }
        [NotNull]
        public virtual Position Position { get; set; }
    }

    public sealed class UserPositionMap : ClassMap<UserPosition>
    {
        public UserPositionMap()
        {
            Table("UserPositions");
            Id(x => x.Id);
            References(x => x.User).Column("UserId").ForeignKey("UserId");
            References(x => x.Position).Column("PositionId").ForeignKey("PositionId");
        }
    }
}
