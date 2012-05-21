using NHibernate.Validator.Engine;

namespace LHASocialWork.Entities.Core
{
    public interface IValidationRequiredEntity
    {
        InvalidValue[] InvalidValues { get; }
        bool IsValid { get; }
        void AddInvalidValue(InvalidValue invalidValue);
        void AddInvalidValues(InvalidValue[] invalidValues);
    }
}