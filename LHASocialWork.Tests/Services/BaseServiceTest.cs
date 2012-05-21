using System.Linq;
using LHASocialWork.Entities.Core;
using LHASocialWork.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LHASocialWork.Tests.Services
{
    public class BaseServiceTest : BaseTest
    {
        private Mock<IBaseRepository> _baseRepository;
        protected Mock<IBaseRepository> MockBaseRepository
        {
            get { return _baseRepository ?? (_baseRepository = new Mock<IBaseRepository>()); }
        }

        protected void ValidateSuccessfulEntityCreation(ValidationRequiredEntity entity)
        {
            Assert.IsTrue(!entity.InvalidValues.Any(), "Entity has invalid values assigned");
        }

        protected void ValidateUnsuccessfulEntityCreation(ValidationRequiredEntity entity)
        {
            Assert.IsTrue(entity.InvalidValues.Any(), "Entity should have invalid values assigned");
        }
    }
}
