using System.Web.Mvc;
using LHASocialWork.Controllers;
using LHASocialWork.Core.Filters;
using LHASocialWork.Entities.Enumerations;

namespace LHASocialWork.Areas.Admin.Controllers
{
    [RequireHttps, RequiresRole(Role = SystemRole.Admin)]
    public class AdminBaseController : BaseController
    {
        public AdminBaseController(BaseServiceCollection baseServiceCollection) : base(baseServiceCollection){}
    }
}
