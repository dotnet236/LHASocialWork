using System.Collections.Generic;
using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using LHASocialWork.Entities.Enumerations;
using NHibernate.Validator.Constraints;

namespace LHASocialWork.Entities
{
    public class Position : ValidationRequiredEntity
    {
        public Position()
        {
            Users = new List<UserPosition>();
        }

        [NotNull]
        public virtual bool SystemPosition { get; set;}
        [NotNull]
        public virtual SystemRole SystemRole { get; set; }
        [NotNullNotEmpty, Length(255)]
        public virtual string Name { get; set; }
        public virtual IList<UserPosition> Users { get; set; }
    }

    public sealed class PositionMap : ClassMap<Position>
    {
        public PositionMap()
        {
            Table("Positions");
            Id(m => m.Id);
            Map(m => m.Name);
            Map(m => m.SystemRole);
            Map(m => m.SystemPosition);
            HasMany(m => m.Users).KeyColumn("PositionId");
        }
    }
}
