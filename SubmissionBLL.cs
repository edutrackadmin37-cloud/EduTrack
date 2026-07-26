using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class SubmissionBLL
    {
        private readonly SubmissionDAL _dal = new SubmissionDAL();

        public Response<Submission> GetById(int submissionId)
        {
            if (submissionId <= 0)
                return Response<Submission>.Failure("Invalid submission ID.", "VALIDATION_ERROR");
            var sub = _dal.GetById(submissionId);
            return sub == null ? Response<Submission>.Failure("Not found.", "NOT_FOUND") : Response<Submission>.Success(sub);
        }

        public Response<int> CreateSubmission(Submission submission)
        {
            if (submission == null || submission.AssignmentID <= 0 || submission.StudentID <= 0)
                return Response<int>.Failure("Invalid submission data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(submission);
                return id > 0 ? Response<int>.Success(id, "Submission created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> UpdateSubmission(Submission submission)
        {
            if (submission == null || submission.SubmissionID <= 0)
                return Response<bool>.Failure("Invalid submission data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(submission);
                return ok ? Response<bool>.Success(true, "Submission updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDeleteSubmission(int submissionId)
        {
            if (submissionId <= 0)
                return Response<bool>.Failure("Invalid submission ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.SoftDelete(submissionId);
                return ok ? Response<bool>.Success(true, "Submission deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Submission>> GetByAssignment(int assignmentId)
        {
            if (assignmentId <= 0)
                return Response<List<Submission>>.Failure("Invalid assignment ID.", "VALIDATION_ERROR");

            try
            {
                var list = _dal.GetByAssignment(assignmentId);
                return Response<List<Submission>>.Success(list);
            }
            catch (Exception ex)
            {
                return Response<List<Submission>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}