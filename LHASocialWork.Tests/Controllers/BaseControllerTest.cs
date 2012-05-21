using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using LHASocialWork.Controllers;
using LHASocialWork.Core;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Image;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WatiN.Core;

namespace LHASocialWork.Tests.Controllers
{
    [TestClass]
    public class BaseControllerTest : BaseTest
    {
        protected MockBaseController BaseController;

        protected static IList<string> ErrorMessages = new List<string>
                                                    {
                                                        "Server Error",
                                                        "Unexpected Error",
                                                        "Event not found",
                                                    };

        protected static RequestContext RequestContext
        {
            get
            {
                return new RequestContext(new MockHttpContextBase(), new RouteData());
            }
        }

        protected static BaseServiceCollection GetBaseServiceCollection(bool userIsAdmin = true)
        {
            var user = new Mock<User>();
            user.Setup(x => x.IsAdmin).Returns(userIsAdmin);
            user.Setup(x => x.MemberEvents).Returns(new List<EventMember>());
            var accountService = new Mock<IAccountService>();
            accountService.Setup(x => x.GetUserByEmailAddress(It.IsAny<string>())).Returns(user.Object);
            return new BaseServiceCollection(accountService.Object, null, null);
        }

        protected static ControllerContext GetControllerContext(bool userIsAuthenticated, RouteData routeData = null)
        {
            var mockUser = new MockUser { Identity = new MockIdentity("", "", true) };
            var mockHttpContext = new MockHttpContextBase { User = userIsAuthenticated ? mockUser : null };
            return new ControllerContext { RouteData = routeData ?? new RouteData(), HttpContext = mockHttpContext };
        }

        protected static void ValidateViewResult<TModel, TController>( ActionResult actionResult, bool actionMatchesView, string expectedViewName, 
                                                                                            string expectedAreaName = null, object routeValues = null)
            where TModel : class
            where TController : Controller
        {
            #region Test view result is not null and name is valid

            var viewResult = actionResult as ViewResult;

            Assert.IsNotNull(viewResult, "Should have returned a ViewResult");
            Assert.AreEqual(actionMatchesView ? "" : expectedViewName, viewResult.ViewName, "View name should have been {0}", expectedViewName == "" ? "'SAME AS ACTION METHOD NAME'" : expectedViewName);

            #endregion

            #region Test model on view is of correct type

            var model = viewResult.ViewData.Model as TModel;
            Assert.IsNotNull(model, "Model should not be null");

            #endregion

            #region Test view renders without exception

            var routeParams = "";
            if (routeValues != null)
            {
                var type = routeValues.GetType();
                var props = type.GetProperties();
                var pairs = props.Select(x => x.Name + "=" + x.GetValue(routeValues, null)).ToArray();
                var result = string.Join("&", pairs);
                if (!string.IsNullOrEmpty(result)) routeParams = "?" + result;
            }

            var controller = typeof(TController).Name.ToLower();
            controller = controller.Substring(0, controller.IndexOf("controller"));
            var area = (string.IsNullOrEmpty(expectedAreaName) ? "" : expectedAreaName + "/");
            var route = Bootstrapper.ApplicationName + "/" + area + controller + "/" + expectedViewName + routeParams;
            var url = string.Format("http://{0}/{1}", DomainName, route);

            using (var browser = new IE(url))
            {
                #region Https Cert Error

                if (browser.Links.Exists("overridelink"))
                {
                    browser.Link("overridelink").Click();
                    browser.WaitForComplete();
                }

                #endregion

                #region LogIn

                if (browser.ContainsText("Please enter your username and password."))
                {
                    browser.TextField("EmailAddress").Value = "office@lhasocialwork.org";
                    browser.TextField("Password").Value = "Vitec4IT";
                    browser.Button("submit").Click();
                    browser.WaitForComplete();
                }

                #endregion

                #region Check for errors on page

                foreach (var error in ErrorMessages)
                    Assert.IsFalse(browser.ContainsText(error));

                #endregion
            }

            #endregion
        }

        protected static void ValidateRouteOfRedirectResponse(ActionResult actionResult, string action, string controller = null, string area = null)
        {
            var redirectResult = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirectResult, "Redirect response returned should not be null");
            Assert.IsTrue(redirectResult.IsCorrectRoute(action, controller, area));
        }

        protected static void ValidateModelStateOfUnsuccessfulPostResponse(ActionResult actionResult)
        {
            var viewResult = actionResult as ViewResult;
            Assert.IsNotNull(viewResult, "View response returned should not be null");
            Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);
        }

        private void SetupCreateImageLocally()
        {
            var serviceCollection = new BaseServiceCollection(null, new ImageService(null), null);
            BaseController = new MockBaseController(serviceCollection);
        }

        [TestMethod]
        public void CreateImageLocally()
        {
            SetupCreateImageLocally();

            var imageSizes = new List<ImageSizes> { ImageSizes.W180xL200 };
            using (var memoryStream = new MemoryStream())
            {
                const string fileName = "TestFile.png";
                var testBitmap = new Bitmap(400, 400);
                testBitmap.Save(memoryStream, ImageFormat.Png);

                var postedFile = new Mock<IHttpPostedFile>();
                postedFile.Setup(x => x.FileName).Returns(fileName);
                postedFile.Setup(x => x.InputStream).Returns(memoryStream);

                var tmpPath = Path.GetTempPath();
                Bootstrapper.LocalImagesPath = tmpPath;

                var image = BaseController.CreateImageLocally(imageSizes, postedFile.Object);
                var directory = image.FileKey.ToString().Replace("-", "");
                var filePath = Path.Combine(tmpPath, directory, ImageSizes.W180xL200.ToString(), Path.ChangeExtension(Bootstrapper.LocalImageName, "png"));

                try { Assert.IsTrue(File.Exists(filePath)); }
                finally { File.Delete(filePath); }
            }
        }
    }
}
