using System;
using LHASocialWork.Models.Templates;

namespace LHASocialWork.Models.Event
{
    public class EventCoordinatorModel 
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DisplayImage ProfileImage { get; set; }
        public DateTime LastStatusChange { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}