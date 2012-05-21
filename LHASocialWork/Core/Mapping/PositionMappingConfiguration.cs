using AutoMapper;
using LHASocialWork.Areas.Admin.Models.Positions;
using LHASocialWork.Entities;

namespace LHASocialWork.Core.Mapping
{
    public static class PositionMappingConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<CreatePositionResponseModel, Position>();
            Mapper.CreateMap<Position, CreatePositionResponseModel>();//For PositionControllerTest Cases
            Mapper.CreateMap<Position, PositionModel>()
                .ForMember(x => x.AccountCount, r => r.MapFrom(p => p.Users.Count));
            Mapper.CreateMap<UserPosition, UserPositionModel>()
                .ForMember(x => x.Name, r => r.MapFrom(p => string.Format("{0} {1}", p.User.FirstName, p.User.LastName)))
                .ForMember(x => x.Email, r => r.MapFrom(p => p.User.Email))
                .ForMember(x => x.PhoneNumber, r => r.MapFrom(p => p.User.PhoneNumber));
        } 
    }
}