using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Services.Account;
using StructureMap;

namespace LHASocialWork.Core.Filters
{
    public class RequiresRoleAttribute : ActionFilterAttribute
    {
        private readonly IAccountService _accountService;
        public SystemRole Role { get; set; }
        private List<SystemRole> Roles { get; set; }

        public RequiresRoleAttribute()
        {
            _accountService = ObjectFactory.GetInstance<IAccountService>();
        }

        public RequiresRoleAttribute(params SystemRole[] roles)
        {
            Roles = roles.ToList();
            _accountService = ObjectFactory.GetInstance<IAccountService>();
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            var urlHelper = new UrlHelper(filterContext.RequestContext);

            var redirectUrl = "";
            if (filterContext.HttpContext.Request.Url != null)
            {
                var redirectOnSuccess = filterContext.HttpContext.Request.Url.PathAndQuery;
                redirectUrl = string.Format("?returnUrl={0}", redirectOnSuccess);
            }

            if (user != null && !Bootstrapper.TestMode)
            {
                if (!user.Identity.IsAuthenticated)
                {
                    var loginUrl = urlHelper.Action("LogIn", "Accounts", new { area = "" }) + redirectUrl;
                    filterContext.Result = new RedirectResult(loginUrl);
                }
                else
                {
                    if (Roles == null) Roles = new List<SystemRole>();
                    Roles.Add(Role);

                    var account = _accountService.GetUserByEmailAddress(user.Identity.Name);
                    //TODO: Remove role override
                    if (!account.Positions.Any(x => Roles.Any(sysRole => sysRole == x.Position.SystemRole)) && filterContext.HttpContext.Request["Override"] != "true")
                    {
                        var accessExceptionUrl = urlHelper.Action("AccessRestricted", "Home", new { area = "" });
                        filterContext.Result = new RedirectResult(accessExceptionUrl);
                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}