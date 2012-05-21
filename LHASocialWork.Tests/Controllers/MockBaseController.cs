using System.Collections.Generic;
using LHASocialWork.Controllers;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;

namespace LHASocialWork.Tests.Controllers
{
    public class MockBaseController : BaseController
    {
        public MockBaseController(BaseServiceCollection serviceCollection)
            : base(serviceCollection)
        {
        }

        public new Image CreateImageLocally(IEnumerable<ImageSizes> sizes,  IHttpPostedFile postedFile)
        {
            return base.CreateImageLocally(sizes, postedFile);
        }
    }
}
