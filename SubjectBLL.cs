using EduTrack.DAL;
using EduTrack.Models;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class SubjectBLL
    {
        private readonly SubjectDAL _subjectDAL = new SubjectDAL();
        private readonly ProgrammeSubjectDAL _programmeSubjectDAL = new ProgrammeSubjectDAL();

        public Response<List<Subject>> GetAllSubjects()
        {
            return Response<List<Subject>>.Success(_subjectDAL.GetAll());
        }

        public Response<Subject> GetSubjectById(int id)
        {
            if (id <= 0) return Response<Subject>.Failure("Invalid subject ID.", "VALIDATION_ERROR");
            Subject subject = _subjectDAL.GetById(id);
            return subject == null ? Response<Subject>.Failure("Subject not found.", "NOT_FOUND") : Response<Subject>.Success(subject);
        }

        public Response<int> CreateSubject(Subject subject)
        {
            if (subject == null || string.IsNullOrWhiteSpace(subject.SubjectName))
                return Response<int>.Failure("Subject name is required.", "VALIDATION_ERROR");

            int id = _subjectDAL.Create(subject);
            return id > 0 ? Response<int>.Success(id, "Subject created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
        }

        public Response<bool> UpdateSubject(Subject subject)
        {
            if (subject == null || subject.SubjectID <= 0)
                return Response<bool>.Failure("Invalid subject data.", "VALIDATION_ERROR");

            bool ok = _subjectDAL.Update(subject);
            return ok ? Response<bool>.Success(true, "Subject updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDeleteSubject(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid subject ID.", "VALIDATION_ERROR");
            bool ok = _subjectDAL.SoftDelete(id);
            return ok ? Response<bool>.Success(true, "Subject deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }

        public Response<List<ProgrammeSubject>> GetProgrammeSubjects(int programmeId)
        {
            if (programmeId <= 0) return Response<List<ProgrammeSubject>>.Failure("Invalid programme ID.", "VALIDATION_ERROR");
            return Response<List<ProgrammeSubject>>.Success(_programmeSubjectDAL.GetByProgramme(programmeId));
        }

        public Response<int> AssignSubjectToProgramme(int programmeId, int subjectId, bool isElective)
        {
            if (programmeId <= 0 || subjectId <= 0) return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            int id = _programmeSubjectDAL.Create(programmeId, subjectId, isElective);
            return id > 0 ? Response<int>.Success(id, "Subject assigned.") : Response<int>.Failure("Assignment failed.", "CREATE_FAILED");
        }

        public Response<bool> RemoveProgrammeSubject(int programmeSubjectId)
        {
            if (programmeSubjectId <= 0) return Response<bool>.Failure("Invalid ProgrammeSubject ID.", "VALIDATION_ERROR");
            bool ok = _programmeSubjectDAL.SoftDelete(programmeSubjectId);
            return ok ? Response<bool>.Success(true, "Assignment removed.") : Response<bool>.Failure("Remove failed.", "DELETE_FAILED");
        }
    }
}