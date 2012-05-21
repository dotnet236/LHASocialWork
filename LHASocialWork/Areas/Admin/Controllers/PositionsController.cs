using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LHASocialWork.Areas.Admin.Models.Positions;
using LHASocialWork.Controllers;
using LHASocialWork.Core.Extensions;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Position;
using MvcContrib.Pagination;

namespace LHASocialWork.Areas.Admin.Controllers
{
    public class PositionsController : AdminBaseController
    {
        private readonly IPositionService _positionService;

        private static CreatePositionViewModel DefaultCreatePositionViewModel
        {
            get
            {
                return new CreatePositionViewModel
                           {
                               SystemRoles = EnumExtensions.GetTypedValuesAsSelectList<SystemRole>()
                           };
            }
        }

        public PositionsController (BaseServiceCollection baseServiceCollection, IPositionService positionService) : base(baseServiceCollection)
        {
            _positionService = positionService;
        }

        #region Index

        [HttpGet]
        public ActionResult Index(GridOptionsModel options)
        {
            options.Column = options.Column == "Id" ? "Name" : options.Column;
            var searchCriteria = new PositionsSearchCriteria
                                     {
                                         Ascending = options.Ascending,
                                         OrderByProperty = options.Column
                                     };

            var positions = _positionService.FindPositions(searchCriteria);
            var model = new PositionsModel
                            {
                                Data = Mapper.Map<IEnumerable<Position>, IEnumerable<PositionModel>>(positions).AsPagination(options.Page, 10),
                                Options = options
                            };
            return View(model);
        }

        #endregion

        #region View Position

        public ActionResult Position(GridOptionsModel options)
        {
            if(!options.id.HasValue)
            {
                DisplayError("No position specified.");
                return View(new PositionViewModel {Data = new List<UserPositionModel>().AsPagination(options.Page, 10), Position = new PositionModel()});
            }

            var searchCriteria = new UserPositionsSearchCriteria
                                     {
                                         Ascending = options.Ascending,
                                         DistinctRootEntity = true,
                                         OrderByProperty = options.Column,
                                         Positions = new[] {options.id.Value}
                                     };
            var userPositions = _positionService.FindUserPositions(searchCriteria);
            var model = new PositionViewModel
                            {
                                Position = Mapper.Map<Position, PositionModel>(!userPositions.Any()
                                                                            ? _positionService.GetPosition(options.id.Value)
                                                                            : userPositions.First().Position),
                                Data = Mapper.Map<IEnumerable<UserPosition>, IEnumerable<UserPositionModel>>(userPositions).AsPagination(options.Page, 10)
                            };

            return View(model);
        }

        #endregion

        #region Create Position

        [HttpGet]
        public ActionResult Create()
        {
            return View(DefaultCreatePositionViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Create(CreatePositionResponseModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var position = Mapper.Map<CreatePositionResponseModel, Position>(viewModel);
                position = _positionService.SavePosition(position);
                if (position.InvalidValues.Length == 0)
                {
                    DisplayMessage("Position Created Successfully");
                    return RedirectToAction("Index");
                }
                AddValidationResults(position.InvalidValues);
            }
            return View(viewModel);
        }

        #endregion

        #region Create User Position

        [HttpGet]
        public ActionResult CreateUserPosition(int id)
        {
            var position = _positionService.GetPosition(id);
            var model = new CreateUserPositionViewModel {Name = position.Name, PositionId = position.Id};
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CreateUserPosition(CreateUserPositionModel model)
        {
            var position = _positionService.GetPosition(model.PositionId);

            if(ModelState.IsValid)
            {
                var existingUsers = position.Users;
                var userSearch = new UsersSearchCriteria  { DistinctRootEntity = true, WithId = model.UserIds };
                var users = AccountService.FindUsers(userSearch).Where(x => !existingUsers.Any(r => x.Id == r.User.Id));
                var userPositions = users.Select(x => new UserPosition {Position = position, User = x});
                userPositions = _positionService.SaveUserPositions(userPositions);
                if(!userPositions.Any(x => x.InvalidValues.Any()))
                {
                    DisplayMessage("User positions created successfully");
                    return RedirectToAction("Position", "Positions", new { id = model.PositionId });
                }
                AddValidationResults(userPositions.Where(x => x.InvalidValues.Count() > 0).First().InvalidValues);
                DisplayError("User positions were not created successfully");
            }

            return View(new CreateUserPositionViewModel {Name = position.Name, PositionId = model.PositionId});
        }

        #endregion

        #region Delete User Position

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult DeleteUserPosition(long id)
        {
            var userPosition = _positionService.GetUserPosition(id);
            userPosition = _positionService.DeleteUserPosition(userPosition);
            if(userPosition.InvalidValues.Any())
                foreach (var invalidValue in userPosition.InvalidValues)
                    DisplayError(invalidValue.Message);
            else
                DisplayMessage("User deleted successfully");
            return RedirectToAction("Position", new {id = userPosition.Position.Id});
        }

        #endregion
    }
}
