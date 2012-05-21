using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Web.Security;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Image;

namespace LHASocialWork.Core
{
    public static class SystemEntitiesStore
    {
        public static User User { get; set; }
        public static Address Address { get; set; }
        public static Image ProfileImage { get; set; }
        public static Image EventFlyerImage { get; set; }
        public static IList<Position> Positions { get; set; }
        public static Event Event { get; set; }

        public static User SystemUserTemplate
        {
            get
            {
                return new User
                           {
                               Email = AccountService.DefaultAdminEmailAddress,
                               FirstName = "System",
                               LastName = "Administrator",
                               Password = FormsAuthentication.HashPasswordForStoringInConfigFile("Vitec4IT", FormsAuthPasswordFormat.SHA1.ToString()),
                               PhoneNumber = 00911892220992,
                               Address = SystemAddressTemplate
                           };
            }
        }
        public static Address SystemAddressTemplate
        {
            get
            {
                return new Address
                           {
                               City = "Dharamsala",
                               Street = "Lha office Temple Road, McLeod Ganj",
                               Province = "Distt. Kangra, Himachal Pradesh",
                               Country = "India",
                               Zip = "176219"
                           };
            }
        }
        public static Image SystemUserProfileImageTemplate
        {
            get
            {
                return new Image
                           {
                               Description = "Default User Profile Image",
                               FileKey = ImageService.DefaultUserProfileImageFileKey,
                               Status = ImageStatus.Approved,
                               Title = "Default User Profile Image",
                           };
            }
        }
        public static Image SystemEventFlyerTemplate
        {
            get
            {
                return new Image
                           {
                               Description = "Default Event Flyer Image",
                               FileKey = ImageService.DefaultEventFlyerFileKey,
                               Status = ImageStatus.Approved,
                               Title = "Default Event Flyer Image",
                           };
            }
        }
        public static Event SystemEventTemplate
        {
            get
            {
                var minDate = Convert.ToDateTime("1/1/1753 12:00:00 AM ");
                return new Event
                           {
                               Name = "System Event - DO NOT DELETE",
                               Description = "System Event Description",
                               StartDate = minDate,
                               StartTime = minDate,
                               EndDate = minDate,
                               EndTime = minDate,
                               EventType = EventType.System,
                               Location = Address,
                               Occurrence = EventOccurrence.Never,
                               Privacy = PrivacySetting.Private,
                               Owner = User
                           };
            }
        }
    }
}