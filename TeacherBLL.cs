using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class TeacherBLL
    {
        private readonly UserDAL _userDAL = new UserDAL();
        private readonly ClassDAL _classDAL = new ClassDAL();
        private readonly ClassStudentDAL _classStudentDAL = new ClassStudentDAL();
        private readonly ClassSubjectTeacherDAL _classSubjectTeacherDAL = new ClassSubjectTeacherDAL();
        private readonly SubjectDAL _subjectDAL = new SubjectDAL();
        private readonly ProjectDAL _projectDAL = new ProjectDAL();
        private readonly TimetableDAL _timetableDAL = new TimetableDAL();

        public Response<List<User>> GetTeacherStudents(int teacherId)
        {
            if (teacherId <= 0)
                return Response<List<User>>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var classes = _classDAL.GetAll().Where(c => c.ClassTeacherID == teacherId).Select(c => c.ClassID).ToList();
                var cstClasses = _classSubjectTeacherDAL.GetAll().Where(cst => cst.TeacherID == teacherId).Select(cst => cst.ClassID).Distinct().ToList();
                var allClassIds = classes.Union(cstClasses).Distinct().ToList();

                if (!allClassIds.Any())
                    return Response<List<User>>.Success(new List<User>());

                var students = new List<User>();
                foreach (var classId in allClassIds)
                {
                    var classStudents = _classStudentDAL.GetByClass(classId);
                    foreach (var cs in classStudents)
                    {
                        var user = _userDAL.GetById(cs.StudentID);
                        if (user != null && !students.Any(s => s.UserID == user.UserID))
                            students.Add(user);
                    }
                }

                return Response<List<User>>.Success(students);
            }
            catch (Exception ex)
            {
                return Response<List<User>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Class>> GetTeacherClasses(int teacherId)
        {
            if (teacherId <= 0)
                return Response<List<Class>>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var allClasses = _classDAL.GetAll();
                var assigned = allClasses.Where(c => c.ClassTeacherID == teacherId).ToList();
                var cstClasses = _classSubjectTeacherDAL.GetAll().Where(cst => cst.TeacherID == teacherId).Select(cst => cst.ClassID).Distinct();
                var extra = allClasses.Where(c => cstClasses.Contains(c.ClassID) && !assigned.Any(ac => ac.ClassID == c.ClassID)).ToList();
                assigned.AddRange(extra);
                return Response<List<Class>>.Success(assigned);
            }
            catch (Exception ex)
            {
                return Response<List<Class>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<ClassStudent>> GetClassStudents(int classId)
        {
            if (classId <= 0)
                return Response<List<ClassStudent>>.Failure("Invalid class ID.", "VALIDATION_ERROR");

            try
            {
                var list = _classStudentDAL.GetByClass(classId);
                return Response<List<ClassStudent>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<ClassStudent>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Subject>> GetTeacherSubjects(int teacherId, int classId)
        {
            if (teacherId <= 0 || classId <= 0)
                return Response<List<Subject>>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var cstList = _classSubjectTeacherDAL.GetAll().Where(cst => cst.TeacherID == teacherId && cst.ClassID == classId).ToList();
                var subjectIds = cstList.Select(cst => cst.SubjectID).Distinct().ToList();
                var subjects = _subjectDAL.GetAll().Where(s => subjectIds.Contains(s.SubjectID)).ToList();
                return Response<List<Subject>>.Success(subjects);
            }
            catch (Exception ex)
            {
                return Response<List<Subject>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> GetOrCreateClassSubjectTeacher(int classId, int subjectId, int teacherId)
        {
            if (classId <= 0 || subjectId <= 0 || teacherId <= 0)
                return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var existing = _classSubjectTeacherDAL.GetAll().FirstOrDefault(cst => cst.ClassID == classId && cst.SubjectID == subjectId && cst.TeacherID == teacherId);
                if (existing != null)
                    return Response<int>.Success(existing.ClassSubjectTeacherID);

                var newCst = new ClassSubjectTeacher
                {
                    ClassID = classId,
                    SubjectID = subjectId,
                    TeacherID = teacherId
                };
                int id = _classSubjectTeacherDAL.Create(newCst);
                return id > 0 ? Response<int>.Success(id, "Assignment created.") : Response<int>.Failure("Failed to create assignment.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ClassSubjectTeacher> GetClassSubjectTeacher(int classSubjectTeacherId)
        {
            if (classSubjectTeacherId <= 0)
                return Response<ClassSubjectTeacher>.Failure("Invalid ID.", "VALIDATION_ERROR");

            try
            {
                var item = _classSubjectTeacherDAL.GetById(classSubjectTeacherId);
                return item == null
                    ? Response<ClassSubjectTeacher>.Failure("Not found.", "NOT_FOUND")
                    : Response<ClassSubjectTeacher>.Success(item);
            }
            catch (Exception ex)
            {
                return Response<ClassSubjectTeacher>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<SessionModel>> GetTodaySessions(int teacherId)
        {
            if (teacherId <= 0)
                return Response<List<SessionModel>>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var today = DateTime.Now.Date;
                var dayOfWeek = today.DayOfWeek.ToString();
                var allTimetables = _timetableDAL.GetAll();
                var timetables = allTimetables.Where(t => t.TeacherID == teacherId && t.DayOfWeek == dayOfWeek).ToList();

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

        public Response<List<Project>> GetTeacherProjects(int teacherId)
        {
            if (teacherId <= 0)
                return Response<List<Project>>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var cstIds = _classSubjectTeacherDAL.GetAll().Where(cst => cst.TeacherID == teacherId).Select(cst => cst.ClassSubjectTeacherID).ToList();
                var allProjects = _projectDAL.GetAll();
                var projects = allProjects.Where(p => cstIds.Contains(p.ClassSubjectTeacherID)).ToList();
                return Response<List<Project>>.Success(projects);
            }
            catch (Exception ex)
            {
                return Response<List<Project>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<User>> GetStudentsForTest(int testId, int teacherId)
        {
            return Response<List<User>>.Failure("Not implemented.", "NOT_IMPLEMENTED");
        }
    }

    // SessionModel is defined inside the same namespace
    public class SessionModel
    {
        public int SessionID { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Room { get; set; }
        public string Status { get; set; }
        public string StatusClass { get; set; }
        public string BadgeClass { get; set; }
        public string CanJoin { get; set; }
        public string JoinText { get; set; }
    }
}