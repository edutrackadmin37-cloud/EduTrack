// ============================================================
// Models/SchoolYear.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class SchoolYear
    {
        public int SchoolYearID { get; set; }
        public int SchoolID { get; set; }
        public string YearName { get; set; } // e.g., "2025/2026"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string SchoolName { get; set; }
    }
}