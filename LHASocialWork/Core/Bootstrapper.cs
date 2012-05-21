using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using LHASocialWork.Core.Binding;
using LHASocialWork.Core.Extensions;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;
using LHASocialWork.Repositories;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Account;
using LHASocialWork.Services.Event;
using LHASocialWork.Services.Image;
using LHASocialWork.Services.Position;
using NHibernate;
using StructureMap;
using LHASocialWork.Core.Mapping;

namespace LHASocialWork.Core
{
    public static class Bootstrapper
    {
        public static string ApplicationName = "LHASocialWork";
        public static string ApplicationRootPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string ImagesPath = Path.Combine("Content", "Images");
        public static string LocalImagesPath = Path.Combine(ApplicationRootPath, ImagesPath);
        public static string WebImagesPath = Path.Combine("~/", ImagesPath);
        public const string LocalImageName = "Image";
        public static string SiteTitle = "Volunteer In Tibet";

        public static bool TestMode
        {
            get { return Assembly.GetCallingAssembly().Location.Contains("Test"); }
        }

        public static void Initialize()
        {
            ConfigureStructureMap();
            ConfigureAutoMapper();
            ConfigureModelBinder();
            ConfigureProviderFactories();
            ConfigureNHibernateProfiler();
            ConfigureDefaultDatabaseObjects();
            ConfigureDirectories();
        }

        private static void ConfigureStructureMap()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new ApplicationRegistry()));
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
        }

        private static void ConfigureNHibernateProfiler()
        {
            HibernatingRhinos.Profiler.Appender.NHibernate.NHibernateProfiler.Initialize();
        }

        private static void ConfigureAutoMapper()
        {
            MappingConfiguration.Configure();
            //Disabled until we find an easy way to ignore attribute on a base object.  
            //Mapper.AssertConfigurationIsValid();
        }

        private static void ConfigureModelBinder()
        {
            ModelBinders.Binders.Add(typeof(SearchConditional), new EnumBinder<SearchConditional>(SearchConditional.Equals));
            ModelBinders.Binders.Add(typeof(SystemRole), new EnumBinder<SystemRole>(SystemRole.Member));
            ModelBinders.Binders.Add(typeof(DateTime), new DateTimeBinder());
            ModelBinders.Binders.Add(typeof(IHttpPostedFile), new HttpPostedFileBinder());
            ModelBinders.Binders.Add(typeof(EventOccurrence), new EnumBinder<EventOccurrence>(EventOccurrence.Once));
            ModelBinders.Binders.Add(typeof(EventType), new EnumBinder<EventType>(EventType.Event));
            ModelBinders.Binders.Add(typeof(PrivacySetting), new EnumBinder<PrivacySetting>(PrivacySetting.Public));
        }

        private static void ConfigureProviderFactories()
        {
            ValueProviderFactories.Factories.Add(new JsonValueProviderFactory());
        }

        private static void ConfigureDefaultDatabaseObjects()
        {
            if (TestMode) return;
            var session = ObjectFactory.GetInstance<ISessionFactory>().OpenSession();
            var repository = new BaseRepository(session);
            //Add checks for if owner has somehow changed on system objects
            ConfigureSystemPositions(repository);
            ConfigureSystemAccounts(repository);
            ConfigureSystemImages(repository);
            ConfigureSystemEvents(repository);
        }

        private static void ConfigureSystemPositions(IBaseRepository repository)
        {
            var positionService = new PositionService(repository);
            var systemRoles = EnumExtensions.GetTypedValues<SystemRole>().ToList();
            var positionSearchCriteria = new PositionsSearchCriteria
                                             {
                                                 WithRoles = systemRoles,
                                                 OnlySystemPositions = true
                                             };

            var systemPositions = positionService.FindPositions(positionSearchCriteria);
            foreach (var role1 in systemRoles.Where(role1 => !systemPositions.Any(x => x.SystemRole == role1)))
                positionService.SavePosition(new Position { Name = role1.ToString(), SystemRole = role1, SystemPosition = true });
            SystemEntitiesStore.Positions = positionService.FindPositions(positionSearchCriteria).ToList();
        }

        private static void ConfigureSystemAccounts(IBaseRepository repository)
        {
            var imageService = new ImageService(repository);
            var positionService = new PositionService(repository);
            var accountService = new AccountService(repository, imageService, positionService);

            User adminAccount;

            if (accountService.DefaultSystemAccount == null)
            {
                adminAccount = SystemEntitiesStore.SystemUserTemplate;
                adminAccount.ProfilePicture = imageService.DefaultUserProfileImage ?? SystemEntitiesStore.SystemUserProfileImageTemplate;
                adminAccount.ProfilePicture.Owner = adminAccount;
                if (accountService.SaveUser(adminAccount).InvalidValues.Any())
                    throw new Exception("Default system user was not created successfully");
            }

            adminAccount = accountService.GetUserByEmailAddress(SystemEntitiesStore.SystemUserTemplate.Email);
            SystemEntitiesStore.User = adminAccount;
            SystemEntitiesStore.Address = adminAccount.Address;
            if (!adminAccount.Positions.Any(x => x.Position.SystemRole == SystemRole.Admin))
            {

                var adminPositionSearchCriteria = new PositionsSearchCriteria
                                                 {
                                                     WithRoles = new List<SystemRole> { SystemRole.Admin },
                                                     OnlySystemPositions = true
                                                 };

                var adminPosition = positionService.FindPositions(adminPositionSearchCriteria).First();
                positionService.SaveUserPosition(new UserPosition { Position = adminPosition, User = adminAccount });
            }
        }

        private static void ConfigureSystemImages(IBaseRepository repository)
        {
            var imageService = new ImageService(repository);
            var accountService = new AccountService(repository, imageService, null);
            var defaultAdmin = accountService.DefaultSystemAccount;

            #region User Profile Image

            var defaultUserProfileImage = imageService.DefaultUserProfileImage;
            if (defaultUserProfileImage == null)
            {
                defaultUserProfileImage = SystemEntitiesStore.SystemUserProfileImageTemplate;
                defaultUserProfileImage.Owner = defaultAdmin;
                defaultUserProfileImage = imageService.SaveImage(defaultUserProfileImage);
                if (defaultUserProfileImage.InvalidValues.Any())
                    throw new Exception("Default user profile image was not created successfully");
            }

            SystemEntitiesStore.ProfileImage = imageService.DefaultUserProfileImage;

            #endregion

            #region Event Flyer Image

            var defaultEventFlyerImage = imageService.DefaultEventFlyerImage;
            if (defaultEventFlyerImage == null)
            {
                defaultEventFlyerImage = SystemEntitiesStore.SystemEventFlyerTemplate;
                defaultEventFlyerImage.Owner = defaultAdmin;
                imageService.SaveImage(defaultEventFlyerImage);
                if (defaultEventFlyerImage.InvalidValues.Any())
                    throw new Exception("Default user profile image was not created successfully");
            }

            SystemEntitiesStore.EventFlyerImage = defaultEventFlyerImage;

            #endregion
        }

        private static void ConfigureSystemEvents(IBaseRepository repository)
        {
            var eventService = new EventService(repository, new ImageService(repository));
            var testEvent = eventService.GetEventByName(SystemEntitiesStore.SystemEventTemplate.Name);
            if (testEvent == null)
            {
                eventService.SaveEvent(SystemEntitiesStore.SystemEventTemplate);
                testEvent = eventService.GetEventByName(SystemEntitiesStore.SystemEventTemplate.Name);
            }
            SystemEntitiesStore.Event = testEvent;
        }

        private static void ConfigureDirectories()
        {
            if (!Directory.Exists(LocalImagesPath))
                Directory.CreateDirectory(LocalImagesPath);
        }
    }
}