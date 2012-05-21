using System;
using System.Collections.Generic;
using System.Linq;
using LHASocialWork.Entities;
using LHASocialWork.Repositories;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Base;
using LHASocialWork.Services.Image;
using LHASocialWork.Services.Position;
using NHibernate.Criterion;
using NHibernate.Validator.Engine;

namespace LHASocialWork.Services.Account
{
    public class AccountService : BaseService, IAccountService
    {
        public static readonly string DefaultAdminEmailAddress = "office@lhasocialwork.org";
        public User DefaultSystemAccount
        {
            get { return GetUserByEmailAddress(DefaultAdminEmailAddress); }
        }

        private readonly IImageService _imageService;
        private readonly IPositionService _positionService;

        public AccountService(IBaseRepository repository, IImageService imageService, IPositionService positionService)
            : base(repository)
        {
            _imageService = imageService;
            _positionService = positionService;
        }

        public User SaveUser(User user)
        {
            if (user.Positions.Count() == 0)
                user.Positions.Add(new UserPosition { Position = _positionService.MemberPosition, User = user });
            if (user.ProfilePicture == null)
                user.ProfilePicture = _imageService.DefaultUserProfileImage;
            return ValidateUserUniqueness(user).InvalidValues.Any() ? user : ValidateAndSave(user);
        }

        public User ValidateUserUniqueness(User user)
        {
            var criteria = DetachedCriteria.For<User>().Add(Restrictions.Eq("Email", user.Email));
            if (Repository.List<User>(criteria).Any())
                user.AddInvalidValue(new InvalidValue("Email address already exists", typeof(User), "Email", user.Email, user, null));
            return user;
        }

        public IEnumerable<User> GetUsers()
        {
            var criteria = DetachedCriteria.For<User>();
            return Repository.List<User>(criteria);
        }

        public IEnumerable<User> FindUsers(UsersSearchCriteria search)
        {
            return Repository.List<User>(search.BuildCriteria());
        }

        public User GetUserByEmailAddress(string emailAddress)
        {
            var usersSearchCriteria = new UsersSearchCriteria
                                          {
                                              WithEmailAddresses = new[] {  emailAddress }
                                          };
            return Repository.Get<User>(usersSearchCriteria.BuildCriteria());
        }

        public User GetUserById(long userId)
        {
            return Repository.Get<User>(userId);
        }
    }
}
