using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Data;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Position;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;

namespace LHASocialWork.Tests.Services
{
    [TestClass]
    public class PositionServiceTest : BaseServiceTest
    {
        private IPositionService _positionService;

        #region List Positions

        #region Setup

        private void SetupGetPositions()
        {
            MockBaseRepository.Setup(x => x.List<Position>(DetachedCriteria.For<Position>())).Returns(new List<Position>());
            _positionService = new PositionService(MockBaseRepository.Object);
        }

        #endregion

        [TestMethod]
        public void GetPositions()
        {
            SetupGetPositions();
            var positions = _positionService.GetPositions();
            Assert.IsNotNull(positions, "Positions list should not be null");
        }

        #endregion

        #region Create Position

        private Position _validPosition;
        private Position _invalidPosition;

        #region Setup

        private void SetupCreatePositionSuccess()
        {
            _validPosition = new Position
                                 {
                                     Name = "TestPosition",
                                     SystemRole = SystemRole.Member,
                                 };

            _validPosition.AddInvalidValues(new NHibernate.Validator.Engine.InvalidValue[0]);

            MockBaseRepository.Setup(x => x.SaveOrUpdate(_validPosition, null)).Returns(_validPosition);
            MockBaseRepository.Setup(x => x.List<Position>(DetachedCriteria.For<Position>())).Returns(new List<Position>());
            _positionService = new PositionService(MockBaseRepository.Object);
        }

        private void SetupCreatePositionFailure()
        {
            _invalidPosition = new Position
                                 {
                                     SystemRole = SystemRole.Member,
                                 };

            MockBaseRepository.Setup(x => x.List<Position>(DetachedCriteria.For<Position>())).Returns(new List<Position>());
            _positionService = new PositionService(MockBaseRepository.Object);
        }

        #endregion

        [TestMethod]
        public void CreatePositionSuccess()
        {
            SetupCreatePositionSuccess();
            ValidateSuccessfulEntityCreation(_positionService.SavePosition(_validPosition));
        }

        [TestMethod]
        public void CreatePositionFailure()
        {
            SetupCreatePositionFailure();
            var position = _positionService.SavePosition(_invalidPosition);
            ValidateUnsuccessfulEntityCreation(position);
        }

        #endregion

        #region Create User Position

        private readonly UserPosition _validUserPosition = new UserPosition
                                                               {
                                                                   Position = new Position(),
                                                                   User = new User()
                                                               };

        private readonly UserPosition _invalidUserPosition = new UserPosition
                                                               {
                                                                   User = new User()
                                                               };

        #region Setup

        private void SetupCreateUserPositionSuccess()
        {
            MockBaseRepository.Setup(x => x.SaveOrUpdate(It.IsAny<IList<UserPosition>>(), null)).Returns(new List<UserPosition> { _validUserPosition });
            _positionService = new PositionService(MockBaseRepository.Object);
        }

        private void SetupCreateUserPositionFailure()
        {
            MockBaseRepository.Setup(x => x.SaveOrUpdate(It.IsAny<IList<UserPosition>>(), null)).Returns(new List<UserPosition> { _invalidUserPosition });
            _positionService = new PositionService(MockBaseRepository.Object);
        }

        #endregion

        [TestMethod]
        public void CreateUserPositionSuccess()
        {
            SetupCreateUserPositionSuccess();
            ValidateSuccessfulEntityCreation(_positionService.SaveUserPositions(new List<UserPosition> { _validUserPosition }).First());
        }

        [TestMethod]
        public void CreateUserPositionFailure()
        {
            SetupCreateUserPositionFailure();
            ValidateUnsuccessfulEntityCreation(_positionService.SaveUserPositions(new List<UserPosition> { _invalidUserPosition }).First());
        }

        #endregion

        #region List User Positions

        #region Setup

        private void SetupFindUserPositions()
        {
            MockBaseRepository.Setup(x => x.List<UserPosition>(It.IsAny<DetachedCriteria>())).Returns(new List<UserPosition>());
            _positionService = new PositionService(MockBaseRepository.Object);
        }

        #endregion

        [TestMethod]
        public void FindUserPositions()
        {
            SetupFindUserPositions();
            var userPositions = _positionService.FindPositions(new PositionsSearchCriteria());
            Assert.IsNotNull(userPositions, "User positions list should not be null");
        }

        #endregion

        #region Delete User Position

        #region

        private void SetupDeleteUserPosition()
        {
            MockBaseRepository.Setup(x => x.Delete(It.IsAny<Entity>(), null));
            _positionService = new PositionService(MockBaseRepository.Object);
        }

        #endregion

        [TestMethod]
        public void ShouldDeleteNonSystemUserPosition()
        {
            SetupDeleteUserPosition();
            var userPosition = new UserPosition { User = new User { SystemUser = false }, Position = new Position { SystemPosition = false } };
            _positionService.DeleteUserPosition(userPosition);
            Assert.IsFalse(_positionService.DeleteUserPosition(userPosition).InvalidValues.Any(), "Should of deleted non system user position");
        }

        [TestMethod]
        public void ShouldNotDeleteSystemUserPosition()
        {
            SetupDeleteUserPosition();
            var userPosition = new UserPosition { User = new User { SystemUser = true }, Position = new Position { SystemPosition = true } };
            Assert.IsTrue(_positionService.DeleteUserPosition(userPosition).InvalidValues.Any(), "Should not of deleted system user position");
        }

        #endregion

    }
}
