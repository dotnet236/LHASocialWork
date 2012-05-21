using System.Web.Configuration;
using System.Web.Security;
using AutoMapper;
using LHASocialWork.Entities;
using LHASocialWork.Models.Account;
using LHASocialWork.Models.Templates;
using Address = LHASocialWork.Models.Templates.Address;

namespace LHASocialWork.Core.Mapping
{
    public static class AccountMappingConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<User, SystemInfo>()
                .ForMember(x => x.ConfirmPassword, r => r.Ignore());
            Mapper.CreateMap<User, PersonalInfo>();
            Mapper.CreateMap<Entities.Address, Address>();
            Mapper.CreateMap<Address, Entities.Address>();

            Mapper.CreateMap<SystemInfo, User>()
                .ForMember(x => x.Password, r => r.MapFrom(p => FormsAuthentication.HashPasswordForStoringInConfigFile(p.Password, FormsAuthPasswordFormat.SHA1.ToString())));
            Mapper.CreateMap<PersonalInfo, User>();
            Mapper.CreateMap<Address, Entities.Address>();

            Mapper.CreateMap<User, CreateAccountViewModel>()
                .ForMember(x => x.SystemInfo, r => r.MapFrom(Mapper.Map<User, SystemInfo>))
                .ForMember(x => x.PersonInfo, r => r.MapFrom(Mapper.Map<User, PersonalInfo>))
                .ForMember(x => x.Address, r => r.MapFrom(s => Mapper.Map<Entities.Address, Address>(s.Address)));

            Mapper.CreateMap<CreateAccountModel, CreateAccountViewModel>()
                .ConstructUsing(x => new CreateAccountViewModel())
                .ForMember(x => x.SystemInfo, r => r.MapFrom(s => s.SystemInfo == null ? new SystemInfo() : Mapper.Map<SystemInfo, SystemInfo>(s.SystemInfo)))
                .ForMember(x => x.PersonInfo, r => r.MapFrom(s => s.PersonInfo == null ? new PersonalInfo() : Mapper.Map<PersonalInfo, PersonalInfo>(s.PersonInfo)))
                .ForMember(x => x.Address, r => r.MapFrom(s => s.Address == null ? new Address() : Mapper.Map<Address, Address>(s.Address)));

            Mapper.CreateMap<CreateAccountModel, User>()
                .ConvertUsing(x =>
                                  {
                                      var user = Mapper.Map<PersonalInfo, User>(x.PersonInfo);
                                      user = Mapper.Map(x.SystemInfo, user);
                                      user.Address = Mapper.Map<Address, Entities.Address>(x.Address);
                                      return user;
                                  });
        }
    }
}