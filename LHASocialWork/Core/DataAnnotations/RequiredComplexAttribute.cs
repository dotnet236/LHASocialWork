using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LHASocialWork.Core.DataAnnotations
{
    public class RequiredComplexAttribute : RequiredAttribute
    {
        public RequiredComplexAttribute()
        {
            ValidateCollections = true;
        }

        public override bool IsValid(object value)
        {
            if(value == null)
                return false;

            if (ValidateCollections && value is ICollection)
            {
                var items = (ICollection)value;
                return items.Cast<object>().Any(IsValid);
            }

            if (value.GetType().IsArray)
                return ((object[])value).Length > 0;

            return base.IsValid(value);
        }

        public bool ValidateCollections { get; set; }
    }
}