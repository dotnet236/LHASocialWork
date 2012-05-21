using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using LHASocialWork.Areas.Admin.Controllers;
using LHASocialWork.Areas.Admin.Models.Positions;
using LHASocialWork.Controllers;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Position;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace LHASocialWork.Tests.Controllers
{
    [TestClass]
    public class AdminPositionControllerTest : BaseControllerTest
    {
        private PositionsController _positionsController;

        #region Index

        #region HttpGet

        #region Setup
        
        private void SetupGetPositions()
        {
            var service = new Mock<IPositionService>();
            service.Setup(x => x.GetPositions()).Returns(new List<Position>());
            _positionsController = new PositionsController(null, service.Object);
        }

        #endregion

        [TestMethod]
        public void GetPositions()
        {
            SetupGetPositions();
            var response = _positionsController.Index(new GridOptionsModel());
            ValidateViewResult<PositionsModel, PositionsController>(response, true, "", "admin");
        }

        #endregion

        #endregion

        #region Create Position

        private readonly Position _validPosition = new Position()
                                              {
                                                  Name = "TestPositionName",
                                                  SystemRole = SystemRole.Admin
                                              };

        private readonly Position _invalidPosition = new Position()
                                              {
                                                  SystemRole = SystemRole.Admin
                                              };

        #region HttpGet

        #region Setup

        private void SetupGetCreatePosition()
        {
            _positionsController = new PositionsController(null, null);
        }

        #endregion

        [TestMethod]
        public void GetCreatePosition()
        {
            SetupGetCreatePosition();
            var response = _positionsController.Create();
            ValidateViewResult<CreatePositionViewModel, PositionsController>(response, true, "Create", "admin");
        }

        #endregion

        #region HttpPost

        private void SetupPostCreatePositionSuccess()
        {
            var service = new Mock<IPositionService>();
            service.Setup(x => x.SavePosition(_validPosition)).Returns(_validPosition);
            _positionsController = new PositionsController(null, service.Object);
        }

        private void SetupPostCreatePositionFailure()
        {
            _positionsController = new PositionsController(null, null);
        }

        [TestMethod]
        public void PostCreatePositionSuccess()
        {
            SetupPostCreatePositionSuccess();
            var validModel = Mapper.Map<Position, CreatePositionResponseModel>(_validPosition);
            var response = _positionsController.Create(validModel) as RedirectToRouteResult;
            ValidateRouteOfRedirectResponse(response, "Index");
        }

        [TestMethod]
        public void PostCreatePositionFailure()
        {
            SetupPostCreatePositionFailure();
            var invalidModel = Mapper.Map<Position, CreatePositionResponseModel>(_invalidPosition);
            var response = _positionsController.CallWithModelValidation(x => x.Create(invalidModel), invalidModel) as ViewResult;
            ValidateModelStateOfUnsuccessfulPostResponse(response);
        }


        #endregion

        #endregion

        #region View Position

        #region Setup
        
        private void SetupReadPosition()
        {
            var service = new Mock<IPositionService>();
            service.Setup(x => x.FindUserPositions(It.IsAny<UserPositionsSearchCriteria>())).Returns(
                new List<UserPosition> {new UserPosition { User = new User(), Position = new Position() }});
            _positionsController = new PositionsController(null, service.Object);
        }

        #endregion

        #region HttpGet

        [TestMethod]
        public void ReadPosition()
        {
            SetupReadPosition();
            var result = _positionsController.Position(new GridOptionsModel {id = 2});
            ValidateViewResult<PositionViewModel, PositionsController>(result, true, "position", "admin");
        }

        #endregion

        #endregion

        #region Create User Position

        private readonly CreateUserPositionModel _validCreateUserPositionModel = new CreateUserPositionModel
                                                                            {
                                                                                PositionId = 1,
                                                                                UserIds = new long[] { 1 }
                                                                            };

        private readonly CreateUserPositionModel _invalidCreateUserPositionModel = new CreateUserPositionModel
                                                                            {
                                                                                PositionId = 1
                                                                            };
        #region HttpGet

        #region Setup

        private void SetupGetCreateUserPosition()
        {
            var position = new Mock<Position>();
            position.Setup(x => x.Id).Returns(1);
            position.Setup(x => x.Name).Returns("TestPosition");
            var service = new Mock<IPositionService>();
            service.Setup(x => x.GetPosition(It.IsAny<long>())).Returns(position.Object);
            _positionsController = new PositionsController(null, service.Object);
        }

        #endregion

        [TestMethod]
        public void GetCreateUserPosition()
        {
            SetupGetCreateUserPosition();
            var result = _positionsController.CreateUserPosition(6);
            ValidateViewResult<CreateUserPositionViewModel, PositionsController>(result, true, "CreateUserPosition", "admin", new { id = 6 });
        }

        #endregion

        #region HttpPost

        #region Setup

        private void SetupPostCreateUserPositionSuccess()
        {
            var accountService = new Mock<IAccountService>();
            var positionService = new Mock<IPositionService>();
            accountService.Setup(x => x.FindUsers(It.IsAny<UsersSearchCriteria>())).Returns(new List<User>());
            positionService.Setup(x => x.SaveUserPositions(It.IsAny<IEnumerable<UserPosition>>())).Returns(new List<UserPosition> { new UserPosition() });
            positionService.Setup(x => x.GetPosition(It.IsAny<long>())).Returns(new Position {Name = "", Id = 0});
            _positionsController = new PositionsController(new BaseServiceCollection(accountService.Object, null, null), positionService.Object);
        }

        private void SetupPostCreateUserPositionFailure()
        {
            var positionService = new Mock<IPositionService>();
            positionService.Setup(x => x.GetPosition(It.IsAny<long>())).Returns(new Position {Name = "", Id = 0});
            _positionsController = new PositionsController(null, positionService.Object);
        }

        #endregion

        [TestMethod]
        public void PostCreateUserPositionSuccess()
        {
            SetupPostCreateUserPositionSuccess();
            var response = _positionsController.CallWithModelValidation(x => x.CreateUserPosition(_validCreateUserPositionModel), _validCreateUserPositionModel);
            ValidateRouteOfRedirectResponse(response, "Position");
        }

        [TestMethod]
        public void PostCreateuserPositionFailure()
        {
            SetupPostCreateUserPositionFailure();
            var response = _positionsController.CallWithModelValidation(x => x.CreateUserPosition(_invalidCreateUserPositionModel), _invalidCreateUserPositionModel);
            ValidateModelStateOfUnsuccessfulPostResponse(response);
        }

        #endregion

        #endregion

        #region Delete User Position

        #region Setup

        private void SetupDeleteUserPosition()
        {
            var service = new Mock<IPositionService>();
            service.Setup(x => x.GetUserPosition(It.IsAny<long>())).Returns(new UserPosition());
            service.Setup(x => x.DeleteUserPosition(It.IsAny<UserPosition>()));
            _positionsController = new PositionsController(null, service.Object);
        }

        #endregion

        public void DeleteUserPosition()
        {
            SetupDeleteUserPosition();
            var result = _positionsController.DeleteUserPosition(5);

        }

        #endregion
    }
}
