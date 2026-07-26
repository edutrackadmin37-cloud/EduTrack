using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class ParentBLL
    {
        private readonly ParentStudentMapDAL _mapDAL = new ParentStudentMapDAL();
        private readonly UserDAL _userDAL = new UserDAL();
        private readonly ClassStudentDAL _classStudentDAL = new ClassStudentDAL();
        private readonly ClassDAL _classDAL = new ClassDAL();
        private readonly GradeDAL _gradeDAL = new GradeDAL();
        private readonly AttendanceDAL _attendanceDAL = new AttendanceDAL();
        private readonly NotificationDAL _notificationDAL = new NotificationDAL();

        public Response<List<ChildSummary>> GetChildren(int parentId)
        {
            if (parentId <= 0)
                return Response<List<ChildSummary>>.Failure("Invalid parent ID.", "VALIDATION_ERROR");

            try
            {
                var maps = _mapDAL.GetChildrenForParent(parentId);
                var children = new List<ChildSummary>();
                foreach (var m in maps)
                {
                    var student = _userDAL.GetById(m.StudentID);
                    if (student == null) continue;
                    // Get class
                    var enrollment = _classStudentDAL.GetByStudent(student.UserID).FirstOrDefault(cs => cs.IsActive && !cs.IsDeleted);
                    var cls = enrollment != null ? _classDAL.GetById(enrollment.ClassID) : null;
                    // Get attendance rate
                    var attendance = _attendanceDAL.GetAttendanceByClassStudent(enrollment?.ClassStudentID ?? 0);
                    decimal rate = 0;
                    if (attendance.Any())
                    {
                        var present = attendance.Count(a => a.Status == "Present");
                        rate = (decimal)present / attendance.Count * 100;
                    }
                    // Average grade
                    var grades = _gradeDAL.GetAll().Where(g => g.StudentID == student.UserID).ToList();
                    var avg = grades.Any() ? grades.Average(g => g.GradeValue ?? 0) : 0;

                    children.Add(new ChildSummary
                    {
                        UserID = student.UserID,
                        FullName = student.FullName,
                        Email = student.Email,
                        ClassName = cls?.ClassName ?? "Not enrolled",
                        AverageGrade = avg,
                        AttendanceRate = rate
                    });
                }
                return Response<List<ChildSummary>>.Success(children);
            }
            catch (Exception ex)
            {
                return Response<List<ChildSummary>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> IsChildOfParent(int childId, int parentId)
        {
            if (childId <= 0 || parentId <= 0)
                return Response<bool>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var maps = _mapDAL.GetChildrenForParent(parentId);
                var result = maps.Any(m => m.StudentID == childId);
                return Response<bool>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ChildInfo> GetChildInfo(int childId)
        {
            if (childId <= 0)
                return Response<ChildInfo>.Failure("Invalid child ID.", "VALIDATION_ERROR");

            try
            {
                var student = _userDAL.GetById(childId);
                if (student == null) return Response<ChildInfo>.Failure("Child not found.", "NOT_FOUND");
                var enrollment = _classStudentDAL.GetByStudent(childId).FirstOrDefault(cs => cs.IsActive && !cs.IsDeleted);
                var cls = enrollment != null ? _classDAL.GetById(enrollment.ClassID) : null;
                var attendance = _attendanceDAL.GetAttendanceByClassStudent(enrollment?.ClassStudentID ?? 0);
                decimal rate = 0;
                if (attendance.Any())
                {
                    var present = attendance.Count(a => a.Status == "Present");
                    rate = (decimal)present / attendance.Count * 100;
                }
                var grades = _gradeDAL.GetAll().Where(g => g.StudentID == childId).ToList();
                var avg = grades.Any() ? grades.Average(g => g.GradeValue ?? 0) : 0;

                return Response<ChildInfo>.Success(new ChildInfo
                {
                    UserID = student.UserID,
                    FullName = student.FullName,
                    ClassName = cls?.ClassName ?? "Not enrolled",
                    AttendanceRate = rate,
                    OverallAverage = avg
                });
            }
            catch (Exception ex)
            {
                return Response<ChildInfo>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<SubjectPerformance>> GetChildSubjectPerformance(int childId)
        {
            if (childId <= 0)
                return Response<List<SubjectPerformance>>.Failure("Invalid child ID.", "VALIDATION_ERROR");

            try
            {
                var list = new List<SubjectPerformance>();
                var grades = _gradeDAL.GetAll().Where(g => g.StudentID == childId).ToList();
                var submissionToSubject = new Dictionary<int, int>();
                var subjectDAL = new SubjectDAL();
                var allSubjects = subjectDAL.GetAll();

                // You need a way to map SubmissionID to SubjectID. 
                // If you have a SubmissionDAL or similar, use it here.
                // For now, let's assume Grade has a SubjectID property (if not, you need to provide this mapping).
                // If not available, you need to provide more info.

                // Group by SubjectID (assuming you can get it from Grade or via mapping)
                var grouped = grades.GroupBy(g =>
                {
                    // If Grade has SubjectID property, use it directly:
                    // return g.SubjectID;

                    // Otherwise, you need to map SubmissionID to SubjectID here.
                    // For now, return 0 as a fallback.
                    return 0;
                });

                foreach (var group in grouped)
                {
                    var subject = allSubjects.FirstOrDefault(s => s.SubjectID == group.Key);
                    if (subject == null) continue;
                    var avg = group.Average(g => g.GradeValue ?? 0);
                    list.Add(new SubjectPerformance
                    {
                        SubjectID = subject.SubjectID,
                        SubjectName = subject.SubjectName,
                        SubjectCode = subject.SubjectCode,
                        AverageGrade = avg,
                        AssignmentsCompleted = group.Count(),
                        TestsTaken = 0,
                        AttendanceRate = 100 // placeholder
                    });
                }
                return Response<List<SubjectPerformance>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<SubjectPerformance>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ParentStats> GetParentStats(int parentId)
        {
            if (parentId <= 0)
                return Response<ParentStats>.Failure("Invalid parent ID.", "VALIDATION_ERROR");

            try
            {
                var children = GetChildren(parentId);
                if (!children.IsSuccess) return Response<ParentStats>.Failure(children.Message, children.ErrorCode);
                var avgGrade = children.Data.Any() ? children.Data.Average(c => c.AverageGrade) : 0;
                var avgAttendance = children.Data.Any() ? children.Data.Average(c => c.AttendanceRate) : 0;
                var notifications = _notificationDAL.GetByUser(parentId).Count(n => !n.IsRead);
                return Response<ParentStats>.Success(new ParentStats
                {
                    ChildrenCount = children.Data.Count,
                    AverageGrade = avgGrade,
                    AttendanceRate = avgAttendance,
                    NotificationCount = notifications
                });
            }
            catch (Exception ex)
            {
                return Response<ParentStats>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<string> GenerateReport(int childId, string reportType)
        {
            // Generate report as HTML string; simple implementation
            var child = GetChildInfo(childId);
            if (!child.IsSuccess) return Response<string>.Failure(child.Message, child.ErrorCode);
            var subjects = GetChildSubjectPerformance(childId);
            var html = $"<h2>Report for {child.Data.FullName}</h2><p>Class: {child.Data.ClassName}</p><p>Attendance: {child.Data.AttendanceRate:F0}%</p>";
            html += "<ul>";
            foreach (var s in subjects.Data)
            {
                html += $"<li>{s.SubjectName}: {s.AverageGrade:F1}%</li>";
            }
            html += "</ul>";
            return Response<string>.Success(html);
        }

        // Helper DTOs
        public class ChildSummary
        {
            public int UserID { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string ClassName { get; set; }
            public decimal AverageGrade { get; set; }
            public decimal AttendanceRate { get; set; }
        }

        public class ChildInfo
        {
            public int UserID { get; set; }
            public string FullName { get; set; }
            public string ClassName { get; set; }
            public decimal AttendanceRate { get; set; }
            public decimal OverallAverage { get; set; }
        }

        public class SubjectPerformance
        {
            public int SubjectID { get; set; }
            public string SubjectName { get; set; }
            public string SubjectCode { get; set; }
            public decimal AverageGrade { get; set; }
            public int AssignmentsCompleted { get; set; }
            public int TestsTaken { get; set; }
            public decimal AttendanceRate { get; set; }
        }

        public class ParentStats
        {
            public int ChildrenCount { get; set; }
            public decimal AverageGrade { get; set; }
            public decimal AttendanceRate { get; set; }
            public int NotificationCount { get; set; }
        }
    }
}