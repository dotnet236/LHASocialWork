using System.Collections.Generic;
using System.Web.Mvc;
using LHASocialWork.Areas.Admin.Controllers;
using LHASocialWork.Areas.Admin.Models.Accounts;
using LHASocialWork.Controllers;
using LHASocialWork.Entities;
using LHASocialWork.Models.Account;
using LHASocialWork.Models.Templates;
using LHASocialWork.Services.Account;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using AccountsController = LHASocialWork.Areas.Admin.Controllers.AccountsController;
using Address = LHASocialWork.Models.Templates.Address;

namespace LHASocialWork.Tests.Controllers
{
    [TestClass]
    public class AdminAccountControllerTest : BaseControllerTest
    {
        private AccountsController _accountController;

        #region Read

        #region Setup

        private void SetupGetUsers()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUsers()).Returns(new List<User>());
            _accountController = new AccountsController(new BaseServiceCollection(accountService.Object, null, null));
        }

        #endregion

        #region HttpGet

        public void GetUsers()
        {
            SetupGetUsers();
            var response = _accountController.Index(null);
            ValidateViewResult<AccountsModel, PositionsController>(response, true, "Index");
        }

        #endregion

        #endregion

        #region Create

        private CreateAccountModel _validCreateAccountResponseModel;
        private CreateAccountModel _invalidCreateAccountResponseModel;

        #region HttpGet

        private void SetupGetCreateAccount()
        {
            _accountController = new AccountsController(null);
        }

        public void GetCreateAccount()
        {
            SetupGetCreateAccount();
            var response = _accountController.Create() as ViewResult;
            ValidateViewResult<CreateAccountModel, PositionsController>(response, true, "Create");
        }

        #endregion

        #region HttpPost

        #region Setup

        private void SetupPostCreateUser()
        {
            var accountService = new Mock<IAccountService>();
            accountService.Setup(acct => acct.SaveUser(new User())).Returns(new User());
            _accountController = new AccountsController(new BaseServiceCollection(accountService.Object, null, null));
            _accountController.ControllerContext = new ControllerContext
                                                       {
                                                           Controller = _accountController,
                                                           RequestContext = RequestContext
                                                       };
        }

        private void SetupPostCreateUserSuccess()
        {
            SetupPostCreateUser();
            _validCreateAccountResponseModel = new CreateAccountModel
                                                         {
                                                             SystemInfo = new SystemInfo
                                                                              {
                                                                                  Email = "TestEmail@test.com",
                                                                                  Password = "TestPassword",
                                                                              },
                                                             PersonInfo = new PersonalInfo
                                                             {
                                                                 FirstName = "TestFirstName",
                                                                 LastName = "TestLastName",
                                                                 PhoneNumber = 1234567890,
                                                             },
                                                             Address = new Address
                                                                 {
                                                                     City = "TestAddressCity",
                                                                     Country = "TestAddressCountry",
                                                                     Province = "TestAddressProvince",
                                                                     State = "TestAddressState",
                                                                     Street = "TestAddressStreet",
                                                                     Zip = "12345"
                                                                 }
                                                         };
        }

        private void SetupPostCreateUserFailure()
        {
            SetupPostCreateUser();
            _invalidCreateAccountResponseModel = new CreateAccountModel
                                                         {
                                                             PersonInfo = new PersonalInfo
                                                             {
                                                                 FirstName = "TestFirstName",
                                                                 LastName = "TestLastName",
                                                                 PhoneNumber = 1234567890,
                                                             },
                                                             Address = new Address
                                                                 {
                                                                     City = "TestAddressCity",
                                                                     Country = "TestAddressCountry",
                                                                     Province = "TestAddressProvince",
                                                                     State = "TestAddressState",
                                                                     Street = "TestAddressStreet",
                                                                     Zip = "12345"
                                                                 }
                                                         };
        }

        #endregion

        [TestMethod]
        public void PostCreateUserFailure()
        {
            SetupPostCreateUserFailure();
            var controllerResponse = _accountController.CallWithModelValidation(x => x.Create(_invalidCreateAccountResponseModel),
                                                                                        _invalidCreateAccountResponseModel);
            ValidateModelStateOfUnsuccessfulPostResponse(controllerResponse);
        }

        [TestMethod]
        public void PostCreateUserSuccess()
        {
            SetupPostCreateUserSuccess();
            var controllerResponse = _accountController.CallWithModelValidation(x => x.Create(_validCreateAccountResponseModel),
                                                                                        _validCreateAccountResponseModel);
            ValidateRouteOfRedirectResponse(controllerResponse, "index");
        }

        #endregion

        #endregion
    }
}
