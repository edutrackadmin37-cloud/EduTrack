using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ClassSubjectTeacherBLL
    {
        private readonly ClassSubjectTeacherDAL _dal = new ClassSubjectTeacherDAL();

        public Response<List<ClassSubjectTeacher>> GetByClass(int classId)
        {
            if (classId <= 0)
                return Response<List<ClassSubjectTeacher>>.Failure("Invalid class ID.", "VALIDATION_ERROR");
            return Response<List<ClassSubjectTeacher>>.Success(_dal.GetByClass(classId));
        }

        public Response<int> AssignTeacher(int classId, int subjectId, int teacherId)
        {
            if (classId <= 0 || subjectId <= 0 || teacherId <= 0)
                return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            // Check if already exists
            var existing = _dal.GetByClassAndSubject(classId, subjectId);
            if (existing != null && existing.TeacherID == teacherId)
                return Response<int>.Failure("Teacher already assigned to this class and subject.", "DUPLICATE");

            var model = new ClassSubjectTeacher
            {
                ClassID = classId,
                SubjectID = subjectId,
                TeacherID = teacherId
            };
            int id = _dal.Create(model);
            return id > 0 ? Response<int>.Success(id, "Teacher assigned successfully.") : Response<int>.Failure("Assignment failed.", "CREATE_FAILED");
        }

        public Response<bool> RemoveAssignment(int classSubjectTeacherId)
        {
            if (classSubjectTeacherId <= 0)
                return Response<bool>.Failure("Invalid ID.", "VALIDATION_ERROR");

            bool ok = _dal.SoftDelete(classSubjectTeacherId);
            return ok ? Response<bool>.Success(true, "Assignment removed.") : Response<bool>.Failure("Removal failed.", "DELETE_FAILED");
        }
    }
}