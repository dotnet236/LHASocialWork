using AutoMapper;
using LHASocialWork.Areas.Admin.Models.Accounts;
using LHASocialWork.Entities;

namespace LHASocialWork.Core.Mapping
{
    public static class AdminAccountMappingConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<User, AccountModel>();
        }
    }
}