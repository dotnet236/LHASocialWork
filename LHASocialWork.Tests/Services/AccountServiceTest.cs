using System.Collections.Generic;
using LHASocialWork.Entities;
using LHASocialWork.Repositories;
using LHASocialWork.Services;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Image;
using LHASocialWork.Services.Position;
using LHASocialWork.Tests.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;
using NHibernate.Validator.Engine;

namespace LHASocialWork.Tests.Services
{
    [TestClass]
    public class AccountServiceTest : BaseServiceTest
    {
        private IAccountService _accountService;

        #region Create

        private static User ValidUser
        {
            get
            {
                return new User
                           {
                               Email = "TestEmail@test.com",
                               FirstName = "TestFirstName",
                               LastName = "TestLastName",
                               Password = "TestPassword",
                               PhoneNumber = 1234567890,
                               Address = new Address
                               {
                                   City = "TestAddressCity",
                                   Country = "TestAddressCountry",
                                   Province = "TestAddressProvince",
                                   State = "TestAddressState",
                                   Street = "TestAddressStreet",
                                   Zip = "12345"
                               },
                               ProfilePicture = new Image()
                           };
            }
        }
        private static User InvalidUser
        {
            get
            {
                return new User
                           {
                               Email = "TestEmail@test.com",
                               FirstName = "TestFirstName",
                               LastName = "TestLastName",
                               Password = "TestPassword",
                               PhoneNumber = 1234567890,
                               Positions = new List<UserPosition>()
                           };
            }
        }

        #region Setup

        private void SetupSuccessfulSave()
        {
            var positionService = new Mock<IPositionService>();
            positionService.SetupGet(x => x.MemberPosition).Returns(new Position());
            MockBaseRepository.Setup(x => x.SaveOrUpdate(new User(), null)).Returns(new User());
            MockBaseRepository.Setup(x => x.List<User>(DetachedCriteria.For<User>())).Returns(new List<User>());
            _accountService = new AccountService(MockBaseRepository.Object, new ImageService(MockBaseRepository.Object), positionService.Object);
        }

        private void SetupUnsuccessfulSave()
        {
            MockBaseRepository.Setup(x => x.SaveOrUpdate(new User(), null)).Returns(new User());
            var positionService = new Mock<IPositionService>();
            positionService.SetupGet(x => x.MemberPosition).Returns(new Position());
            _accountService = new AccountService(MockBaseRepository.Object, new ImageService(MockBaseRepository.Object), positionService.Object);
        }

        #endregion

        [TestMethod]
        public void CreateUserSuccess()
        {
            SetupSuccessfulSave();
            var user = _accountService.SaveUser(ValidUser);
            ValidateSuccessfulEntityCreation(user);
        }

        [TestMethod]
        public void CreateUserFailure()
        {
            SetupUnsuccessfulSave();
            var user = _accountService.SaveUser(InvalidUser);
            ValidateUnsuccessfulEntityCreation(user);
        }

        #endregion

        #region List

        #region Setup

        private void SetupGetUsers()
        {
            MockBaseRepository.Setup(x => x.List<User>(DetachedCriteria.For<User>())).Returns(new List<User>());
            _accountService = new AccountService(MockBaseRepository.Object, null, null);
        }

        #endregion

        [TestMethod]
        public void GetUsers()
        {
            SetupGetUsers();
            var users = _accountService.GetUsers();
            Assert.IsNotNull(users, "Accounts list should not be null");
        }

        #endregion

        #region Get User By ID

        #region Setup

        private void SetupGetUserById()
        {
            MockBaseRepository.Setup(x => x.Get<User>(It.IsAny<long>())).Returns(new User());
            _accountService = new AccountService(MockBaseRepository.Object, null, null);
        }

        #endregion

        [TestMethod]
        public void GetUserById()
        {
            SetupGetUserById();
            Assert.IsNotNull(_accountService.GetUserById(4), "User should not be null.");
        }

        #endregion
    }
}
