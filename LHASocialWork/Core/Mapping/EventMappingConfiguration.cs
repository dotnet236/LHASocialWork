using System.Linq;
using System.Web;
using AutoMapper;
using LHASocialWork.Areas.Admin.Models.Events;
using LHASocialWork.Core.Extensions;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Event;
using LHASocialWork.Models.Templates;
using Microsoft.Security.Application;


namespace LHASocialWork.Core.Mapping
{
    public static class EventMappingConfiguration
    {
        public static void Configure()
        {
            Mapper.CreateMap<CreateEventResponseModel, Event>()
                .ForMember(x => x.Name, r => r.MapFrom(s => s.Name.Replace(' ', '_')))
                .ForMember(x => x.Description, r => r.MapFrom(s => HttpUtility.HtmlEncode(AntiXss.GetSafeHtml(s.Description))))
                .ForMember(x => x.StartDate, r => r.MapFrom(s => s.DateTime.StartDate))
                .ForMember(x => x.StartTime, r => r.MapFrom(s => s.DateTime.StartTime))
                .ForMember(x => x.EndDate, r => r.MapFrom(s => s.DateTime.EndDate))
                .ForMember(x => x.EndTime, r => r.MapFrom(s => s.DateTime.EndTime))
                .ForMember(x => x.Flyer, r => r.Ignore());

            Mapper.CreateMap<CreateEventResponseModel, CreateEventViewModel>()
                .ForMember(x => x.Name, r => r.MapFrom(s => s.Name.Replace('_', ' ')))
                .ForMember(x => x.Description, r => r.Ignore())
                .ForMember(x => x.Flyer, r => r.Ignore())
                .ForMember(x => x.Occurrence, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Occurrence",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<EventOccurrence>(s.Privacy.ToString()).ToList(),
                }))
                .ForMember(x => x.Privacy, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Privacy",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<PrivacySetting>(s.Occurrence.ToString()).ToList()
                }))
                .ForMember(x => x.Type, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Type",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<EventType>(s.Type.ToString()).ToList()
                }));

            Mapper.CreateMap<EditEventResponseModel, Event>()
                .ForMember(x => x.Name, r => r.MapFrom(s => s.Name.Replace(' ', '_')))
                .ForMember(x => x.Description, r => r.MapFrom(s => HttpUtility.HtmlEncode(AntiXss.GetSafeHtml(s.Description))))
                .ForMember(x => x.StartDate, r => r.MapFrom(s => s.DateTime.StartDate))
                .ForMember(x => x.StartTime, r => r.MapFrom(s => s.DateTime.StartTime))
                .ForMember(x => x.EndDate, r => r.MapFrom(s => s.DateTime.EndDate))
                .ForMember(x => x.EndTime, r => r.MapFrom(s => s.DateTime.EndTime))
                .ForMember(x => x.EventType, r => r.MapFrom(s => s.Type))
                .ForMember(x => x.Flyer, r => r.Ignore());

            Mapper.CreateMap<Event, ComplexDateTime>();

            Mapper.CreateMap<Event, EditEventViewModel>()
                .ForMember(x => x.Name, r => r.MapFrom(s => s.DisplayName))
                .ForMember(x => x.DateTime, r => r.MapFrom(Mapper.Map<Event, ComplexDateTime>))
                .ForMember(x => x.Description, r => r.MapFrom(s => new RichTextArea { PropertyName = "Description", PropertyValue = HttpUtility.HtmlDecode(s.Description) }))
                .ForMember(x => x.Flyer, r => r.MapFrom(s => new ImageFileUpload { PropertyName = "Flyer", Image = new DisplayImage(s.Flyer.FileKey, s.Flyer.Title) { ImageSize = Event.ThumbnailSize } }))
                .ForMember(x => x.Creator, r => r.Ignore())
                .ForMember(x => x.Address, r => r.MapFrom(s => s.Location))
                .ForMember(x => x.Occurrence, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Occurrence",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<EventOccurrence>(s.Privacy.ToString()).ToList(),
                }))
                .ForMember(x => x.Privacy, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Privacy",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<PrivacySetting>(s.Occurrence.ToString()).ToList()
                }))
                .ForMember(x => x.Type, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Type",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<EventType>(s.EventType.ToString()).ToList()
                }));
            Mapper.CreateMap<EditEventResponseModel, EditEventViewModel>()
                .ForMember(x => x.Name, r => r.MapFrom(s => s.Name.Replace('_', ' ')))
                .ForMember(x => x.Description, r => r.MapFrom(s => new RichTextArea { PropertyName = "Description", PropertyValue = HttpUtility.HtmlDecode(s.Description) }))
                .ForMember(x => x.Creator, r => r.Ignore())
                .ForMember(x => x.Occurrence, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Occurrence",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<EventOccurrence>(s.Privacy.ToString()).ToList(),
                }))
                .ForMember(x => x.Privacy, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Privacy",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<PrivacySetting>(s.Occurrence.ToString()).ToList()
                }))
                .ForMember(x => x.Type, r => r.MapFrom(s => new DropDown
                {
                    PropertyName = "Type",
                    Items = EnumExtensions.GetTypedValuesAsSelectList<EventType>(s.Type.ToString()).ToList()
                }));

            Mapper.CreateMap<Event, EventModel>()
                .ForMember(x => x.Name, r => r.MapFrom(s => s.DisplayName))
                .ForMember(x => x.Type, r => r.MapFrom(s => s.EventType))
                .ForMember(x => x.Flyer, r => r.MapFrom(s => new DisplayImage(s.Flyer.FileKey, s.Flyer.Title) { ImageSize = Event.ThumbnailSize }))
                .ForMember(x => x.Description, r => r.MapFrom(s => HttpUtility.HtmlDecode(s.Description)));

            Mapper.CreateMap<User, InviteEventUserModel>()
                .ForMember(x => x.ProfilePicture, r => r.MapFrom(s => new DisplayImage(s.ProfilePicture.FileKey, s.ProfilePicture.Title)));

            Mapper.CreateMap<EventMember, EventMemberModel>()
                .ForMember(x => x.ProfileImage, r => r.MapFrom(s =>
                                                                   {
                                                                       var image = s.User == null ? SystemEntitiesStore.ProfileImage : s.User.ProfilePicture;
                                                                       return new DisplayImage(image.FileKey,
                                                                                               image.Title,
                                                                                               User.ThumbnailSize);
                                                                   }))
                .ForMember(x => x.Name, r => r.MapFrom(s =>
                                                           {
                                                               if (s.User != null)
                                                                   return s.User.FirstName + s.User.LastName;
                                                               return s.EmailAddress;
                                                           }));

            Mapper.CreateMap<EventCoordinator, EventCoordinatorModel>()
                .ForMember(x => x.StartDate, r => r.MapFrom(s => s.StartDate))
                .ForMember(x => x.ProfileImage, r => r.MapFrom(s =>
                                                                   {
                                                                       var image = s.Coordinator.ProfilePicture;
                                                                       return new DisplayImage(image.FileKey,
                                                                                               image.Title,
                                                                                               User.ThumbnailSize);
                                                                   }))
                .ForMember(x => x.Name, r => r.MapFrom(s => s.Coordinator.FirstName + s.Coordinator.LastName));
            Mapper.CreateMap<User, ThumbnailDisplayModel>()
                .ForMember(x => x.Name, r => r.MapFrom(s => string.Format("{0} {1}", s.FirstName, s.LastName)))
                .ForMember(x => x.Thumbnail, r => r.MapFrom(s =>
                                                                {
                                                                    var image = s.ProfilePicture;
                                                                    return new DisplayImage(image.FileKey,
                                                                                            image.Title,
                                                                                            User.ThumbnailSize);
                                                                }));
        }
    }
}