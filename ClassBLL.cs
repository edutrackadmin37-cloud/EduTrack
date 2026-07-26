// ============================================================
// BLL/ClassBLL.cs (Updated)
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ClassBLL
    {
        private readonly AcademicYearDAL _academicYearDAL = new AcademicYearDAL();
        private readonly StreamDAL _streamDAL = new StreamDAL();
        private readonly ClassSubjectTeacherDAL _classSubjectTeacherDAL = new ClassSubjectTeacherDAL();
        private readonly ClassStudentDAL _classStudentDAL = new ClassStudentDAL();
        private readonly ClassDAL _classDAL = new ClassDAL();  // NEW

        // ---- Academic Years ----
        public Response<List<AcademicYear>> GetAcademicYears()
        {
            return Response<List<AcademicYear>>.Success(_academicYearDAL.GetAll());
        }

        public Response<AcademicYear> GetCurrentAcademicYear()
        {
            AcademicYear item = _academicYearDAL.GetCurrent();
            return item == null
                ? Response<AcademicYear>.Failure("Current academic year not found.", "NOT_FOUND")
                : Response<AcademicYear>.Success(item);
        }

        public Response<int> CreateAcademicYear(AcademicYear model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.YearName))
                return Response<int>.Failure("Invalid academic year.", "VALIDATION_ERROR");

            int id = _academicYearDAL.Create(model);
            return id > 0
                ? Response<int>.Success(id, "Academic year created.")
                : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateAcademicYear(AcademicYear model)
        {
            if (model == null || model.AcademicYearID <= 0)
                return Response<bool>.Failure("Invalid academic year.", "VALIDATION_ERROR");

            bool ok = _academicYearDAL.Update(model);
            return ok
                ? Response<bool>.Success(true, "Academic year updated.")
                : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        // ---- Streams ----
        public Response<List<Stream>> GetStreams()
        {
            return Response<List<Stream>>.Success(_streamDAL.GetAll());
        }

        public Response<int> CreateStream(Stream model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.StreamName))
                return Response<int>.Failure("Invalid stream.", "VALIDATION_ERROR");

            int id = _streamDAL.Create(model);
            return id > 0
                ? Response<int>.Success(id, "Stream created.")
                : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        // ---- Classes (now fully implemented) ----
        public Response<List<Class>> GetClasses()
        {
            return Response<List<Class>>.Success(_classDAL.GetAll());
        }

        public Response<int> CreateClass(Class model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.ClassName) || model.AcademicYearID <= 0 || model.ProgrammeID <= 0 || model.StreamID <= 0)
                return Response<int>.Failure("Invalid class data.", "VALIDATION_ERROR");

            int id = _classDAL.Create(model);
            return id > 0
                ? Response<int>.Success(id, "Class created.")
                : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateClass(Class model)
        {
            if (model == null || model.ClassID <= 0)
                return Response<bool>.Failure("Invalid class data.", "VALIDATION_ERROR");

            bool ok = _classDAL.Update(model);
            return ok
                ? Response<bool>.Success(true, "Class updated.")
                : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDeleteClass(int classId)
        {
            if (classId <= 0) return Response<bool>.Failure("Invalid class ID.", "VALIDATION_ERROR");
            bool ok = _classDAL.SoftDelete(classId);
            return ok
                ? Response<bool>.Success(true, "Class deleted.")
                : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
        // Add to ClassBLL.cs
        public Response<bool> SoftDeleteAcademicYear(int academicYearId)
        {
            if (academicYearId <= 0) return Response<bool>.Failure("Invalid academic year ID.", "VALIDATION_ERROR");
            bool ok = _academicYearDAL.SoftDelete(academicYearId);
            return ok
                ? Response<bool>.Success(true, "Academic year deleted.")
                : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }
        public Response<List<Class>> GetClassesByStudent(int studentId)
        {
            if (studentId <= 0) return Response<List<Class>>.Failure("Invalid student ID.", "VALIDATION_ERROR");
            var classStudentDAL = new ClassStudentDAL();
            var classDAL = new ClassDAL();
            var classStudents = classStudentDAL.GetByStudent(studentId);
            var classes = new List<Class>();
            foreach (var cs in classStudents)
            {
                var cls = classDAL.GetById(cs.ClassID);
                if (cls != null) classes.Add(cls);
            }
            return Response<List<Class>>.Success(classes);
        }
        // ---- ClassSubjectTeacher ----
        public Response<int> AssignTeacherToClassSubject(int classId, int subjectId, int teacherId)
        {
            if (classId <= 0 || subjectId <= 0 || teacherId <= 0)
                return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            int id = _classSubjectTeacherDAL.Create(new ClassSubjectTeacher
            {
                ClassID = classId,
                SubjectID = subjectId,
                TeacherID = teacherId
            });

            return id > 0
                ? Response<int>.Success(id, "Teacher assigned.")
                : Response<int>.Failure("Assignment failed.", "CREATE_FAILED");
        }

        public Response<List<ClassSubjectTeacher>> GetClassSubjectTeachers(int classId)
        {
            if (classId <= 0)
                return Response<List<ClassSubjectTeacher>>.Failure("Invalid class ID.", "VALIDATION_ERROR");

            return Response<List<ClassSubjectTeacher>>.Success(_classSubjectTeacherDAL.GetByClass(classId));
        }

        // ---- Student Enrollment ----
        public Response<int> EnrollStudent(int classId, int studentId)
        {
            if (classId <= 0 || studentId <= 0)
                return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            // Check if already enrolled
            var existing = _classStudentDAL.GetByStudent(studentId);
            if (existing != null && existing.Exists(x => x.ClassID == classId && !x.IsDeleted))
                return Response<int>.Failure("Student already enrolled in this class.", "DUPLICATE");

            var cs = new ClassStudent
            {
                ClassID = classId,
                StudentID = studentId,
                IsActive = true
            };
            int id = _classStudentDAL.Create(cs);
            return id > 0
                ? Response<int>.Success(id, "Student enrolled.")
                : Response<int>.Failure("Enrollment failed.", "CREATE_FAILED");
        }

        public Response<List<ClassStudent>> GetClassStudents(int classId)
        {
            if (classId <= 0)
                return Response<List<ClassStudent>>.Failure("Invalid class ID.", "VALIDATION_ERROR");

            return Response<List<ClassStudent>>.Success(_classStudentDAL.GetByClass(classId));
        }
    }
}