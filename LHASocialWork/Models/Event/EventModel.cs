using System;
using System.Web.Mvc;
using LHASocialWork.Entities.Enumerations;
using LHASocialWork.Models.Templates;

namespace LHASocialWork.Models.Event
{
    public class EventModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DisplayImage Flyer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime EndTime { get; set; }
        public EventOccurrence Occurrence { get; set; }
        public PrivacySetting Privacy { get; set; }
        public EventType Type { get; set; }
        public EventMemberStatus MemberStatus { get; set; }
        public EventMemberStatus CoordinatorStatus { get; set; }

        public MvcHtmlString DefaultDateDisplay
        {
            get
            {
                string displayDate;
                var occurrence = Occurrence != EventOccurrence.Once ? Occurrence.ToString() : "";

                if (StartDate != EndDate)
                    displayDate = string.Format("<div class='date'>{0} {1} - {2} {3} </div><div class='time'>{4} to {5} {6}</div>",
                                                new object[] {  StartDate.DayOfWeek,
                                                                StartDate.ToShortDateString(),
                                                                EndDate.DayOfWeek,
                                                                EndDate.ToShortDateString(),
                                                                StartTime.ToShortTimeString(), 
                                                                EndTime.ToShortTimeString(),
                                                                occurrence});
                else
                    displayDate = string.Format("<div class='date'>{0} {1}</div><div class='time'>{2} to {3}</div>",
                                                new object[] {  StartDate.DayOfWeek,
                                                                StartDate.ToShortDateString(),
                                                                StartTime.ToShortTimeString(), 
                                                                EndTime.ToShortTimeString() });

                return new MvcHtmlString(displayDate);
            }
        }
    }
}