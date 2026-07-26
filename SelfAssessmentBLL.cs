// ============================================================
// BLL/SelfAssessmentBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class SelfAssessmentBLL
    {
        private readonly SelfAssessmentDAL _dal = new SelfAssessmentDAL();

        public Response<List<SelfAssessment>> GetByStudent(int studentId)
        {
            if (studentId <= 0) return Response<List<SelfAssessment>>.Failure("Invalid Student ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByStudent(studentId);
                return Response<List<SelfAssessment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<SelfAssessment>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<SelfAssessment>> GetByProject(int projectId)
        {
            if (projectId <= 0) return Response<List<SelfAssessment>>.Failure("Invalid Project ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByProject(projectId);
                return Response<List<SelfAssessment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<SelfAssessment>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<SelfAssessment> GetById(int id)
        {
            if (id <= 0) return Response<SelfAssessment>.Failure("Invalid SelfAssessment ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<SelfAssessment>.Failure("Self assessment not found.", "NOT_FOUND")
                    : Response<SelfAssessment>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<SelfAssessment>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> HasSelfAssessment(int studentId, int projectId)
        {
            if (studentId <= 0 || projectId <= 0) return Response<bool>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                bool exists = _dal.Exists(studentId, projectId);
                return Response<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(SelfAssessment model)
        {
            if (model == null || model.StudentID <= 0 || model.ProjectID <= 0 || model.Score < 1 || model.Score > 100)
                return Response<int>.Failure("Invalid self assessment data (score must be 1-100).", "VALIDATION_ERROR");

            try
            {
                if (_dal.Exists(model.StudentID, model.ProjectID))
                    return Response<int>.Failure("Self assessment already exists for this project.", "DUPLICATE");

                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Self assessment saved.")
                    : Response<int>.Failure("Save failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(SelfAssessment model)
        {
            if (model == null || model.SelfAssessmentID <= 0 || model.Score < 1 || model.Score > 100)
                return Response<bool>.Failure("Invalid self assessment data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Self assessment updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid SelfAssessment ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Self assessment deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}