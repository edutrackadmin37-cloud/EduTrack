// ============================================================
// Models/SchoolReport.cs
// ============================================================
using System;
using System.Collections.Generic;

namespace EduTrack.Models
{
    public class SchoolReport
    {
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public int AcademicYearID { get; set; }
        public int TotalStudents { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalClasses { get; set; }
        public int TotalProjects { get; set; }
        public decimal OverallAverageGrade { get; set; }
        public decimal OverallAttendanceRate { get; set; }
        public decimal OverallPassRate { get; set; }
        public List<ProgrammeSummary> Programmes { get; set; } = new List<ProgrammeSummary>();
        public DateTime GeneratedDate { get; set; }
    }

    public class ProgrammeSummary
    {
        public int ProgrammeID { get; set; }
        public string ProgrammeName { get; set; }
        public int StudentCount { get; set; }
        public decimal AverageGrade { get; set; }
        public decimal PassRate { get; set; }
    }
}