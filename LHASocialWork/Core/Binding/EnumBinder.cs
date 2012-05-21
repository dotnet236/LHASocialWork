using System;
using System.Linq;
using System.Web.Mvc;

/* Code created by Rupert Bates. 
 * Modified by Anthony Capone.
 * Located at : http://eliasbland.wordpress.com/2009/08/08/enumeration-model-binder-for-asp-net-mvc/
 */
namespace LHASocialWork.Core.Binding
{
    public class EnumBinder<T> : IModelBinder
    {
        private T DefaultValue { get; set; }
        public EnumBinder(T defaultValue)
        {
            DefaultValue = defaultValue;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return bindingContext.ValueProvider.GetValue(bindingContext.ModelName) == null
                       ? DefaultValue
                       : GetEnumValue(DefaultValue, bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue);
        }

        public static TR GetEnumValue<TR>(TR defaultValue, string value)
        {
            var enumType = defaultValue;

            if ((!String.IsNullOrEmpty(value)) && (Contains(typeof(TR), value)))
                enumType = (TR)Enum.Parse(typeof(TR), value, true);

            return enumType;
        }

        public static bool Contains(Type enumType, string value)
        {
            return Enum.GetNames(enumType).Contains(value, StringComparer.OrdinalIgnoreCase);
        }
    }
}