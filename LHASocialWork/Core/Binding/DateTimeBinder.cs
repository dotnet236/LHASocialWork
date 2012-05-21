using System;
using System.Web.Mvc;

namespace LHASocialWork.Core.Binding
{
    public class DateTimeBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                return Convert.ToDateTime(bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue);
            }catch(Exception)
            {
                return DateTime.MinValue;
            }
        }
    }
}