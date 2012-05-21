using LHASocialWork.Core.DataAnnotations;
using LHASocialWork.Models.Templates;

namespace LHASocialWork.Areas.Admin.Models.Events
{
    public class CreateEventViewModel : CreateEventModel
    {
        [RequiredComplex]
        public FileUpload Flyer { get; set; }
        [RequiredComplex]
        public InlineLabel Creator { get; set; }
        [RequiredComplex]
        public RichTextArea Description { get; set; }
        [RequiredComplex]
        public DropDown Occurrence { get; set; }
        [RequiredComplex]
        public DropDown Privacy { get; set; }
        [RequiredComplex]
        public DropDown Type { get; set; }
    }
}