// ============================================================
// Models/SubjectSiloedPerformance.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    /// <summary>
    /// Represents a student's performance for a specific subject (siloed).
    /// No cross-subject aggregation.
    /// </summary>
    public class SubjectSiloedPerformance
    {
        public int PerformanceID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public int ClassID { get; set; }
        public int AcademicYearID { get; set; }
        public decimal AverageGrade { get; set; }
        public decimal HighestGrade { get; set; }
        public decimal LowestGrade { get; set; }
        public int AssignmentsCompleted { get; set; }
        public int TestsTaken { get; set; }
        public decimal AttendanceRate { get; set; }
        public decimal EngagementScore { get; set; }
        public string GradeLetter { get; set; }
        public string Remarks { get; set; }
        public DateTime CalculatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public string SubjectName { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string AcademicYearName { get; set; }
    }
}