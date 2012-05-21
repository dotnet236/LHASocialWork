using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LHASocialWork.Areas.Admin.Models.Events;
using LHASocialWork.Controllers;
using LHASocialWork.Core.Extensions;
using LHASocialWork.Entities;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Event;
using LHASocialWork.Models.Shared;
using LHASocialWork.Models.Templates;
using LHASocialWork.Repositories.Criteria;
using LHASocialWork.Services.Event;
using MvcContrib.Pagination;
using Address = LHASocialWork.Models.Templates.Address;

namespace LHASocialWork.Areas.Admin.Controllers
{
    public class EventsController : AdminBaseController
    {
        private readonly IEventService _eventService;

        public EventsController(BaseServiceCollection baseServiceCollection, IEventService eventService)
            : base(baseServiceCollection)
        {
            _eventService = eventService;
        }

        #region Index

        [HttpGet]
        public ActionResult Index(GridOptionsModel options)
        {
            options.Column = options.Column == "Id" ? "Name" : options.Column;
            var searchCriteria = new EventsSearchCriteria
                                     {
                                         OnlyCurrent = false,//true,
                                         OrderByProperty = options.Column,
                                         Ascending = options.Ascending
                                     };

            var events = _eventService.FindEvents(searchCriteria);
            var model = new EventsModel
                            {
                                Data = Mapper.Map<IEnumerable<Event>, IEnumerable<EventModel>>(events).AsPagination(options.Page, 10),
                                Options = options
                            };
            return View(model);
        }

        #endregion

        #region View Event

        public ActionResult Event(int id)
        {
            var evt = _eventService.GetEventById(id);
            if (evt == null)
            {
                DisplayError("Event not found.");
                return RedirectToAction("Index");
            }

            var model = Mapper.Map<Event, EventModel>(evt);

            model.Flyer.ImageSize = ImageSizes.W180xL200;
            return View(model);
        }

        #endregion

        #region Create Event

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateEventViewModel
                            {
                                Address = Address.DefaultAddress,
                                Flyer = new FileUpload { PropertyName = "Flyer" },
                                DateTime = new ComplexDateTime { PropertyName = "DateTime" },
                                Description = new RichTextArea { PropertyName = "Description", PropertyValue = "" },
                                Creator = new InlineLabel { Value = string.Format("{0} {1}", User.FirstName, User.LastName) },
                                Occurrence = new DropDown { PropertyName = "Occurrence", Items = EnumExtensions.GetTypedValuesAsSelectList<EventOccurrence>().ToList() },
                                Privacy = new DropDown { PropertyName = "Privacy", Items = EnumExtensions.GetTypedValuesAsSelectList<PrivacySetting>().ToList() },
                                Type = new DropDown { PropertyName = "Type", Items = EnumExtensions.GetTypedValuesAsSelectList<EventType>().ToList() }
                            };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Create(CreateEventResponseModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Flyer.ContentLength > 5000000)
                    DisplayError("Flyer is to large, please user a smaller image.");
                else
                {

                    var evt = Mapper.Map<CreateEventResponseModel, Event>(model);
                    evt.Owner = User;
                    evt.Flyer = CreateImageLocally(Entities.Event.FlyerSizes, model.Flyer);
                    evt.Location = Mapper.Map<Address, Entities.Address>(model.Address);
                    evt = _eventService.SaveEvent(evt);

                    if (evt.IsValid)
                    {
                        DisplayMessage("Event created successfully.");
                        return RedirectToAction("Index");
                    }
                    AddValidationResults(evt.InvalidValues);
                    DisplayError("Event was not created successfully.");
                }
            }

            return View(MapToCreatEventViewModel(model));
        }

        private CreateEventViewModel MapToCreatEventViewModel(CreateEventResponseModel responseModel)
        {
            var occurrenceOptions = EnumExtensions.GetTypedValuesAsSelectList<EventOccurrence>().ToList();
            occurrenceOptions.FirstOrDefault(x =>
                                        {
                                            EventOccurrence value;
                                            return !Enum.TryParse(x.Value.ToString(), out value) || value != responseModel.Occurrence;
                                        }).Selected = true;

            var privacyOptions = EnumExtensions.GetTypedValuesAsSelectList<PrivacySetting>().ToList();
            privacyOptions.FirstOrDefault(x =>
                                        {
                                            PrivacySetting value;
                                            return !Enum.TryParse(x.Value.ToString(), out value) || value != responseModel.Privacy;
                                        }).Selected = true;
            var typeOptions = EnumExtensions.GetTypedValuesAsSelectList<EventType>().ToList();
            typeOptions.FirstOrDefault(x =>
                                           {
                                               EventType value;
                                               return !Enum.TryParse(x.Value.ToString(), out value) || value != responseModel.Type;
                                           }).Selected = true;
            var viewModel = Mapper.Map<CreateEventResponseModel, CreateEventViewModel>(responseModel);
            viewModel.Creator = new InlineLabel { Value = string.Format("{0} {1}", User.FirstName, User.LastName) };
            viewModel.Occurrence = new DropDown { PropertyName = "Occurrence", Items = occurrenceOptions };
            viewModel.Privacy = new DropDown { PropertyName = "Privacy", Items = privacyOptions };
            viewModel.Description = new RichTextArea { PropertyName = "Description", PropertyValue = responseModel.Description };
            viewModel.Flyer = new FileUpload { PropertyName = "Flyer" };
            viewModel.Type = new DropDown {PropertyName = "Type", Items = typeOptions};

            return viewModel;
        }

        #endregion

        #region Edit Event

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var evt = _eventService.GetEventById(id);
            var model = Mapper.Map<Event, EditEventViewModel>(evt);
            model.Creator = new InlineLabel { Value = string.Format("{0} {1}", User.FirstName, User.LastName) };
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public ActionResult Edit(EditEventResponseModel model)
        {
            var extendedValidationResults = model.IsValid();
            var oldEvent = _eventService.GetEventById(model.Id);
            if(!extendedValidationResults.Any() && ModelState.IsValid)
            {
                var newEvent = Mapper.Map(model, oldEvent);
                if (model.FileChanged)
                    newEvent.Flyer = CreateImageLocally(Entities.Event.FlyerSizes, model.Flyer);
                newEvent = _eventService.SaveEvent(newEvent);
                if(!newEvent.InvalidValues.Any())
                {
                    DisplayMessage("Event updated successfully.");
                    return RedirectToAction("Event", new {id = model.Id});
                }
                DisplayError("Event updated failed.");
                AddValidationResults(newEvent.InvalidValues);
            }

            foreach(var error in extendedValidationResults)
                ModelState.AddModelError(error.Key, error.Value);

            var viewModel = Mapper.Map<EditEventResponseModel, EditEventViewModel>(model);
            viewModel.Flyer = new ImageFileUpload { PropertyName = "Flyer", Image = new DisplayImage(oldEvent.Flyer.FileKey, oldEvent.Flyer.Title, Entities.Event.ThumbnailSize) };
            return View(viewModel);
        }

        #endregion

    }
}
