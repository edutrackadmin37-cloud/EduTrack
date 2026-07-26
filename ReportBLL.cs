// ============================================================
// BLL/ReportBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EduTrack.BLL
{
    public class ReportBLL
    {
        private readonly ReportDAL _reportDAL = new ReportDAL();
        private readonly UserBLL _userBLL = new UserBLL();

        #region Student Report Card

        /// <summary>
        /// Generates a student report card with subject-siloed analytics
        /// </summary>
        /// 
        // Add to ReportBLL.cs

        public Response<SchoolPerformanceOverview> GetSchoolPerformanceOverview()
        {
            // Implement using DAL
            var dal = new ReportDAL();
            var data = dal.GetSchoolPerformanceOverview();
            return data != null ? Response<SchoolPerformanceOverview>.Success(data) : Response<SchoolPerformanceOverview>.Failure("No data");
        }

        public Response<SchoolOverallPerformance> GetSchoolOverallPerformance()
        {
            var dal = new ReportDAL();
            var data = dal.GetSchoolOverallPerformance();
            return data != null ? Response<SchoolOverallPerformance>.Success(data) : Response<SchoolOverallPerformance>.Failure("No data");
        }

        public Response<DepartmentSubjectPerformance> GetDepartmentSubjectPerformance(int subjectId)
        {
            var dal = new ReportDAL();
            var data = dal.GetDepartmentSubjectPerformance(subjectId);
            return data != null ? Response<DepartmentSubjectPerformance>.Success(data) : Response<DepartmentSubjectPerformance>.Failure("No data");
        }

        public Response<GradingConsistencyData> GetDepartmentGradingConsistency(int deptId)
        {
            var dal = new ReportDAL();
            var data = dal.GetDepartmentGradingConsistency(deptId);
            return data != null ? Response<GradingConsistencyData>.Success(data) : Response<GradingConsistencyData>.Failure("No data");
        }

        public Response<List<ProjectApprovalDto>> GetPendingApprovals(int headmasterId)
        {
            var dal = new ProjectDAL();
            var projects = dal.GetAll().Where(p => p.Status == "ProposalSubmitted").ToList();
            var dtoList = projects.Select(p => new ProjectApprovalDto
            {
                ProjectID = p.ProjectID,
                Title = p.Title,
                SubmittedBy = new UserDAL().GetById(p.CreatedBy)?.FullName ?? "Unknown",
                Date = p.CreatedAt,
                Status = p.Status
            }).ToList();
            return Response<List<ProjectApprovalDto>>.Success(dtoList);
        }

        public Response<decimal> GetSchoolAttendanceRate()
        {
            var dal = new ReportDAL();
            var rate = dal.GetSchoolAttendanceRate();
            return Response<decimal>.Success(rate);
        }
        public Response<StudentReportCard> GetStudentReportCard(int studentId, int? subjectId = null, int? academicYearId = null)
        {
            if (studentId <= 0)
                return Response<StudentReportCard>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                var userResp = _userBLL.GetUserById(studentId);
                if (!userResp.IsSuccess || userResp.Data == null)
                    return Response<StudentReportCard>.Failure("Student not found.", "NOT_FOUND");

                StudentReportCard report = new StudentReportCard
                {
                    StudentID = studentId,
                    StudentName = userResp.Data.FullName,
                    GeneratedDate = DateTime.Now
                };

                // Get subject reports
                var subjectReports = _reportDAL.GetStudentSubjectReports(studentId, subjectId, academicYearId);
                foreach (var sr in subjectReports)
                {
                    // Get assignment grade details for this subject
                    if (subjectId.HasValue || sr.SubjectID > 0)
                    {
                        sr.GradeDetails = _reportDAL.GetStudentAssignmentGrades(studentId, sr.SubjectID);
                    }

                    // Get engagement indicators
                    // Need projectId – we'll get from projects the student is part of
                    // For now, we'll leave as empty or fetch later
                    report.SubjectReports.Add(sr);
                }

                // Get attendance summary
                var attendance = _reportDAL.GetParentAttendanceSummary(studentId);
                report.OverallAttendance = attendance.OverallAttendance;

                return Response<StudentReportCard>.Success(report, "Report card generated successfully.");
            }
            catch (Exception ex)
            {
                return Response<StudentReportCard>.Failure($"Error generating report: {ex.Message}", "REPORT_ERROR");
            }
        }

        #endregion

        #region Teacher Performance Report

        /// <summary>
        /// Generates a teacher performance report
        /// </summary>
        public Response<EduTrack.Models.TeacherPerformanceReport> GetTeacherPerformanceReport(int teacherId, int? academicYearId = null)
        {
            if (teacherId <= 0)
                return Response<EduTrack.Models.TeacherPerformanceReport>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var userResp = _userBLL.GetUserById(teacherId);
                if (!userResp.IsSuccess || userResp.Data == null)
                    return Response<EduTrack.Models.TeacherPerformanceReport>.Failure("Teacher not found.", "NOT_FOUND");

                EduTrack.Models.TeacherPerformanceReport report = new EduTrack.Models.TeacherPerformanceReport
                {
                    TeacherID = teacherId,
                    TeacherName = userResp.Data.FullName,
                    Department = userResp.Data.Bio ?? "N/A",
                    GeneratedDate = DateTime.Now
                };

                var subjectPerformances = _reportDAL.GetTeacherSubjectPerformance(teacherId, academicYearId);
                report.SubjectPerformances = subjectPerformances;

                if (subjectPerformances.Any())
                {
                    report.TotalStudents = subjectPerformances.Sum(x => x.StudentCount);
                    report.TotalAssignments = subjectPerformances.Sum(x => x.AssignmentCount);
                    report.AverageClassGrade = subjectPerformances.Average(x => x.AverageGrade);
                    report.SubmissionRate = subjectPerformances.Average(x => x.SubmissionRate);
                    report.OnTimeRate = subjectPerformances.Average(x => x.PassRate);
                }

                return Response<EduTrack.Models.TeacherPerformanceReport>.Success(report, "Teacher performance report generated successfully.");
            }
            catch (Exception ex)
            {
                return Response<EduTrack.Models.TeacherPerformanceReport>.Failure($"Error generating report: {ex.Message}", "REPORT_ERROR");
            }
        }

        #endregion

        #region Departmental Analytics

        /// <summary>
        /// Generates departmental analytics report
        /// </summary>
        public Response<DepartmentAnalyticsReport> GetDepartmentAnalytics(int departmentId, int? academicYearId = null)
        {
            if (departmentId <= 0)
                return Response<DepartmentAnalyticsReport>.Failure("Invalid department ID.", "VALIDATION_ERROR");

            try
            {
                DepartmentAnalyticsReport report = new DepartmentAnalyticsReport
                {
                    DepartmentID = departmentId,
                    GeneratedDate = DateTime.Now
                };

                var subjectAnalytics = _reportDAL.GetDepartmentSubjectAnalytics(departmentId, academicYearId);
                report.SubjectAnalytics = subjectAnalytics;

                if (subjectAnalytics.Any())
                {
                    report.TotalStudents = subjectAnalytics.Sum(x => x.StudentCount);
                    // Teachers count would need additional query – we'll add later
                }

                return Response<DepartmentAnalyticsReport>.Success(report, "Department analytics generated successfully.");
            }
            catch (Exception ex)
            {
                return Response<DepartmentAnalyticsReport>.Failure($"Error generating report: {ex.Message}", "REPORT_ERROR");
            }
        }

        #endregion

        #region Project Portfolio

        /// <summary>
        /// Generates a project portfolio report
        /// </summary>
        public Response<ProjectPortfolioReport> GetProjectPortfolio(int projectId)
        {
            if (projectId <= 0)
                return Response<ProjectPortfolioReport>.Failure("Invalid project ID.", "VALIDATION_ERROR");

            try
            {
                var report = _reportDAL.GetProjectPortfolio(projectId);
                if (report == null || report.ProjectID == 0)
                    return Response<ProjectPortfolioReport>.Failure("Project not found.", "NOT_FOUND");

                // Get team portfolios
                var teams = _reportDAL.GetTeamPortfolios(projectId);
                foreach (var team in teams)
                {
                    // Get members
                    team.Members = _reportDAL.GetTeamMemberContributions(team.TeamID);
                    report.TeamPortfolios.Add(team);
                }

                report.GeneratedDate = DateTime.Now;

                return Response<ProjectPortfolioReport>.Success(report, "Project portfolio generated successfully.");
            }
            catch (Exception ex)
            {
                return Response<ProjectPortfolioReport>.Failure($"Error generating report: {ex.Message}", "REPORT_ERROR");
            }
        }

        #endregion

        #region Parent Report

        /// <summary>
        /// Generates a parent report for a child
        /// </summary>
        public Response<ParentReport> GetParentReport(int childId, int? academicYearId = null)
        {
            if (childId <= 0)
                return Response<ParentReport>.Failure("Invalid child ID.", "VALIDATION_ERROR");

            try
            {
                var userResp = _userBLL.GetUserById(childId);
                if (!userResp.IsSuccess || userResp.Data == null)
                    return Response<ParentReport>.Failure("Child not found.", "NOT_FOUND");

                ParentReport report = new ParentReport
                {
                    ChildID = childId,
                    ChildName = userResp.Data.FullName,
                    GeneratedDate = DateTime.Now
                };

                // Get subject reports
                var subjectReports = _reportDAL.GetStudentSubjectReports(childId, null, academicYearId);
                report.SubjectReports = subjectReports;

                // Get attendance summary
                var attendance = _reportDAL.GetParentAttendanceSummary(childId);
                report.OverallAttendance = attendance.OverallAttendance;
                report.DaysPresent = attendance.Present;
                report.DaysAbsent = attendance.Absent;
                report.DaysLate = attendance.Late;

                // Get engagement (will need projectId)
                // We'll fetch projects and get engagement for each

                return Response<ParentReport>.Success(report, "Parent report generated successfully.");
            }
            catch (Exception ex)
            {
                return Response<ParentReport>.Failure($"Error generating report: {ex.Message}", "REPORT_ERROR");
            }
        }

        #endregion

        #region Export Methods

        /// <summary>
        /// Exports report data to PDF
        /// </summary>
        public Response<byte[]> ExportToPdf(object reportData)
        {
            try
            {
                // In production, use iTextSharp or similar PDF library
                // For now, return a simple HTML as byte array
                string html = GenerateHtmlReport(reportData);
                byte[] bytes = Encoding.UTF8.GetBytes(html);
                return Response<byte[]>.Success(bytes, "PDF exported.");
            }
            catch (Exception ex)
            {
                return Response<byte[]>.Failure($"Export failed: {ex.Message}", "EXPORT_ERROR");
            }
        }

        /// <summary>
        /// Exports report data to Excel
        /// </summary>
        public Response<byte[]> ExportToExcel(object reportData)
        {
            try
            {
                // In production, use EPPlus or ClosedXML
                string html = GenerateHtmlReport(reportData);
                byte[] bytes = Encoding.UTF8.GetBytes(html);
                return Response<byte[]>.Success(bytes, "Excel exported.");
            }
            catch (Exception ex)
            {
                return Response<byte[]>.Failure($"Export failed: {ex.Message}", "EXPORT_ERROR");
            }
        }

        /// <summary>
        /// Exports report data to Word
        /// </summary>
        public Response<byte[]> ExportToWord(object reportData)
        {
            try
            {
                // In production, use DocX or Aspose.Words
                string html = GenerateHtmlReport(reportData);
                byte[] bytes = Encoding.UTF8.GetBytes(html);
                return Response<byte[]>.Success(bytes, "Word exported.");
            }
            catch (Exception ex)
            {
                return Response<byte[]>.Failure($"Export failed: {ex.Message}", "EXPORT_ERROR");
            }
        }

        /// <summary>
        /// Generates HTML report from data
        /// </summary>
        private string GenerateHtmlReport(object reportData)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><head><style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #667eea; }");
            sb.AppendLine("table { width: 100%; border-collapse: collapse; margin: 10px 0; }");
            sb.AppendLine("th { background: linear-gradient(135deg, #667eea, #764ba2); color: white; padding: 8px; text-align: left; }");
            sb.AppendLine("td { padding: 8px; border: 1px solid #ddd; }");
            sb.AppendLine("tr:nth-child(even) { background: #f8f9fa; }");
            sb.AppendLine("</style></head><body>");
            sb.AppendLine($"<h1>EduTrack Report</h1>");
            sb.AppendLine($"<p>Generated: {DateTime.Now:yyyy-MM-dd HH:mm}</p>");
            sb.AppendLine("<hr/>");

            if (reportData is StudentReportCard studentReport)
            {
                sb.AppendLine($"<h2>Student: {studentReport.StudentName}</h2>");
                sb.AppendLine($"<p>Class: {studentReport.ClassName} | Academic Year: {studentReport.AcademicYear}</p>");
                sb.AppendLine($"<p>Attendance: {studentReport.OverallAttendance:P2}</p>");
                sb.AppendLine("<h3>Subject Performance</h3>");
                sb.AppendLine("<table><thead><tr><th>Subject</th><th>Average</th><th>Grade</th><th>Submissions</th><th>Remarks</th></tr></thead><tbody>");
                foreach (var sr in studentReport.SubjectReports)
                {
                    sb.AppendLine($"<tr><td>{sr.SubjectName}</td><td>{sr.AverageGrade:F2}</td><td>{sr.GradeLetter}</td><td>{sr.SubmissionCount}/{sr.AssignmentsCount}</td><td>{sr.Remarks}</td></tr>");
                }
                sb.AppendLine("</tbody></table>");
            }
            else if (reportData is EduTrack.Models.TeacherPerformanceReport teacherReport)
            {
                sb.AppendLine($"<h2>Teacher: {teacherReport.TeacherName}</h2>");
                sb.AppendLine($"<p>Department: {teacherReport.Department}</p>");
                sb.AppendLine($"<p>Total Students: {teacherReport.TotalStudents} | Assignments: {teacherReport.TotalAssignments}</p>");
                sb.AppendLine($"<p>Class Average: {teacherReport.AverageClassGrade:F2} | Pass Rate: {teacherReport.SubmissionRate:P2}</p>");
                sb.AppendLine("<h3>Subject Performance</h3>");
                sb.AppendLine("<table><thead><tr><th>Subject</th><th>Students</th><th>Avg Grade</th><th>Pass Rate</th><th>Submissions</th></tr></thead><tbody>");
                foreach (var sp in teacherReport.SubjectPerformances)
                {
                    sb.AppendLine($"<tr><td>{sp.SubjectName}</td><td>{sp.StudentCount}</td><td>{sp.AverageGrade:F2}</td><td>{sp.PassRate:P2}</td><td>{sp.SubmissionRate:P2}</td></tr>");
                }
                sb.AppendLine("</tbody></table>");
            }
            else
            {
                sb.AppendLine("<p>Report data preview: Unsupported report type.</p>");
                sb.AppendLine($"<pre>{reportData}</pre>");
            }

            sb.AppendLine("<hr/><p><em>EduTrack PBL-LMS System - GES SHS Compliant</em></p>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        #endregion
    }
}