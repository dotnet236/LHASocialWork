using System;
using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using LHASocialWork.Entities.Enumerations;
using NHibernate.Validator.Constraints;

namespace LHASocialWork.Entities
{
    public class Image : ValidationRequiredEntity
    {
        [NotNull, NotEmpty, Length(255)]
        public virtual string Title { get; set; }
        [NotNull, NotEmpty, Length(255)]
        public virtual string Description { get; set; }
        [NotNull]
        public virtual Guid FileKey { get; set; }
        [NotNull]
        public virtual User Owner { get; set; }
        [NotNull]
        public virtual ImageStatus Status { get; set; }
    }

    public sealed class ImageClassMap : ClassMap<Image>
    {
        public ImageClassMap()
        {
            Table("Images");
            Id(m => m.Id);
            Map(m => m.Title);
            Map(m => m.Description);
            Map(m => m.FileKey);
            Map(m => m.Status);
            References(m => m.Owner).Column("OwnerId").ForeignKey("UserId");
        }
    }
}
