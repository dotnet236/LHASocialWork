using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using LHASocialWork.Core;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Authentication;
using LHASocialWork.Services.Image;
using NHibernate.Validator.Engine;
using System.IO;

namespace LHASocialWork.Controllers
{
    public class BaseController : AsyncController
    {
        protected IAccountService AccountService;
        protected IAuthenticationService AuthenticationService;
        protected IImageService ImageService;

        private User _user;
        public new User User
        {
            get
            {
                if (_user == null && HttpContext != null && HttpContext.User != null)
                    _user = AccountService.GetUserByEmailAddress(HttpContext.User.Identity.Name);
                return _user;
            }
        }

        protected RedirectToRouteResult RedirectToLoginResult()
        {
            return RedirectToRoute("Default", new { action = "login", controller = "accounts", returnUrl = Request.Url == null ? "" : Request.Url.AbsolutePath });
        }

        public BaseController(BaseServiceCollection baseServiceCollection)
        {
            if (baseServiceCollection == null) return;

            AccountService = baseServiceCollection.AccountService;
            AuthenticationService = baseServiceCollection.AuthenticationService;
            ImageService = baseServiceCollection.ImageService;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            TempData["Roles"] = User == null ? new List<SystemRole>() : User.Positions.Select(x => x.Position.SystemRole);
            base.OnActionExecuted(filterContext);
        }
        
        protected void AddValidationResults(InvalidValue[] invalidValues)
        {
            if (invalidValues == null) return;
            foreach (var invalidValue in invalidValues)
                ModelState.AddModelError(invalidValue.PropertyName, invalidValue.Message);
        }

        protected void AddValidationResult(string summaryErrorMessage, InvalidValue[] invalidValues)
        {
            ViewData["Error"] = new ErrorModel { Message = summaryErrorMessage };
            AddValidationResults(invalidValues);
        }

        protected void DisplayError(string errorMessage)
        {
            TempData["Error"] = new ErrorModel { Message = errorMessage };
        }

        protected void DisplayAlert(string alertMessage)
        {
            TempData["Alert"] = new AlertModel { Message = alertMessage };
        }

        protected void DisplayMessage(string message)
        {
            TempData["Message"] = new MessageModel { Message = message };
        }

        public Image CreateImageLocally(IEnumerable<ImageSizes> sizes, IHttpPostedFile postedFile)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                postedFile.FileName.Replace(invalidChar.ToString(), "");

            var fileKey = Guid.NewGuid();
            var directoryName = fileKey.ToString().Replace("-", "");
            var fileName = Path.ChangeExtension(Bootstrapper.LocalImageName, "png");

            foreach (var size in sizes)
            {
                using (var imageStream = ImageService.ResizeImage(postedFile.InputStream, size))
                {
                    var directoryPath = Path.Combine(Bootstrapper.LocalImagesPath, directoryName, size.ToString());
                    if (!Directory.Exists(directoryPath))
                        Directory.CreateDirectory(directoryPath);

                    var filePath = Path.Combine(directoryPath, fileName);
                    using (var file = System.IO.File.Create(filePath))
                        file.Write(imageStream.ToArray(), 0, (int)imageStream.Length);
                }
            }

            return new Image
                       {
                           Description = "Image",
                           FileKey = fileKey,
                           Owner = _user,
                           Status = ImageStatus.Approved,
                           Title = postedFile.FileName
                       };
        }

        public static bool IsEmail(string inputEmail)
        {
            if (inputEmail == null) return false;
            const string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                    @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                    @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            var re = new Regex(strRegex);
            return re.IsMatch(inputEmail);
        }
    }
}
