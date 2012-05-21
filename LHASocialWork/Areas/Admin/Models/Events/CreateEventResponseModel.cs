using LHASocialWork.Core.DataAnnotations;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Shared;

namespace LHASocialWork.Areas.Admin.Models.Events
{
    public class CreateEventResponseModel : CreateEventModel
    {
        [RequiredComplex]
        public IHttpPostedFile Flyer { get; set; }
        [RequiredComplex]
        public string Description { get; set; }
        [RequiredComplex]
        public EventOccurrence Occurrence { get; set; }
        [RequiredComplex]
        public PrivacySetting Privacy { get; set; }
        [RequiredComplex]
        public EventType Type { get; set; }
    }
}