using System.Web.Mvc;

namespace LHASocialWork.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(BaseServiceCollection baseServiceCollection) : base(baseServiceCollection){}

        #region Index

        public ActionResult Index()
        {
            return User == null ? View("StartPage") : View();
        }

        #endregion

        #region Access Restricted

        [HttpGet]
        public ActionResult AccessRestricted()
        {
            DisplayError("You do not have permission to access this page.");
            return View();
        }

        #endregion

        #region Error Pages

        public ActionResult UnhandledError()
        {
            DisplayError("We're sorry! An unexpected error has occured. Our engineers have been notified.");
            return View("Index");
        }

        public ActionResult PageNotFound()
        {
            DisplayError("The page you request was not found.");
            return View("Index");
        }

        #endregion
    }
}
