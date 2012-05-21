using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LHASocialWork;
using LHASocialWork.Controllers;

namespace LHASocialWork.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : BaseControllerTest
    {
        private HomeController _homeController;

        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController(null);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            if (result == null) return;

            var viewData = result.ViewData;
            //Assert.AreEqual("Welcome to ASP.NET MVC!", viewData["Message"]);
        }

        #region Error

        #region HttpGet

        #region

        private void SetupGetUnhandledError()
        {
            _homeController = new HomeController(null);
        }

        #endregion

        [TestMethod]
        public void GetUnhandledError()
        {
            SetupGetUnhandledError();
            var result = _homeController.UnhandledError() as ViewResult;
            Assert.IsNotNull(result, "View should now be null");
            Assert.IsNotNull(result.TempData["Error"], "No errors found in temp data");
        }

        #endregion

        #endregion

    }
}
