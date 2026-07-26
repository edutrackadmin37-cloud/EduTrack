// ============================================================
// BLL/ContinuousAssessmentBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ContinuousAssessmentBLL
    {
        private readonly ContinuousAssessmentDAL _dal = new ContinuousAssessmentDAL();

        public Response<List<ContinuousAssessment>> GetByStudent(int studentId)
        {
            if (studentId <= 0) return Response<List<ContinuousAssessment>>.Failure("Invalid Student ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByStudent(studentId);
                return Response<List<ContinuousAssessment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<ContinuousAssessment>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<ContinuousAssessment>> GetByClassSubject(int classId, int subjectId, int academicYearId)
        {
            if (classId <= 0 || subjectId <= 0 || academicYearId <= 0)
                return Response<List<ContinuousAssessment>>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByClassSubject(classId, subjectId, academicYearId);
                return Response<List<ContinuousAssessment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<ContinuousAssessment>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ContinuousAssessment> GetById(int id)
        {
            if (id <= 0) return Response<ContinuousAssessment>.Failure("Invalid ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<ContinuousAssessment>.Failure("Record not found.", "NOT_FOUND")
                    : Response<ContinuousAssessment>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<ContinuousAssessment>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ContinuousAssessment> GetByStudentSubject(int studentId, int subjectId, int academicYearId)
        {
            if (studentId <= 0 || subjectId <= 0 || academicYearId <= 0)
                return Response<ContinuousAssessment>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByStudentSubject(studentId, subjectId, academicYearId);
                return data == null
                    ? Response<ContinuousAssessment>.Failure("No record found.", "NOT_FOUND")
                    : Response<ContinuousAssessment>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<ContinuousAssessment>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(ContinuousAssessment model)
        {
            if (model == null || model.StudentID <= 0 || model.SubjectID <= 0 || model.ClassID <= 0 || model.AcademicYearID <= 0)
                return Response<int>.Failure("Invalid continuous assessment data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Continuous assessment saved.")
                    : Response<int>.Failure("Save failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(ContinuousAssessment model)
        {
            if (model == null || model.ContinuousAssessmentID <= 0)
                return Response<bool>.Failure("Invalid continuous assessment data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Continuous assessment updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Continuous assessment deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}