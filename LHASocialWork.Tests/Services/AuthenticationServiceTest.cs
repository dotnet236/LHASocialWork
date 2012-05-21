using System.Collections.Generic;
using LHASocialWork.Entities;
using LHASocialWork.Services.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;

namespace LHASocialWork.Tests.Services
{
    [TestClass]
    public class AuthenticationServiceTest : BaseServiceTest
    {
        private IAuthenticationService _authenticationService;

        private readonly User _validUser = new User {Email = "ValidEmailAddress@address.com", Password = "ValidPassword"};

        private void SetupValidateUserSuccess()
        {
            MockBaseRepository.Setup(x => x.List<User>(It.IsAny<DetachedCriteria>())).Returns(new List<User>{ new User() });
            _authenticationService = new AuthenticationService(MockBaseRepository.Object);
        }

        [TestMethod]
        public void ValidateUserSuccess()
        {
            SetupValidateUserSuccess();
            Assert.IsTrue(_authenticationService.ValidateUser(_validUser.Email, _validUser.Password));
        }
    }
}
