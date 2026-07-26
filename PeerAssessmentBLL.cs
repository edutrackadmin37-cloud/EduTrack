// ============================================================
// BLL/PeerAssessmentBLL.cs (Complete)
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class PeerAssessmentBLL
    {
        private readonly PeerAssessmentDAL _dal = new PeerAssessmentDAL();

        public Response<List<PeerAssessment>> GetByAssessor(int assessorId)
        {
            if (assessorId <= 0) return Response<List<PeerAssessment>>.Failure("Invalid Assessor ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByAssessor(assessorId);
                return Response<List<PeerAssessment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<PeerAssessment>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<PeerAssessment>> GetByAssessee(int assesseeId)
        {
            if (assesseeId <= 0) return Response<List<PeerAssessment>>.Failure("Invalid Assessee ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByAssessee(assesseeId);
                return Response<List<PeerAssessment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<PeerAssessment>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<PeerAssessment>> GetByProject(int projectId)
        {
            if (projectId <= 0) return Response<List<PeerAssessment>>.Failure("Invalid Project ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByProject(projectId);
                return Response<List<PeerAssessment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<PeerAssessment>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<PeerAssessment> GetById(int id)
        {
            if (id <= 0) return Response<PeerAssessment>.Failure("Invalid PeerAssessment ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<PeerAssessment>.Failure("Assessment not found.", "NOT_FOUND")
                    : Response<PeerAssessment>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<PeerAssessment>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> HasPeerAssessment(int assessorId, int assesseeId, int projectId)
        {
            if (assessorId <= 0 || assesseeId <= 0 || projectId <= 0)
                return Response<bool>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                bool exists = _dal.Exists(assessorId, assesseeId, projectId);
                return Response<bool>.Success(exists);
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(PeerAssessment model)
        {
            if (model == null || model.AssessorID <= 0 || model.AssesseeID <= 0 || model.ProjectID <= 0 || model.Score < 1 || model.Score > 5)
                return Response<int>.Failure("Invalid peer assessment data (score must be 1-5).", "VALIDATION_ERROR");

            try
            {
                // Prevent duplicate
                if (_dal.Exists(model.AssessorID, model.AssesseeID, model.ProjectID))
                    return Response<int>.Failure("Assessment already exists for this student and project.", "DUPLICATE");

                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Peer assessment saved.")
                    : Response<int>.Failure("Save failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(PeerAssessment model)
        {
            if (model == null || model.PeerAssessmentID <= 0 || model.Score < 1 || model.Score > 5)
                return Response<bool>.Failure("Invalid peer assessment data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Peer assessment updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid PeerAssessment ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Peer assessment deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}