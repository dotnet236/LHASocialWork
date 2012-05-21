using System;
using LHASocialWork.Core.DataAnnotations;

namespace LHASocialWork.Models.Templates
{
    public class ComplexDateTime : EditorTemplate
    {
        public ComplexDateTime()
        {
            StartDate = DateTime.Now.AddDays(1);
            StartTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, 12, 0, 0);
            EndDate = StartDate.AddDays(1);
            EndTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 13, 0, 0);
        }

        [RequiredComplex]
        public DateTime StartDate { get; set; }
        [RequiredComplex]
        public DateTime StartTime { get; set; }
        [RequiredComplex]
        public DateTime EndDate { get; set; }
        [RequiredComplex]
        public DateTime EndTime { get; set; }
    }
}