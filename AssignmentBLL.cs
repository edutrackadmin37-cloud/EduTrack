// ============================================================
// BLL/AssignmentBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class AssignmentBLL
    {
        private readonly AssignmentDAL _assignmentDAL = new AssignmentDAL();
        private readonly SubmissionDAL _submissionDAL = new SubmissionDAL();
        private readonly GradeDAL _gradeDAL = new GradeDAL();
        private readonly RubricDAL _rubricDAL = new RubricDAL();

        /// <summary>
        /// Gets all assignments for a specific class-subject-teacher combination.
        /// </summary>
        public Response<List<Assignment>> GetAssignmentsByClassSubjectTeacher(int classSubjectTeacherId)
        {
            if (classSubjectTeacherId <= 0)
                return Response<List<Assignment>>.Failure("Invalid ClassSubjectTeacher ID.", "VALIDATION_ERROR");

            try
            {
                List<Assignment> data = _assignmentDAL.GetByClassSubjectTeacher(classSubjectTeacherId);
                return Response<List<Assignment>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Assignment>>.Failure($"Failed to retrieve assignments: {ex.Message}", "DAL_ERROR");
            }
        }
        public Response<int> CreateSubmission(Submission model)
        {
            if (model == null) return Response<int>.Failure("Invalid submission data.", "VALIDATION_ERROR");
            if (model.AssignmentID <= 0 || model.StudentID <= 0) return Response<int>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                int id = _submissionDAL.Create(model);
                return id > 0 ? Response<int>.Success(id, "Submission created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
        /// <summary>
        /// Gets all submissions for a specific assignment.
        /// </summary>
        public Response<List<Submission>> GetSubmissionsByAssignment(int assignmentId)
        {
            if (assignmentId <= 0)
                return Response<List<Submission>>.Failure("Invalid Assignment ID.", "VALIDATION_ERROR");

            try
            {
                List<Submission> data = _submissionDAL.GetByAssignment(assignmentId);
                return Response<List<Submission>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Submission>>.Failure($"Failed to retrieve submissions: {ex.Message}", "DAL_ERROR");
            }
        }

        /// <summary>
        /// Creates a new assignment.
        /// </summary>
        public Response<int> CreateAssignment(Assignment model)
        {
            if (model == null)
                return Response<int>.Failure("Assignment data is required.", "VALIDATION_ERROR");

            if (model.ClassSubjectTeacherID <= 0)
                return Response<int>.Failure("ClassSubjectTeacher is required.", "VALIDATION_ERROR");

            if (string.IsNullOrWhiteSpace(model.Title))
                return Response<int>.Failure("Assignment title is required.", "VALIDATION_ERROR");

            if (model.RubricID <= 0)
                return Response<int>.Failure("Rubric is required.", "VALIDATION_ERROR");

            if (model.DueDate.HasValue && model.AssignedDate.HasValue && model.DueDate.Value < model.AssignedDate.Value)
                return Response<int>.Failure("Due date cannot be before assigned date.", "VALIDATION_ERROR");

            try
            {
                int id = _assignmentDAL.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Assignment created successfully.")
                    : Response<int>.Failure("Failed to create assignment.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error creating assignment: {ex.Message}", "DAL_ERROR");
            }
        }

        /// <summary>
        /// Grades a submission (creates or updates a Grade record).
        /// </summary>
        public Response<bool> GradeSubmission(Grade grade)
        {
            if (grade == null)
                return Response<bool>.Failure("Grade data is required.", "VALIDATION_ERROR");

            if (grade.SubmissionID <= 0)
                return Response<bool>.Failure("Invalid Submission ID.", "VALIDATION_ERROR");

            if (grade.StudentID <= 0)
                return Response<bool>.Failure("Invalid Student ID.", "VALIDATION_ERROR");

            if (grade.GradedBy <= 0)
                return Response<bool>.Failure("Invalid Grader ID.", "VALIDATION_ERROR");

            if (!grade.GradeValue.HasValue || grade.GradeValue.Value < 0 || grade.GradeValue.Value > 100)
                return Response<bool>.Failure("Grade value must be between 0 and 100.", "VALIDATION_ERROR");

            try
            {
                // Check if a grade already exists for this submission
                var existing = _gradeDAL.GetBySubmission(grade.SubmissionID);
                Grade existingGrade = existing != null && existing.Count > 0 ? existing[0] : null;

                bool result;
                if (existingGrade != null)
                {
                    // Update existing grade
                    existingGrade.GradeValue = grade.GradeValue;
                    existingGrade.Remarks = grade.Remarks;
                    result = _gradeDAL.Update(existingGrade);
                }
                else
                {
                    // Create new grade
                    int newId = _gradeDAL.Create(grade);
                    result = newId > 0;
                }

                return result
                    ? Response<bool>.Success(true, "Grade saved successfully.")
                    : Response<bool>.Failure("Failed to save grade.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error saving grade: {ex.Message}", "DAL_ERROR");
            }
        }

        /// <summary>
        /// Updates an existing assignment.
        /// </summary>
        public Response<bool> UpdateAssignment(Assignment model)
        {
            if (model == null || model.AssignmentID <= 0)
                return Response<bool>.Failure("Invalid assignment data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _assignmentDAL.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Assignment updated successfully.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error updating assignment: {ex.Message}", "DAL_ERROR");
            }
        }

        /// <summary>
        /// Soft deletes an assignment.
        /// </summary>
        public Response<bool> SoftDeleteAssignment(int assignmentId)
        {
            if (assignmentId <= 0)
                return Response<bool>.Failure("Invalid assignment ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _assignmentDAL.SoftDelete(assignmentId);
                return ok
                    ? Response<bool>.Success(true, "Assignment deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error deleting assignment: {ex.Message}", "DAL_ERROR");
            }
        }

        /// <summary>
        /// Gets a specific assignment by ID.
        /// </summary>
        public Response<Assignment> GetAssignmentById(int assignmentId)
        {
            if (assignmentId <= 0)
                return Response<Assignment>.Failure("Invalid assignment ID.", "VALIDATION_ERROR");

            try
            {
                Assignment data = _assignmentDAL.GetById(assignmentId);
                return data == null
                    ? Response<Assignment>.Failure("Assignment not found.", "NOT_FOUND")
                    : Response<Assignment>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<Assignment>.Failure($"Error retrieving assignment: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}