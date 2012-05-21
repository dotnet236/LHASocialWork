using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using LHASocialWork.Entities.Enumerations;
using NHibernate.Validator.Constraints;

namespace LHASocialWork.Entities
{
    public class User : ValidationRequiredEntity
    {
        public User()
        {
            Positions = new List<UserPosition>();
        }

        [NotNull]
        public virtual bool SystemUser { get; set; }
        [NotNull, NotEmpty, Email]
        public virtual string Email { get; set; }
        [NotNull, NotEmpty, Length(6, 255)]
        public virtual  string Password { get; set; }
        [NotNull, NotEmpty, Length(255)]
        public virtual string FirstName { get; set; }
        [NotNull, NotEmpty, Length(255)]
        public virtual string LastName { get; set; }
        [NotNull]
        public virtual long PhoneNumber { get; set; }
        [NotNull]
        public virtual Address Address { get; set; }
        [NotNull]
        public virtual Image ProfilePicture { get; set; }

        public virtual IList<UserPosition> Positions { get; set; }
        public virtual IList<EventMember> MemberEvents { get; set; }
        public virtual IList<EventCoordinator> CoordinatorEvents { get; set; }

        public virtual bool IsAdmin
        {
            get { return Positions.Any(x => x.Position.SystemRole == SystemRole.Admin); }
        }
        public virtual bool IsStaff
        {
            get { return Positions.Any(x => x.Position.SystemRole == SystemRole.Staff); }
        }

        public static ImageSizes ThumbnailSize = ImageSizes.W44xL44;
        public static ImageSizes SmallSize = ImageSizes.W87xL87;

        public static IList<ImageSizes> ProfilePictureSizes = new List<ImageSizes>
                                                          {
                                                              ThumbnailSize,
                                                              SmallSize
                                                          };
    }

    public sealed class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(m => m.Id);

            Map(x => x.SystemUser);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.PhoneNumber);

            References(x => x.Address).Column("AddressId").ForeignKey("AddressId").Cascade.SaveUpdate();
            References(x => x.ProfilePicture).Column("ProfilePictureId").ForeignKey("ImageId").Cascade.SaveUpdate();

            HasMany(x => x.Positions).KeyColumn("UserId").Cascade.SaveUpdate().Cascade.Delete().Cascade.SaveUpdate().LazyLoad();
            HasMany(x => x.MemberEvents).KeyColumn("UserId").Cascade.SaveUpdate().Cascade.Delete().Cascade.SaveUpdate().LazyLoad();
            HasMany(x => x.CoordinatorEvents).KeyColumn("CoordinatorId").Cascade.SaveUpdate().Cascade.Delete().Cascade.SaveUpdate().LazyLoad();
        }
    }
}