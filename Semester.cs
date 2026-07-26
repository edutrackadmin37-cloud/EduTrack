// ============================================================
// Models/Semester.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class Semester
    {
        public int SemesterID { get; set; }
        public int SchoolYearID { get; set; }
        public string SemesterName { get; set; } // e.g., "First Term"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}