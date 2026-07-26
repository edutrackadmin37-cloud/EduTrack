using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class StudentBLL
    {
        private readonly ClassStudentDAL _classStudentDAL = new ClassStudentDAL();
        private readonly ClassDAL _classDAL = new ClassDAL();
        private readonly ProjectDAL _projectDAL = new ProjectDAL();
        private readonly TeamDAL _teamDAL = new TeamDAL();
        private readonly ProjectTeamMemberDAL _memberDAL = new ProjectTeamMemberDAL();
        private readonly UserDAL _userDAL = new UserDAL();
        private readonly GradeDAL _gradeDAL = new GradeDAL();
        private readonly TimetableDAL _timetableDAL = new TimetableDAL();
        private readonly AttendanceDAL _attendanceDAL = new AttendanceDAL();
        private readonly SubjectDAL _subjectDAL = new SubjectDAL();
        private readonly SubmissionDAL _submissionDAL = new SubmissionDAL();
        private readonly AssignmentDAL _assignmentDAL = new AssignmentDAL();
        private readonly ClassSubjectTeacherDAL _cstDAL = new ClassSubjectTeacherDAL();

        public Response<List<Class>> GetStudentClasses(int studentId)
        {
            if (studentId <= 0)
                return Response<List<Class>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                var enrollments = _classStudentDAL.GetByStudent(studentId);
                var classes = new List<Class>();
                foreach (var e in enrollments)
                {
                    var cls = _classDAL.GetById(e.ClassID);
                    if (cls != null) classes.Add(cls);
                }
                return Response<List<Class>>.Success(classes);
            }
            catch (Exception ex)
            {
                return Response<List<Class>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<TeamMemberInfo>> GetStudentTeams(int studentId)
        {
            if (studentId <= 0)
                return Response<List<TeamMemberInfo>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                var allMembers = _memberDAL.GetAll();
                var userMemberships = allMembers.Where(m => m.StudentID == studentId && !m.IsDeleted).ToList();
                var teamIds = userMemberships.Select(m => m.TeamID).Distinct().ToList();

                var teams = new List<TeamMemberInfo>();
                foreach (var tid in teamIds)
                {
                    var team = _teamDAL.GetById(tid);
                    if (team == null) continue;
                    var project = _projectDAL.GetById(team.ProjectID);
                    if (project == null) continue;
                    var members = _memberDAL.GetByTeam(tid);
                    var memberNames = string.Join(", ", members.Select(m => _userDAL.GetById(m.StudentID)?.FullName ?? "Unknown"));
                    teams.Add(new TeamMemberInfo
                    {
                        TeamID = tid,
                        TeamName = team.TeamName,
                        ProjectTitle = project.Title,
                        MemberCount = members.Count,
                        MemberNames = memberNames
                    });
                }
                return Response<List<TeamMemberInfo>>.Success(teams);
            }
            catch (Exception ex)
            {
                return Response<List<TeamMemberInfo>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<ProjectSummary>> GetStudentProjects(int studentId)
        {
            if (studentId <= 0)
                return Response<List<ProjectSummary>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                var memberships = _memberDAL.GetAll().Where(m => m.StudentID == studentId && !m.IsDeleted).ToList();
                var teamIds = memberships.Select(m => m.TeamID).Distinct().ToList();
                var teams = _teamDAL.GetAll().Where(t => teamIds.Contains(t.TeamID) && !t.IsDeleted).ToList();
                var projectIds = teams.Select(t => t.ProjectID).Distinct().ToList();
                var projects = _projectDAL.GetAll().Where(p => projectIds.Contains(p.ProjectID) && !p.IsDeleted).ToList();

                var summaries = new List<ProjectSummary>();
                foreach (var p in projects)
                {
                    var team = teams.FirstOrDefault(t => t.ProjectID == p.ProjectID);
                    summaries.Add(new ProjectSummary
                    {
                        ProjectID = p.ProjectID,
                        Title = p.Title,
                        Description = p.Description,
                        SubjectName = "N/A",
                        TeamName = team?.TeamName ?? "N/A",
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        Status = p.Status,
                        StatusClass = p.Status.ToLower()
                    });
                }
                return Response<List<ProjectSummary>>.Success(summaries);
            }
            catch (Exception ex)
            {
                return Response<List<ProjectSummary>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ProjectSummary> GetStudentProject(int studentId, int projectId)
        {
            if (studentId <= 0 || projectId <= 0)
                return Response<ProjectSummary>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var project = _projectDAL.GetById(projectId);
                if (project == null) return Response<ProjectSummary>.Failure("Project not found.", "NOT_FOUND");
                var teams = _teamDAL.GetByProject(projectId);
                var memberships = _memberDAL.GetAll().Where(m => m.StudentID == studentId && !m.IsDeleted).ToList();
                var team = teams.FirstOrDefault(t => memberships.Any(m => m.TeamID == t.TeamID));
                var summary = new ProjectSummary
                {
                    ProjectID = project.ProjectID,
                    Title = project.Title,
                    Description = project.Description,
                    TeamName = team?.TeamName ?? "N/A",
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    Status = project.Status,
                    StatusClass = project.Status.ToLower()
                };
                return Response<ProjectSummary>.Success(summary);
            }
            catch (Exception ex)
            {
                return Response<ProjectSummary>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<StudentGrade>> GetStudentGrades(int studentId)
        {
            if (studentId <= 0)
                return Response<List<StudentGrade>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                // Get all grades and filter by student via submissions
                var allGrades = _gradeDAL.GetAll();
                var filteredGrades = new List<Grade>();
                foreach (var g in allGrades)
                {
                    var sub = _submissionDAL.GetById(g.SubmissionID);
                    if (sub != null && sub.StudentID == studentId)
                        filteredGrades.Add(g);
                }

                var list = new List<StudentGrade>();
                foreach (var g in filteredGrades)   // <-- renamed from 'grades' to avoid conflict
                {
                    var submission = _submissionDAL.GetById(g.SubmissionID);
                    if (submission == null) continue;
                    var assignment = _assignmentDAL.GetById(submission.AssignmentID);
                    if (assignment == null) continue;
                    var cst = _cstDAL.GetById(assignment.ClassSubjectTeacherID);
                    if (cst == null) continue;
                    var subject = _subjectDAL.GetById(cst.SubjectID);
                    if (subject == null) continue;
                    var cls = _classDAL.GetById(cst.ClassID);

                    list.Add(new StudentGrade
                    {
                        SubjectName = subject.SubjectName,
                        SubjectCode = subject.SubjectCode,
                        ClassName = cls?.ClassName ?? "",
                        GradeValue = g.GradeValue ?? 0,
                        GradeLetter = GetGradeLetter(g.GradeValue ?? 0),
                        AssignmentsCompleted = 1,
                        TestsTaken = 0
                    });
                }
                return Response<List<StudentGrade>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<StudentGrade>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<OverallStats> GetStudentOverallStats(int studentId)
        {
            if (studentId <= 0)
                return Response<OverallStats>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                var gradesResp = GetStudentGrades(studentId);
                if (!gradesResp.IsSuccess) return Response<OverallStats>.Failure(gradesResp.Message, gradesResp.ErrorCode);
                var avg = gradesResp.Data.Any() ? gradesResp.Data.Average(g => g.GradeValue) : 0;
                return Response<OverallStats>.Success(new OverallStats
                {
                    OverallGrade = avg,
                    ClassRank = "N/A"
                });
            }
            catch (Exception ex)
            {
                return Response<OverallStats>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<SessionModel>> GetTodaySessions(int studentId)
        {
            if (studentId <= 0)
                return Response<List<SessionModel>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                var classesResp = GetStudentClasses(studentId);
                if (!classesResp.IsSuccess) return Response<List<SessionModel>>.Failure(classesResp.Message, classesResp.ErrorCode);
                var classIds = classesResp.Data.Select(c => c.ClassID).ToList();

                var today = DateTime.Now.Date;
                var dayOfWeek = today.DayOfWeek.ToString();
                var allTimetables = _timetableDAL.GetAll();
                var timetables = allTimetables.Where(t => classIds.Contains(t.ClassID) && t.DayOfWeek == dayOfWeek).ToList();

                var sessions = new List<SessionModel>();
                foreach (var tt in timetables)
                {
                    var cls = _classDAL.GetById(tt.ClassID);
                    var subj = _subjectDAL.GetById(tt.SubjectID);
                    var teacher = _userDAL.GetById(tt.TeacherID);
                    if (cls == null || subj == null || teacher == null) continue;

                    var now = DateTime.Now.TimeOfDay;
                    var start = TimeSpan.Parse(tt.StartTime);
                    var end = TimeSpan.Parse(tt.EndTime);

                    string status, statusClass, badgeClass, joinText;
                    bool canJoin;

                    if (now < start)
                    {
                        status = "Upcoming";
                        statusClass = "upcoming";
                        badgeClass = "warning";
                        canJoin = false;
                        joinText = "Not Started";
                    }
                    else if (now > end)
                    {
                        status = "Ended";
                        statusClass = "ended";
                        badgeClass = "secondary";
                        canJoin = false;
                        joinText = "Ended";
                    }
                    else
                    {
                        status = "Active";
                        statusClass = "active";
                        badgeClass = "success";
                        canJoin = true;
                        joinText = "Join Now";
                    }

                    sessions.Add(new SessionModel
                    {
                        SessionID = tt.TimetableID,
                        ClassName = cls.ClassName,
                        SubjectName = subj.SubjectName,
                        TeacherName = teacher.FullName,
                        StartTime = TimeSpan.Parse(tt.StartTime),
                        EndTime = TimeSpan.Parse(tt.EndTime),
                        Room = tt.Room ?? "N/A",
                        Status = status,
                        StatusClass = statusClass,
                        BadgeClass = badgeClass,
                        CanJoin = canJoin ? "" : "disabled",
                        JoinText = joinText
                    });
                }

                return Response<List<SessionModel>>.Success(sessions);
            }
            catch (Exception ex)
            {
                return Response<List<SessionModel>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<SessionDetail> GetSessionDetails(int sessionId, int studentId)
        {
            return Response<SessionDetail>.Failure("Not implemented.", "NOT_IMPLEMENTED");
        }

        public Response<CanJoinResult> CanJoinSession(int sessionId, int studentId)
        {
            return Response<CanJoinResult>.Failure("Not implemented.", "NOT_IMPLEMENTED");
        }

        public Response<List<TeamWithMembers>> GetStudentTeamsWithMembers(int studentId)
        {
            if (studentId <= 0)
                return Response<List<TeamWithMembers>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                var memberships = _memberDAL.GetAll().Where(m => m.StudentID == studentId && !m.IsDeleted).ToList();
                var teamIds = memberships.Select(m => m.TeamID).Distinct().ToList();
                var teams = _teamDAL.GetAll().Where(t => teamIds.Contains(t.TeamID) && !t.IsDeleted).ToList();

                var result = new List<TeamWithMembers>();
                foreach (var t in teams)
                {
                    var members = _memberDAL.GetByTeam(t.TeamID);
                    var memberUsers = new List<User>();
                    foreach (var m in members)
                    {
                        var u = _userDAL.GetById(m.StudentID);
                        if (u != null) memberUsers.Add(u);
                    }
                    var project = _projectDAL.GetById(t.ProjectID);
                    result.Add(new TeamWithMembers
                    {
                        TeamID = t.TeamID,
                        TeamName = t.TeamName,
                        ProjectTitle = project?.Title ?? "N/A",
                        MemberCount = memberUsers.Count,
                        Members = memberUsers
                    });
                }
                return Response<List<TeamWithMembers>>.Success(result);
            }
            catch (Exception ex)
            {
                return Response<List<TeamWithMembers>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<decimal> GetStudentAverageGrade(int studentId)
        {
            var gradesResp = GetStudentGrades(studentId);
            if (!gradesResp.IsSuccess) return Response<decimal>.Failure(gradesResp.Message, gradesResp.ErrorCode);
            var avg = gradesResp.Data.Any() ? gradesResp.Data.Average(g => g.GradeValue) : 0;
            return Response<decimal>.Success(avg);
        }

        private string GetGradeLetter(decimal score)
        {
            if (score >= 90) return "A";
            if (score >= 80) return "B";
            if (score >= 70) return "C";
            if (score >= 60) return "D";
            return "F";
        }

        // DTOs
        public class TeamMemberInfo
        {
            public int TeamID { get; set; }
            public string TeamName { get; set; }
            public string ProjectTitle { get; set; }
            public int MemberCount { get; set; }
            public string MemberNames { get; set; }
        }

        public class ProjectSummary
        {
            public int ProjectID { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string SubjectName { get; set; }
            public string TeamName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public string Status { get; set; }
            public string StatusClass { get; set; }
        }

        public class StudentGrade
        {
            public string SubjectName { get; set; }
            public string SubjectCode { get; set; }
            public string ClassName { get; set; }
            public decimal GradeValue { get; set; }
            public string GradeLetter { get; set; }
            public int AssignmentsCompleted { get; set; }
            public int TestsTaken { get; set; }
        }

        public class OverallStats
        {
            public decimal OverallGrade { get; set; }
            public string ClassRank { get; set; }
        }

        public class TeamWithMembers
        {
            public int TeamID { get; set; }
            public string TeamName { get; set; }
            public string ProjectTitle { get; set; }
            public int MemberCount { get; set; }
            public List<User> Members { get; set; }
        }

        public class SessionDetail
        {
            public string ClassName { get; set; }
            public string SubjectName { get; set; }
            public string TeacherName { get; set; }
            public string Room { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
        }

        public class CanJoinResult
        {
            public bool CanJoin { get; set; }
            public bool IsUpcoming { get; set; }
        }
    }
}