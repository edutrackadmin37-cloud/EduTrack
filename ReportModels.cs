// ============================================================
// Models/ReportModels.cs – Consolidated (all report models)
// ============================================================
using System;
using System.Collections.Generic;

namespace EduTrack.Models
{
    /// <summary>
    /// Student Report Card – PER SUBJECT (siloed)
    /// </summary>
    public class StudentReportCard
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string ClassName { get; set; }
        public string AcademicYear { get; set; }
        public List<SubjectReport> SubjectReports { get; set; } = new List<SubjectReport>();
        public decimal OverallAttendance { get; set; }
        public DateTime GeneratedDate { get; set; }
    }

    public class SubjectReport
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public decimal? AverageGrade { get; set; }
        public decimal? HighestGrade { get; set; }
        public decimal? LowestGrade { get; set; }
        public int SubmissionCount { get; set; }
        public int AssignmentsCount { get; set; }
        public int OnTimeSubmissions { get; set; }
        public int LateSubmissions { get; set; }
        public decimal? AttendanceRate { get; set; }
        public List<AssignmentGradeDetail> GradeDetails { get; set; } = new List<AssignmentGradeDetail>();
        public List<EngagementIndicator> EngagementIndicators { get; set; } = new List<EngagementIndicator>();
        public decimal OverallPerformance { get; set; }
        public string GradeLetter { get; set; }
        public string Remarks { get; set; }
    }

    public class AssignmentGradeDetail
    {
        public string AssignmentTitle { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public decimal? GradeValue { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }

    public class EngagementIndicator
    {
        public int WeekNumber { get; set; }
        public bool Participation { get; set; }
        public bool Questioning { get; set; }
        public bool ProblemSolving { get; set; }
        public bool Collaboration { get; set; }
        public bool TaskCompletion { get; set; }
        public bool Motivation { get; set; }
        public decimal EngagementScore { get; set; }
    }

    /// <summary>
    /// Teacher Performance Report – PER SUBJECT (siloed)
    /// </summary>
    public class TeacherPerformanceReport
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string Department { get; set; }
        public int TotalStudents { get; set; }
        public int TotalAssignments { get; set; }
        public decimal AverageClassGrade { get; set; }
        public decimal SubmissionRate { get; set; }
        public decimal OnTimeRate { get; set; }
        public List<SubjectPerformance> SubjectPerformances { get; set; } = new List<SubjectPerformance>();
        public DateTime GeneratedDate { get; set; }
    }

    public class SubjectPerformance
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int StudentCount { get; set; }
        public decimal AverageGrade { get; set; }
        public decimal HighestGrade { get; set; }
        public decimal LowestGrade { get; set; }
        public int AssignmentCount { get; set; }
        public decimal SubmissionRate { get; set; }
        public decimal PassRate { get; set; }
        public List<StudentGradeSummary> StudentSummaries { get; set; } = new List<StudentGradeSummary>();
    }

    public class StudentGradeSummary
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public decimal AverageGrade { get; set; }
        public string GradeLetter { get; set; }
        public int Submissions { get; set; }
    }

    /// <summary>
    /// Departmental Analytics – PER DEPARTMENT (siloed by subject)
    /// </summary>
    public class DepartmentAnalyticsReport
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalProgrammes { get; set; }
        public List<ProgrammeAnalytics> ProgrammeAnalytics { get; set; } = new List<ProgrammeAnalytics>();
        public List<SubjectAnalytics> SubjectAnalytics { get; set; } = new List<SubjectAnalytics>();
        public DateTime GeneratedDate { get; set; }
    }

    public class ProgrammeAnalytics
    {
        public int ProgrammeID { get; set; }
        public string ProgrammeName { get; set; }
        public int StudentCount { get; set; }
        public decimal OverallAverage { get; set; }
        public decimal PassRate { get; set; }
        public List<SubjectAnalytics> Subjects { get; set; } = new List<SubjectAnalytics>();
    }

    public class SubjectAnalytics
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public int StudentCount { get; set; }
        public decimal AverageGrade { get; set; }
        public decimal HighestGrade { get; set; }
        public decimal LowestGrade { get; set; }
        public decimal PassRate { get; set; }
        public int AssignmentCount { get; set; }
        public decimal SubmissionRate { get; set; }
        public decimal EngagementScore { get; set; }
        public string TrendDirection { get; set; } // Up, Down, Stable
    }

    /// <summary>
    /// Project Portfolio Report
    /// </summary>
    public class ProjectPortfolioReport
    {
        public int ProjectID { get; set; }
        public string ProjectTitle { get; set; }
        public string ProjectStatus { get; set; }
        public string SubjectName { get; set; }
        public string ClassName { get; set; }
        public string TeacherName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int TotalTeams { get; set; }
        public int TotalStudents { get; set; }
        public List<TeamPortfolio> TeamPortfolios { get; set; } = new List<TeamPortfolio>();
        public decimal OverallTeamAverage { get; set; }
        public decimal OverallIndividualAverage { get; set; }
        public DateTime GeneratedDate { get; set; }
    }
    public class SchoolPerformanceOverview
    {
        public decimal OverallAverage { get; set; }
        public decimal OverallAttendance { get; set; }
        public decimal PassRate { get; set; }
        public decimal EngagementRate { get; set; }
        public int ProjectsCompleted { get; set; }
    }

    public class SchoolOverallPerformance
    {
        public decimal OverallScore { get; set; }
    }

    public class DepartmentSubjectPerformance
    {
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public decimal AverageGrade { get; set; }
        public int StudentCount { get; set; }
        public string TeacherName { get; set; }
    }

    public class GradingConsistencyData
    {
        public decimal Variation { get; set; }
        public decimal HighestGrade { get; set; }
        public decimal LowestGrade { get; set; }
        public decimal ConsistencyScore { get; set; }
    }

    public class ProjectApprovalDto
    {
        public int ProjectID { get; set; }
        public string Title { get; set; }
        public string SubmittedBy { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
    public class TeamPortfolio
    {
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public int MemberCount { get; set; }
        public decimal? TeamScore { get; set; }
        public string AssessmentComments { get; set; }
        public List<MemberContribution> Members { get; set; } = new List<MemberContribution>();
        public decimal AverageIndividualScore { get; set; }
        public decimal WeightedTeamScore { get; set; }
    }

    public class MemberContribution
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public decimal IndividualScore { get; set; }
        public string Feedback { get; set; }
        public string GradeLetter { get; set; }
    }

    /// <summary>
    /// Parent Report – Consolidated view of child's performance
    /// </summary>
    public class ParentReport
    {
        public int ChildID { get; set; }
        public string ChildName { get; set; }
        public string ClassName { get; set; }
        public string AcademicYear { get; set; }
        public List<SubjectReport> SubjectReports { get; set; } = new List<SubjectReport>();
        public decimal OverallAttendance { get; set; }
        public int DaysPresent { get; set; }
        public int DaysAbsent { get; set; }
        public int DaysLate { get; set; }
        public decimal AverageEngagementScore { get; set; }
        public List<EngagementIndicator> RecentEngagement { get; set; } = new List<EngagementIndicator>();
        public List<ProjectSummary> Projects { get; set; } = new List<ProjectSummary>();
        public DateTime GeneratedDate { get; set; }
    }

    public class ProjectSummary
    {
        public string ProjectTitle { get; set; }
        public string Status { get; set; }
        public decimal? TeamScore { get; set; }
        public decimal? IndividualScore { get; set; }
        public string TeamName { get; set; }
    }

    /// <summary>
    /// Report Parameter Model
    /// </summary>
    public class ReportParameters
    {
        public int? UserID { get; set; }
        public int? SubjectID { get; set; }
        public int? ClassID { get; set; }
        public int? ProgrammeID { get; set; }
        public int? DepartmentID { get; set; }
        public int? ProjectID { get; set; }
        public int? AcademicYearID { get; set; }
        public int? WeekNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ReportType { get; set; }
        public bool IncludeCharts { get; set; } = true;
        public bool IncludeSummary { get; set; } = true;
    }

    /// <summary>
    /// Grade helper for GES SHS grading system
    /// </summary>
    public static class GradeHelper
    {
        public static string GetGradeLetter(decimal score)
        {
            if (score >= 90) return "A1";
            if (score >= 80) return "B2";
            if (score >= 70) return "B3";
            if (score >= 60) return "C4";
            if (score >= 55) return "C5";
            if (score >= 50) return "C6";
            if (score >= 40) return "D7";
            if (score >= 30) return "E8";
            return "F9";
        }

        public static string GetRemark(decimal score)
        {
            if (score >= 90) return "Excellent";
            if (score >= 80) return "Very Good";
            if (score >= 70) return "Good";
            if (score >= 60) return "Credit";
            if (score >= 50) return "Pass";
            if (score >= 40) return "Weak Pass";
            if (score >= 30) return "Fail";
            return "Poor";
        }
    }
}