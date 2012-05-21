using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LHASocialWork.Areas.Admin.Models.Accounts;
using LHASocialWork.Controllers;
using LHASocialWork.Entities;
using LHASocialWork.Models.Account;
using LHASocialWork.Models.Shared;
using LHASocialWork.Repositories.Criteria;
using MvcContrib.Pagination;

namespace LHASocialWork.Areas.Admin.Controllers
{
    public class AccountsController : AdminBaseController
    {
        public AccountsController(BaseServiceCollection baseServiceCollection) : base(baseServiceCollection)
        {
        }

        #region Index

        public ActionResult Index(GridOptionsModel options)
        {
            options.Column = options.Column == "id" ? options.Column = "Email" : options.Column;

            var userSearchCriteria = new UsersSearchCriteria
                                         {
                                             Ascending = options.Ascending,
                                             OrderByProperty = options.Column,
                                         };

            var users = AccountService.FindUsers(userSearchCriteria);
            var usersModel = new AccountsModel
                                 {
                                     Data = Mapper.Map<IEnumerable<User>, IEnumerable<AccountModel>>(users).AsPagination(options.Page, 10),
                                     Options = options
                                 };
            return View(usersModel);
        }

        #endregion

        #region Create Account

        public ActionResult Create()
        {
            return View(new CreateAccountViewModel { FormPostAction = "Create" });
        }

        [HttpPost]
        public ActionResult Create(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var user = Mapper.Map<CreateAccountModel, User>(model);
                if (!AccountService.SaveUser(user).InvalidValues.Any())
                {
                    DisplayMessage("User Created Successfully");
                    return RedirectToAction("Index");
                }
                AddValidationResults(user.InvalidValues);
            }
            var viewModel = Mapper.Map<CreateAccountModel, CreateAccountViewModel>(model);
            viewModel.FormPostAction = "Create";
            return View(viewModel);
        }

        #endregion
    }
}
