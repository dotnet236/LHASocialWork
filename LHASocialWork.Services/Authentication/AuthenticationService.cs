using System;
using System.Linq;
using LHASocialWork.Entities;
using LHASocialWork.Repositories;
using LHASocialWork.Services.Base;
using NHibernate.Criterion;

namespace LHASocialWork.Services.Authentication
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        public AuthenticationService(IBaseRepository repository) : base(repository){}

        public int MinPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public bool ValidateUser(string emailAddress, string password)
        {
            var criteria = DetachedCriteria.For<User>()
                .Add(Restrictions.Eq("Email", emailAddress))
                .Add(Restrictions.Eq("Password", password));
            return Repository.List<User>(criteria).Any();
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }
    }
}