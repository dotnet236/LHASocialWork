using System;
using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using LHASocialWork.Entities.Enumerations;
using NHibernate.Validator.Constraints;
using NHibernate.Validator.Engine;

namespace LHASocialWork.Entities
{
    public class EventCoordinator : ValidationRequiredEntity
    {
        [NotNull]
        public virtual Event Event { get; set; }
        [NotNull]
        public virtual User Coordinator { get; set; }
        [NotNull]
        public virtual EventMemberStatus Status { get; set; }
        [NotNull]
        public virtual DateTime LastStatusChange { get; set; }
        [NotNull]
        public virtual DateTime StartDate { get; set; }
        [NotNull]
        public virtual DateTime EndDate { get; set; }

        public override bool IsValid
        {
            get
            {
                if(StartDate > EndDate)
                {
                    AddInvalidValue(new InvalidValue("End date must be after start date", typeof(EventCoordinator), "StartDate", StartDate, this, null));
                    return true;
                }
                return base.IsValid;
            }
        }
    }

    public class EventCoordinatorMap : ClassMap<EventCoordinator>
    {
        public EventCoordinatorMap()
        {
            Table("EventCoordinators");

            Id(x => x.Id);

            Map(x => x.Status);
            Map(x => x.LastStatusChange);
            Map(m => m.StartDate);
            Map(m => m.EndDate);

            References(x => x.Coordinator).Column("CoordinatorId").ForeignKey("UserId");
            References(x => x.Event).Column("EventId").ForeignKey("EventId");
        }
    }
}
