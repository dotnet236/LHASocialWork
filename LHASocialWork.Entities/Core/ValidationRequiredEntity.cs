using FluentNHibernate.Data;
using NHibernate.Validator.Engine;

namespace LHASocialWork.Entities.Core
{
    public class ValidationRequiredEntity : Entity, IValidationRequiredEntity
    {
        public ValidationRequiredEntity()
        {
            InvalidValues = new InvalidValue[0];
        }

        public virtual bool IsValid
        {
            get { return InvalidValues != null && InvalidValues.Length == 0; }
        }

        public virtual InvalidValue[] InvalidValues { get; private set; }

        public virtual void AddInvalidValue(InvalidValue invalidValue)
        {
            if (InvalidValues == null)
                InvalidValues = new InvalidValue[1];

            var tmpArray = new InvalidValue[InvalidValues.Length + 1];
            tmpArray[InvalidValues.Length] = invalidValue;

            if (InvalidValues.Length > 0)
                InvalidValues.CopyTo(tmpArray, 0);
            InvalidValues = tmpArray;
        }

        public virtual void AddInvalidValues(InvalidValue[] invalidValues)
        {
            foreach (var invalidValue in invalidValues)
                AddInvalidValue(invalidValue);
        }
    }
}