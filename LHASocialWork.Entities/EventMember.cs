using System;
using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using LHASocialWork.Entities.Enumerations;
using NHibernate.Validator.Constraints;

namespace LHASocialWork.Entities
{
    public class EventMember : ValidationRequiredEntity
    {
        [NotNull]
        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual long? FacebookUser { get; set; }
        public virtual EventMemberStatus Status { get; set; }
        public virtual DateTime LastStatusChange { get; set; }

        public override bool IsValid
        {
            get
            {
                var valid = true;
                var attending = Status == EventMemberStatus.Attended || Status == EventMemberStatus.Confirmed;
                if (attending)
                    valid = User != null && EmailAddress == null && FacebookUser == null && LastStatusChange != DateTime.MinValue;
                if(Status == EventMemberStatus.Confirmed)
                    valid = User != null && EmailAddress == null && FacebookUser == null;
                if (Status == EventMemberStatus.Invited)
                    valid = ((User != null && EmailAddress == null && FacebookUser == null) ||
                            (User == null && EmailAddress != null && FacebookUser == null) ||
                            (User == null && EmailAddress == null && FacebookUser != null));
                if (Status == EventMemberStatus.Requested)
                    valid = User != null && EmailAddress == null && FacebookUser == null;
                return valid && base.IsValid;
            }
        }
    }

    public sealed class EventMemberMap : ClassMap<EventMember>
    {
        public EventMemberMap()
        {
            Table("EventMembers");
            Id(m => m.Id);
            Map(x => x.EmailAddress);
            Map(x => x.FacebookUser);
            Map(x => x.Status);
            Map(x => x.LastStatusChange);
            References(x => x.User).Column("UserId").ForeignKey("UserId");
            References(x => x.Event).Column("EventId").ForeignKey("EventId");
        }
    }
}
