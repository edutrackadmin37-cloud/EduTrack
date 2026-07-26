// ============================================================
// Models/ReportCard.cs
// ============================================================
using System;
using System.Collections.Generic;

namespace EduTrack.Models
{
    public class ReportCard
    {
        public int ReportCardID { get; set; }
        public int StudentID { get; set; }
        public int AcademicYearID { get; set; }
        public int ClassID { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string GeneratedBy { get; set; } // UserID or name
        public string ReportType { get; set; } // e.g., "Term1", "Final"
        public string FilePath { get; set; } // Path to generated PDF/Word/Excel
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public List<SubjectReport> SubjectReports { get; set; } = new List<SubjectReport>();
    }

    // Note: SubjectReport is already defined in ReportModels.cs, so remove this duplicate definition.
    // Keep only the ReportCard class above.
}