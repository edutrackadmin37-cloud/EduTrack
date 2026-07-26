// ============================================================
// DAL/ReportDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ReportDAL : BaseDAL
    {
        /// <summary>
        /// Gets student report card data per subject (siloed)
        /// </summary>
        public List<SubjectReport> GetStudentSubjectReports(int studentId, int? subjectId = null, int? academicYearId = null)
        {
            List<SubjectReport> list = new List<SubjectReport>();
            using (SqlDataReader r = ExecuteReader("sp_GetStudentSubjectReports",
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@SubjectID", (object)subjectId ?? DBNull.Value),
                new SqlParameter("@AcademicYearID", (object)academicYearId ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(new SubjectReport
                    {
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        SubjectCode = GetValue<string>(r, "SubjectCode"),
                        AverageGrade = GetValue<decimal?>(r, "AverageGrade"),
                        HighestGrade = GetValue<decimal?>(r, "HighestGrade"),
                        LowestGrade = GetValue<decimal?>(r, "LowestGrade"),
                        SubmissionCount = GetValue<int>(r, "SubmissionCount"),
                        AssignmentsCount = GetValue<int>(r, "AssignmentsCount"),
                        OnTimeSubmissions = GetValue<int>(r, "OnTimeSubmissions"),
                        LateSubmissions = GetValue<int>(r, "LateSubmissions"),
                        AttendanceRate = GetValue<decimal?>(r, "AttendanceRate"),
                        OverallPerformance = GetValue<decimal>(r, "OverallPerformance"),
                        GradeLetter = GetValue<string>(r, "GradeLetter"),
                        Remarks = GetValue<string>(r, "Remarks")
                    });
                }
            }
            return list;
        }
        public SchoolPerformanceOverview GetSchoolPerformanceOverview()
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSchoolPerformanceOverview"))
            {
                if (r.Read())
                {
                    return new SchoolPerformanceOverview
                    {
                        OverallAverage = GetValue<decimal>(r, "OverallAverage"),
                        OverallAttendance = GetValue<decimal>(r, "OverallAttendance"),
                        PassRate = GetValue<decimal>(r, "PassRate"),
                        EngagementRate = GetValue<decimal>(r, "EngagementRate"),
                        ProjectsCompleted = GetValue<int>(r, "ProjectsCompleted")
                    };
                }
                return null;
            }
        }

        public SchoolOverallPerformance GetSchoolOverallPerformance()
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSchoolOverallPerformance"))
            {
                if (r.Read())
                {
                    return new SchoolOverallPerformance { OverallScore = GetValue<decimal>(r, "OverallScore") };
                }
                return null;
            }
        }

        public DepartmentSubjectPerformance GetDepartmentSubjectPerformance(int subjectId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetDepartmentSubjectPerformance", new SqlParameter("@SubjectID", subjectId)))
            {
                if (r.Read())
                {
                    return new DepartmentSubjectPerformance
                    {
                        SubjectID = subjectId,
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        AverageGrade = GetValue<decimal>(r, "AverageGrade"),
                        StudentCount = GetValue<int>(r, "StudentCount"),
                        TeacherName = GetValue<string>(r, "TeacherName")
                    };
                }
                return null;
            }
        }

        public GradingConsistencyData GetDepartmentGradingConsistency(int deptId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetDepartmentGradingConsistency", new SqlParameter("@DepartmentID", deptId)))
            {
                if (r.Read())
                {
                    return new GradingConsistencyData
                    {
                        Variation = GetValue<decimal>(r, "Variation"),
                        HighestGrade = GetValue<decimal>(r, "HighestGrade"),
                        LowestGrade = GetValue<decimal>(r, "LowestGrade"),
                        ConsistencyScore = GetValue<decimal>(r, "ConsistencyScore")
                    };
                }
                return null;
            }
        }

        public decimal GetSchoolAttendanceRate()
        {
            object result = ExecuteScalar("sp_GetSchoolAttendanceRate");
            return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
        }
        /// <summary>
        /// Gets assignment grade details for a student
        /// </summary>
        public List<AssignmentGradeDetail> GetStudentAssignmentGrades(int studentId, int subjectId)
        {
            List<AssignmentGradeDetail> list = new List<AssignmentGradeDetail>();
            using (SqlDataReader r = ExecuteReader("sp_GetStudentAssignmentGrades",
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@SubjectID", subjectId)))
            {
                while (r.Read())
                {
                    list.Add(new AssignmentGradeDetail
                    {
                        AssignmentTitle = GetValue<string>(r, "AssignmentTitle"),
                        DueDate = GetValue<DateTime?>(r, "DueDate"),
                        SubmissionDate = GetValue<DateTime?>(r, "SubmissionDate"),
                        GradeValue = GetValue<decimal?>(r, "GradeValue"),
                        Status = GetValue<string>(r, "Status"),
                        Remarks = GetValue<string>(r, "Remarks")
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Gets engagement indicators for a student per week
        /// </summary>
        public List<EngagementIndicator> GetStudentEngagement(int studentId, int projectId)
        {
            List<EngagementIndicator> list = new List<EngagementIndicator>();
            using (SqlDataReader r = ExecuteReader("sp_GetStudentEngagement",
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(new EngagementIndicator
                    {
                        WeekNumber = GetValue<int>(r, "WeekNumber"),
                        Participation = GetValue<bool>(r, "Participation"),
                        Questioning = GetValue<bool>(r, "Questioning"),
                        ProblemSolving = GetValue<bool>(r, "ProblemSolving"),
                        Collaboration = GetValue<bool>(r, "Collaboration"),
                        TaskCompletion = GetValue<bool>(r, "TaskCompletion"),
                        Motivation = GetValue<bool>(r, "Motivation"),
                        EngagementScore = GetValue<decimal>(r, "EngagementScore")
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Gets teacher performance report data
        /// </summary>
        public List<SubjectPerformance> GetTeacherSubjectPerformance(int teacherId, int? academicYearId = null)
        {
            List<SubjectPerformance> list = new List<SubjectPerformance>();
            using (SqlDataReader r = ExecuteReader("sp_GetTeacherSubjectPerformance",
                new SqlParameter("@TeacherID", teacherId),
                new SqlParameter("@AcademicYearID", (object)academicYearId ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(new SubjectPerformance
                    {
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        StudentCount = GetValue<int>(r, "StudentCount"),
                        AverageGrade = GetValue<decimal>(r, "AverageGrade"),
                        HighestGrade = GetValue<decimal>(r, "HighestGrade"),
                        LowestGrade = GetValue<decimal>(r, "LowestGrade"),
                        AssignmentCount = GetValue<int>(r, "AssignmentCount"),
                        SubmissionRate = GetValue<decimal>(r, "SubmissionRate"),
                        PassRate = GetValue<decimal>(r, "PassRate")
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Gets department analytics
        /// </summary>
        public List<SubjectAnalytics> GetDepartmentSubjectAnalytics(int departmentId, int? academicYearId = null)
        {
            List<SubjectAnalytics> list = new List<SubjectAnalytics>();
            using (SqlDataReader r = ExecuteReader("sp_GetDepartmentSubjectAnalytics",
                new SqlParameter("@DepartmentID", departmentId),
                new SqlParameter("@AcademicYearID", (object)academicYearId ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(new SubjectAnalytics
                    {
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        StudentCount = GetValue<int>(r, "StudentCount"),
                        AverageGrade = GetValue<decimal>(r, "AverageGrade"),
                        HighestGrade = GetValue<decimal>(r, "HighestGrade"),
                        LowestGrade = GetValue<decimal>(r, "LowestGrade"),
                        PassRate = GetValue<decimal>(r, "PassRate"),
                        AssignmentCount = GetValue<int>(r, "AssignmentCount"),
                        SubmissionRate = GetValue<decimal>(r, "SubmissionRate"),
                        EngagementScore = GetValue<decimal>(r, "EngagementScore"),
                        TrendDirection = GetValue<string>(r, "TrendDirection")
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Gets project portfolio data
        /// </summary>
        public ProjectPortfolioReport GetProjectPortfolio(int projectId)
        {
            ProjectPortfolioReport report = new ProjectPortfolioReport();
            using (SqlDataReader r = ExecuteReader("sp_GetProjectPortfolio", new SqlParameter("@ProjectID", projectId)))
            {
                if (r.Read())
                {
                    report.ProjectID = GetValue<int>(r, "ProjectID");
                    report.ProjectTitle = GetValue<string>(r, "Title");
                    report.ProjectStatus = GetValue<string>(r, "Status");
                    report.SubjectName = GetValue<string>(r, "SubjectName");
                    report.ClassName = GetValue<string>(r, "ClassName");
                    report.TeacherName = GetValue<string>(r, "TeacherName");
                    report.StartDate = GetValue<DateTime?>(r, "StartDate");
                    report.EndDate = GetValue<DateTime?>(r, "EndDate");
                    report.TotalTeams = GetValue<int>(r, "TotalTeams");
                    report.TotalStudents = GetValue<int>(r, "TotalStudents");
                    report.OverallTeamAverage = GetValue<decimal>(r, "OverallTeamAverage");
                    report.OverallIndividualAverage = GetValue<decimal>(r, "OverallIndividualAverage");
                }
            }
            return report;
        }

        /// <summary>
        /// Gets team portfolio for a project
        /// </summary>
        public List<TeamPortfolio> GetTeamPortfolios(int projectId)
        {
            List<TeamPortfolio> list = new List<TeamPortfolio>();
            using (SqlDataReader r = ExecuteReader("sp_GetTeamPortfolios", new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(new TeamPortfolio
                    {
                        TeamID = GetValue<int>(r, "TeamID"),
                        TeamName = GetValue<string>(r, "TeamName"),
                        MemberCount = GetValue<int>(r, "MemberCount"),
                        TeamScore = GetValue<decimal?>(r, "TeamScore"),
                        AssessmentComments = GetValue<string>(r, "Comments"),
                        AverageIndividualScore = GetValue<decimal>(r, "AverageIndividualScore"),
                        WeightedTeamScore = GetValue<decimal>(r, "WeightedTeamScore")
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Gets team members with contributions
        /// </summary>
        public List<MemberContribution> GetTeamMemberContributions(int teamId)
        {
            List<MemberContribution> list = new List<MemberContribution>();
            using (SqlDataReader r = ExecuteReader("sp_GetTeamMemberContributions", new SqlParameter("@TeamID", teamId)))
            {
                while (r.Read())
                {
                    list.Add(new MemberContribution
                    {
                        StudentID = GetValue<int>(r, "StudentID"),
                        StudentName = GetValue<string>(r, "StudentName"),
                        IndividualScore = GetValue<decimal>(r, "IndividualScore"),
                        Feedback = GetValue<string>(r, "Feedback"),
                        GradeLetter = GetValue<string>(r, "GradeLetter")
                    });
                }
            }
            return list;
        }

        /// <summary>
        /// Gets parent report data for a child
        /// </summary>
        public List<SubjectReport> GetParentReportSubjects(int studentId, int? academicYearId = null)
        {
            // Reuse student subject reports for parent
            return GetStudentSubjectReports(studentId, null, academicYearId);
        }

        /// <summary>
        /// Gets parent attendance summary
        /// </summary>
        public (decimal OverallAttendance, int Present, int Absent, int Late) GetParentAttendanceSummary(int studentId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetParentAttendanceSummary", new SqlParameter("@StudentID", studentId)))
            {
                if (r.Read())
                {
                    return (
                        GetValue<decimal>(r, "AttendanceRate"),
                        GetValue<int>(r, "PresentCount"),
                        GetValue<int>(r, "AbsentCount"),
                        GetValue<int>(r, "LateCount")
                    );
                }
            }
            return (0, 0, 0, 0);
        }
    }
}