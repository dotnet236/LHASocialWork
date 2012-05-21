using System.Collections.Generic;
using LHASocialWork.Models.Shared;

namespace LHASocialWork.Models.Account
{
    public class AccountSearchByNameModel : SearchModel
    {
        public AccountSearchByNameModel() : base("Name")
        {
            term = "";
        }
    }
}