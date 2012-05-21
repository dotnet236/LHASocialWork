using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using LHASocialWork.Entities.Enumerations;
using NHibernate.Validator.Constraints;

namespace LHASocialWork.Entities
{
    public class Event : ValidationRequiredEntity
    {
        [NotNull, NotEmpty, Length(255)]
        public virtual string Name { get; set; }
        public virtual string DisplayName{ get { return Name.Replace("_", " "); }}
        [NotNull]
        public virtual Image Flyer { get; set; }
        [NotNull, NotEmpty]
        public virtual string Description { get; set; }
        [NotNull]
        public virtual Address Location { get; set; }
        [NotNull]
        public virtual User Owner { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual DateTime EndTime { get; set; }
        [NotNull]
        public virtual EventType EventType { get; set; }
        [NotNull]
        public virtual EventOccurrence Occurrence { get; set; }
        [NotNull]
        public virtual PrivacySetting Privacy { get; set; }

        public virtual IList<EventMember> AllMembers { get; set; }
        public virtual IList<EventMember> InvitedMembers { get; set; }
        public virtual IList<EventMember> RequestedMembers { get; set; }
        public virtual IList<EventMember> ConfirmedMembers { get; set; }
        public virtual IList<EventMember> DeclinedMembers { get; set; }
        public virtual IList<EventMember> AttendedMembers { get; set; }
        public virtual IList<EventMember> AttendingMembers { get; set; }

        public virtual IList<EventCoordinator> AllCoordinators { get; set; }
        public virtual IList<EventCoordinator> ConfirmedCoordinators { get; set; }
        public virtual IList<EventCoordinator> RequestedCoordinators { get; set; }

        public static ImageSizes ThumbnailSize = ImageSizes.W87xL87;
        public static ImageSizes SmallSize = ImageSizes.W180xL200;
        public static IList<ImageSizes> FlyerSizes = new List<ImageSizes>
                                                          {
                                                              ThumbnailSize,
                                                              SmallSize
                                                          };
    }

    public sealed class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Table("Events");

            Id(m => m.Id);

            Map(m => m.Name).Unique();
            Map(m => m.Description);
            Map(m => m.StartDate);
            Map(m => m.StartTime);
            Map(m => m.EndDate);
            Map(m => m.EndTime);
            Map(m => m.EventType);
            Map(m => m.Occurrence);
            Map(m => m.Privacy);

            References(m => m.Location).Column("LocationId").ForeignKey("AddressId").Cascade.SaveUpdate();
            References(m => m.Owner).Column("OwnerId").ForeignKey("UserId");
            References(m => m.Flyer).Column("FlyerId").ForeignKey("ImageId").Cascade.SaveUpdate();

            HasMany(m => m.AllMembers).KeyColumn("EventId").LazyLoad();
            HasMany(m => m.AttendedMembers).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Attended + "'").LazyLoad();
            HasMany(m => m.ConfirmedMembers).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Confirmed + "'").LazyLoad();
            HasMany(m => m.DeclinedMembers).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Declined + "'").LazyLoad();
            HasMany(m => m.RequestedMembers).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Requested + "'").LazyLoad();
            HasMany(m => m.InvitedMembers).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Invited + "'").LazyLoad();
            HasMany(m => m.AttendingMembers).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Attended + "' or Status='" + EventMemberStatus.Confirmed + "'").LazyLoad();

            HasMany(m => m.AllCoordinators).KeyColumn("EventId").LazyLoad();
            HasMany(m => m.ConfirmedCoordinators).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Confirmed + "'").LazyLoad();
            HasMany(m => m.RequestedCoordinators).KeyColumn("EventId").Where("Status='" + EventMemberStatus.Requested + "'").LazyLoad();
        }
    }
}
