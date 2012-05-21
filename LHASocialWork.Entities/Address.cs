using FluentNHibernate.Mapping;
using LHASocialWork.Entities.Core;
using NHibernate.Validator.Constraints;

namespace LHASocialWork.Entities
{
    public class Address : ValidationRequiredEntity
    {
        [NotNull, NotEmpty, Length(255)]
        public virtual string Street { get; set; }
        [NotNull, NotEmpty, Length(255)]
        public virtual string City { get; set; }
        [Length(255)]
        public virtual string Province { get; set; }
        [Length(255)]
        public virtual string State { get; set; }
        [NotNull, NotEmpty, Length(255)]
        public virtual string Country { get; set; }
        [NotNull, NotEmpty, Length(255)]
        public virtual string Zip { get; set; }

       
    }

    public sealed class AddressMap : ClassMap<Address>
    {
        public AddressMap()
        {
            Table("Addresses");
            Id(x => x.Id);
            Map(x => x.Street);
            Map(x => x.City);
            Map(x => x.Province);
            Map(x => x.State);
            Map(x => x.Country);
            Map(x => x.Zip);
        }
    }
}