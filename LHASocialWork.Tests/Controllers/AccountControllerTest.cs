using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using LHASocialWork.Entities;
using LHASocialWork.Models.Account;
using LHASocialWork.Models.Templates;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Authentication;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LHASocialWork.Controllers;
using Moq;
using Address = LHASocialWork.Models.Templates.Address;

namespace LHASocialWork.Tests.Controllers
{
    /* Controller post method failures should be a result of 
     * invalid models or invalid entities.  The test should check 
     * that on passing in an invalid model, the user is sent back to the original page.
     * Controller post method succecces should be a result of
     * valid models.  The test should check that on pass in a valid 
     * model, the user is redirected to a different page.
     */
    [TestClass]
    public class AccountControllerTest : BaseControllerTest
    {
        private AccountsController _accountController;

        #region Create

        private CreateAccountModel _validCreateAccountResponseModel;
        private CreateAccountModel _invalidCreateAccountResponseModel;

        #region HttpGet

        #region Setup

        private void SetupGetRegister()
        {
            _accountController = new AccountsController(null);
        }

        #endregion

        [TestMethod]
        public void GetRegister()
        {
            SetupGetRegister();

            var response = _accountController.Register("");
            ValidateViewResult<CreateAccountViewModel, AccountsController>(response, true, "Register");
        }

        #endregion

        #region HttpPost

        #region Setup

        private void SetupPostRegister()
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

        private void SetupPostRegisterSuccess()
        {
            SetupPostRegister();
            _validCreateAccountResponseModel = new CreateAccountModel
                                                         {
                                                             PersonInfo = new PersonalInfo
                                                             {

                                                                 FirstName = "TestFirstName",
                                                                 LastName = "TestLastName",
                                                                 PhoneNumber = 1234567890
                                                             },
                                                             SystemInfo = new SystemInfo
                                                             {
                                                                 Email = "TestEmail@test.com",
                                                                 Password = "TestPassword"
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

        private void SetupPostRegisterFailure()
        {
            SetupPostRegister();
            _invalidCreateAccountResponseModel = new CreateAccountModel
                            {
                                PersonInfo = new PersonalInfo
                                {
                                    FirstName = "TestFirstName",
                                    LastName = "TestLastName"
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
        public void PostRegisterSuccess()
        {
            SetupPostRegisterSuccess();
            var controllerResponse = _accountController.CallWithModelValidation(x => x.Register(_validCreateAccountResponseModel), _validCreateAccountResponseModel);
            ValidateRouteOfRedirectResponse(controllerResponse, "index", "home");
        }

        [TestMethod]
        public void PostRegisterFailure()
        {
            SetupPostRegisterFailure();
            var controllerResponse = _accountController.CallWithModelValidation(x => x.Register(_invalidCreateAccountResponseModel), _invalidCreateAccountResponseModel);
            ValidateModelStateOfUnsuccessfulPostResponse(controllerResponse);
        }

        #endregion

        #endregion

        #region Authenicate

        #region HttpGet

        private void SetupGetLogInForNonAuthenticatedUser()
        {
            _accountController = new AccountsController(null);
        }

        private void SetupGetLogInForAuthenticatedUser()
        {
            var mockUser = new MockUser { Identity = new MockIdentity("", "", true) };
            var mockHttpContext = new MockHttpContextBase { User = mockUser };
            var controllerContext = new ControllerContext { HttpContext = mockHttpContext };
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns(new User());
            var baseServiceCollection = new BaseServiceCollection(accountService.Object, null, null);
            _accountController = new AccountsController(baseServiceCollection) { ControllerContext = controllerContext };
        }

        [TestMethod]
        public void GetLogInForNonAuthenticatedUser()
        {
            SetupGetLogInForNonAuthenticatedUser();

            var response = _accountController.LogIn("") as ViewResult;
            ValidateViewResult<AccountLogInModel, AccountsController>(response, true, "LogIn");
        }

        [TestMethod]
        public void GetLogInForAuthenticatedUser()
        {
            SetupGetLogInForAuthenticatedUser();

            var response = _accountController.LogIn("") as RedirectToRouteResult;

            Assert.IsNotNull(response, "Redirect result should not be null");
            Assert.IsTrue(response.IsCorrectRoute("Index", "Home"));
        }

        #endregion

        #region HttpPost

        private static AccountLogInModel ValidAccountLogInModel
        {
            get
            {
                return new AccountLogInModel
                           {
                               EmailAddress = "test@test.com",
                               RememberMe = false,
                               Password = "Vitec4IT"
                           };
            }
        }

        #region Setup

        private void SetupPostLogInSuccess()
        {
            var authenicationService = new Mock<IAuthenticationService>();
            authenicationService.Setup(x => x.ValidateUser(ValidAccountLogInModel.EmailAddress, HashString(ValidAccountLogInModel.Password))).Returns(true);

            var httpContext = new MockHttpContextBase { User = new MockUser { Identity = new MockIdentity("", "", false) } };
            var controllerContext = new ControllerContext(httpContext, new RouteData(), new BaseController(null));
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns<User>(null);
            _accountController = new AccountsController(new BaseServiceCollection(accountService.Object, null, authenicationService.Object)) { ControllerContext = controllerContext };
        }

        private void SetupPostLogOut()
        {
            _accountController = new AccountsController(null);
            var httpContext = new MockHttpContextBase { User = new MockUser { Identity = new MockIdentity("", "", false) } };
            var controllerContext = new ControllerContext(httpContext, new RouteData(), new BaseController(null));
            _accountController = new AccountsController(null) { ControllerContext = controllerContext };
        }

        #endregion

        [TestMethod]
        public void PostLogInSucess()
        {
            SetupPostLogInSuccess();
            var response = _accountController.LogIn(ValidAccountLogInModel) as RedirectToRouteResult;

            Assert.IsNotNull(response, "Redirect result should not be null");
            Assert.IsTrue(response.IsCorrectRoute("Index", "Home"));
            Assert.IsNotNull(_accountController.Response.Cookies[FormsAuthentication.FormsCookieName],
                "Authentication cookie should not be null");
        }

        [TestMethod, Ignore]
        public void PostLogOut()
        {
            SetupPostLogOut();
            var response = _accountController.LogOut() as RedirectToRouteResult;

            Assert.IsNotNull(response, "Redirect result should not be null");
            Assert.IsTrue(response.IsCorrectRoute("Index", "Home"));
            Assert.IsNull(_accountController.Response.Cookies[FormsAuthentication.FormsCookieName], "Authentication cookie should be null");
        }

        #endregion

        #endregion

        #region Search Users By Name

        #region XMLHttpGet

        #region Setup

        private void SetupXmlGetSearchUsersByName()
        {
            var service = new Mock<IAccountService>();
            var mockRequest = new Mock<HttpRequestBase>();
            var httpContext = new Mock<HttpContextBase>();

            service.Setup(x => x.FindUsers(It.IsAny<UsersSearchCriteria>())).Returns(new List<User>());
            _accountController = new AccountsController(new BaseServiceCollection(service.Object, null, null));

            mockRequest.SetupGet(x => x.Headers).Returns(new WebHeaderCollection { { "X-Requested-With", "XMLHttpRequest" } });
            httpContext.SetupGet(x => x.Request).Returns(mockRequest.Object);
            _accountController.ControllerContext = new ControllerContext(httpContext.Object, new RouteData(), _accountController);
        }

        #endregion

        [TestMethod]
        public void XmlGetSearchUsersByName()
        {
            SetupXmlGetSearchUsersByName();
            var result = _accountController.SearchByName(new AccountSearchByNameModel()) as JsonResult;
            Assert.IsNotNull(result, "JSON Result should not be null.");
        }

        #endregion

        #endregion

    }
}