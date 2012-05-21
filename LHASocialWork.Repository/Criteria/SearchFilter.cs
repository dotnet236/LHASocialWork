using System;
using NHibernate.Criterion;

namespace LHASocialWork.Repositories.Criteria
{
    public class SearchFilter<T>
    {
        public string PropertyName { get; set; }
        public object PropertyValue { get; set; }
        public SearchConditional Conditional { get; set; }

        public ICriterion GenerateRestriction(string alias = "this")
        {
            if (PropertyName != null)
            {
                var propertyName = string.Format("{0}.{1}", alias, PropertyName);
                var propertyValue = ConvertPropertyValue();

                switch (Conditional)
                {
                    case SearchConditional.Equals:
                        return Restrictions.Eq(propertyName, propertyValue);
                    case SearchConditional.NotEquals:
                        return Restrictions.Not(Restrictions.Eq(propertyName, propertyValue));
                    case SearchConditional.Like:
                        return Restrictions.Like(propertyName, (string)propertyValue, MatchMode.Anywhere);
                    case SearchConditional.SensitiveLike:
                        return Restrictions.Like(propertyName, (string)propertyValue, MatchMode.Anywhere);
                    case SearchConditional.LessThan:
                        return Restrictions.Le(propertyName, propertyValue);
                    case SearchConditional.LessThanOrEqual:
                        return Restrictions.Or(Restrictions.Le(propertyName, propertyValue), Restrictions.Eq(propertyName, propertyValue));
                    case SearchConditional.GreaterThan:
                        return Restrictions.Ge(propertyName, propertyValue);
                    case SearchConditional.GreaterThanOrEqual:
                        return Restrictions.Or(Restrictions.Ge(propertyName, propertyValue), Restrictions.Eq(propertyName, propertyValue));
                }
            }

            return null;
        }

        private object ConvertPropertyValue()
        {
            var entityType = typeof(T);
            var propertyType = entityType.GetProperty(PropertyName).PropertyType;

            if (propertyType == typeof(DateTime))
                return Convert.ToDateTime(PropertyValue);
            if (propertyType == typeof(long))
                return Convert.ToInt32(PropertyValue);
            if (propertyType == typeof(int))
                return Convert.ToInt32(PropertyValue);
            if (propertyType == typeof(decimal))
                return Convert.ToDecimal(PropertyValue);
            if (propertyType == typeof(double))
                return Convert.ToDouble(PropertyValue);
            if (propertyType == typeof(bool))
                return Convert.ToBoolean(PropertyValue);
            if (propertyType == typeof(string))
                return Convert.ToString(PropertyValue);
            if (propertyType == typeof(Guid))
                return new Guid(PropertyValue.ToString());

            return PropertyValue;
        }
    }
}