using System.Collections.Generic;
using LHASocialWork.Core.DataAnnotations;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;

namespace LHASocialWork.Areas.Admin.Models.Events
{
    public class EditEventResponseModel : CreateEventModel
    {
        [RequiredComplex]
        public long Id { get; set; }
        public bool FileChanged { get; set; }
        public IHttpPostedFile Flyer { get; set; }
        [RequiredComplex]
        public string Description { get; set; }
        [RequiredComplex]
        public EventOccurrence Occurrence { get; set; }
        [RequiredComplex]
        public PrivacySetting Privacy { get; set; }
        [RequiredComplex]
        public EventType Type { get; set; }

        public IDictionary<string, string> IsValid()
        {
            var errors = new Dictionary<string, string>();
            if (FileChanged && Flyer == null)
                errors.Add("Flyer", "Must upload new picture if changing flyer.");
            return errors;
        }
    }
}