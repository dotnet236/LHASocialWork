using LHASocialWork.Models.Templates;

namespace LHASocialWork.Models.Event
{
    public class ThumbnailDisplayModel
    {
        public object Id { get; set; }
        public string Name { get; set; }
        public DisplayImage Thumbnail { get; set; }
    }
}