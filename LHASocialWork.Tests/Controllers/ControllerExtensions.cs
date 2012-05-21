using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace LHASocialWork.Tests.Controllers
{
    /* Code Created by Roberto Hernández.
     * Located at http://blog.overridethis.com/blog/post/2010/04/22/MVC2-Model-Validation-and-Testing-Scenarios.aspx
     */
    public static class ControllerExtensions
    {
        public static ActionResult CallWithModelValidation<TC, TR, T>(this TC controller
                , Func<TC, TR> action
                , T model)
            where TC : Controller
            where TR : ActionResult
            where T : class
        {

            var validationContext = new ValidationContext(model, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, validationResults);
            foreach (var validationResult in validationResults)
            {
                controller
                    .ModelState
                    .AddModelError(validationResult.MemberNames.First(),
                        validationResult.ErrorMessage);
            }

            return action(controller);
        }
    }
}
