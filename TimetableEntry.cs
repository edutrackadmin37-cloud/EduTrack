// ============================================================
// Models/TimetableEntry.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class TimetableEntry
    {
        public int EntryID { get; set; }
        public int TimetableID { get; set; }
        public int PeriodNumber { get; set; }
        public DateTime Date { get; set; }
        public string Activity { get; set; } // e.g., "Mathematics", "Break"
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}