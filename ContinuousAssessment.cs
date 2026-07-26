// ============================================================
// Models/ContinuousAssessment.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class ContinuousAssessment
    {
        public int ContinuousAssessmentID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public int ClassID { get; set; }
        public int AcademicYearID { get; set; }
        public decimal CA1 { get; set; } // First continuous assessment
        public decimal CA2 { get; set; }
        public decimal CA3 { get; set; }
        public decimal CA4 { get; set; }
        public decimal TotalCA { get; set; } // Calculated
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string StudentName { get; set; }
        public string SubjectName { get; set; }
    }
}