using System.Collections.Generic;
using LHASocialWork.Entities;
using LHASocialWork.Repositories.Criteria;

namespace LHASocialWork.Services.Account
{
    public interface IAccountService
    {
        User SaveUser(User user);
        User ValidateUserUniqueness(User user);
        IEnumerable<User> GetUsers();
        IEnumerable<User> FindUsers(UsersSearchCriteria search);
        User GetUserByEmailAddress(string emailAddress);
        User GetUserById(long userId);
    }
}