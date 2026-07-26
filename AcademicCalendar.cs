// ============================================================
// Models/AcademicCalendar.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class AcademicCalendar
    {
        public int CalendarID { get; set; }
        public int SchoolID { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EventType { get; set; } // e.g., Holiday, Exam, Break
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string SchoolName { get; set; }
    }
}