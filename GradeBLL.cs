using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class GradeBLL
    {
        private readonly GradeDAL _dal = new GradeDAL();

        public Response<Grade> GetBySubmission(int submissionId)
        {
            if (submissionId <= 0)
                return Response<Grade>.Failure("Invalid submission ID.", "VALIDATION_ERROR");
            var list = _dal.GetBySubmission(submissionId);
            var grade = list != null && list.Count > 0 ? list[0] : null;
            return grade == null ? Response<Grade>.Failure("Not found.", "NOT_FOUND") : Response<Grade>.Success(grade);
        }

        public Response<int> CreateGrade(Grade grade)
        {
            if (grade == null || grade.SubmissionID <= 0 || grade.StudentID <= 0 || grade.GradedBy <= 0)
                return Response<int>.Failure("Invalid grade data.", "VALIDATION_ERROR");

            if (!grade.GradeValue.HasValue || grade.GradeValue.Value < 0 || grade.GradeValue.Value > 100)
                return Response<int>.Failure("Grade must be between 0 and 100.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(grade);
                return id > 0 ? Response<int>.Success(id, "Grade saved.") : Response<int>.Failure("Save failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> UpdateGrade(Grade grade)
        {
            if (grade == null || grade.GradeID <= 0)
                return Response<bool>.Failure("Invalid grade data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(grade);
                return ok ? Response<bool>.Success(true, "Grade updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDeleteGrade(int gradeId)
        {
            if (gradeId <= 0)
                return Response<bool>.Failure("Invalid grade ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.SoftDelete(gradeId);
                return ok ? Response<bool>.Success(true, "Grade deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> GradeSubmission(int submissionId, decimal gradeValue, int gradedBy)
        {
            if (submissionId <= 0 || gradedBy <= 0)
                return Response<bool>.Failure("Invalid submission or grader.", "VALIDATION_ERROR");

            if (gradeValue < 0 || gradeValue > 100)
                return Response<bool>.Failure("Grade must be between 0 and 100.", "VALIDATION_ERROR");

            try
            {
                var existing = GetBySubmission(submissionId);
                if (existing.IsSuccess && existing.Data != null)
                {
                    existing.Data.GradeValue = gradeValue;
                    existing.Data.GradedBy = gradedBy;
                    existing.Data.DateGraded = DateTime.Now;
                    return UpdateGrade(existing.Data);
                }
                else
                {
                    // Need StudentID and AssignmentID – fetch from submission
                    var subDAL = new SubmissionDAL();
                    var sub = subDAL.GetById(submissionId);
                    if (sub == null) return Response<bool>.Failure("Submission not found.", "NOT_FOUND");
                    var grade = new Grade
                    {
                        SubmissionID = submissionId,
                        StudentID = sub.StudentID,
                        GradeValue = gradeValue,
                        GradedBy = gradedBy,
                        DateGraded = DateTime.Now
                    };
                    var result = CreateGrade(grade);
                    return result.IsSuccess ? Response<bool>.Success(true, "Grade saved.") : Response<bool>.Failure(result.Message, result.ErrorCode);
                }
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}