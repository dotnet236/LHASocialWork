using System;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using LHASocialWork.Core.Filters;
using LHASocialWork.Entities;
using LHASocialWork.Models.Account;
using LHASocialWork.Models.Shared.JSON;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Authentication;

namespace LHASocialWork.Controllers
{
    [RequireHttps]
    public class AccountsController : BaseController
    {
        public AccountsController (BaseServiceCollection baseServiceCollection) : base(baseServiceCollection){}

        #region Index

        [RequiresAuthentication]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Register

        [RequireHttps, RequiresAnonymous()]
        public ActionResult Register(string returnUrl)
        {
            return View(new CreateAccountViewModel { FormPostAction = "Register", ReturnUrl = returnUrl });
        }

        [RequireHttps, HttpPost, ValidateAntiForgeryToken, RequiresAnonymous()]
        public ActionResult Register(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<CreateAccountModel, User>(model);
                user = AccountService.SaveUser(user);
                if (!user.InvalidValues.Any())
                {
                    if (Request["returnUrl"] != null)
                        return Redirect(Request["returnUrl"]);
                    return RedirectToAction("Index", "Home");
                }
                AddValidationResults(user.InvalidValues);
                DisplayError("Registration failed.  There are " + user.InvalidValues.Length + " error(s) on the page.");
            }

            var invalidViewModel = Mapper.Map<CreateAccountModel, CreateAccountViewModel>(model);
            return View(invalidViewModel);
        }

        #endregion

        #region Authentication

        [RequireHttps]
        public ActionResult LogIn(string returnUrl)
        {
            if (User != null)
                return RedirectToAction("Index", "Home");

            return View(new AccountLogInModel { ReturnUrl = returnUrl });
        }

        [HttpPost, ValidateAntiForgeryToken, RequireHttps]
        public ActionResult LogIn(AccountLogInModel model)
        {
            if (User == null)
            {
                if (!ModelState.IsValid)
                    return View(model);
                if (!AuthenticationService.ValidateUser(model.EmailAddress, HashString(model.Password)))
                {
                    DisplayError("Sorry, invalid username and/or password.  Please try again.");
                    return View(model);
                }
                SetAuthCookie(model);
                if (!string.IsNullOrEmpty(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void SetAuthCookie(AccountLogInModel model)
        {
            var ticket = GetFormsAuthenticationTicket(model.EmailAddress, model.RememberMe);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(
                                        FormsAuthentication.FormsCookieName,
                                        encryptedTicket) { Secure = true };
            Response.SetCookie(authCookie);
        }

        [RequiresAuthentication, RequireHttps]
        public ActionResult LogOut()
        {
            if (User != null)
                FormsAuthentication.SignOut();
            if (Request["returnUrl"] != null)
                return Redirect(Request["returnUrl"]);
            return RedirectToAction("Index", "Home");
        }

        private static FormsAuthenticationTicket GetFormsAuthenticationTicket(string userName, bool isPersistent)
        {
            return new FormsAuthenticationTicket(1,
                    userName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(60),
                    isPersistent,
                    String.Empty,
                    FormsAuthentication.FormsCookiePath);
        }

        private static string HashString(string unhashedString)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(unhashedString, FormsAuthPasswordFormat.SHA1.ToString());
        }


        #endregion

        #region Search

        [RequiresAuthentication, HttpGet]
        public ActionResult SearchByName(AccountSearchByNameModel searchModel)
        {
            var search = new UsersSearchCriteria
                             {
                                 NameIsLike = searchModel.term,
                                 DistinctRootEntity = true,
                                 OrderByProperty = "FirstName",
                             };
            var users = AccountService.FindUsers(search);
            if (Request.IsAjaxRequest())
                return Json(users.Select(x => new JQueryAutoCompleteResponseModel(x.Id.ToString(), string.Format("{0} {1}", x.FirstName, x.LastName))),
                        JsonRequestBehavior.AllowGet);
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
