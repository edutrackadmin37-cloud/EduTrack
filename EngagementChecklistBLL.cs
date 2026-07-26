// ============================================================
// BLL/EngagementChecklistBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class EngagementChecklistBLL
    {
        private readonly EngagementChecklistDAL _dal = new EngagementChecklistDAL();

        /// <summary>
        /// Saves a new engagement checklist or updates an existing one for the same student/project/week.
        /// </summary>
        public Response<int> Save(EngagementChecklist model)
        {
            if (model == null)
                return Response<int>.Failure("Engagement data is required.", "VALIDATION_ERROR");

            if (model.ClassStudentID <= 0 || model.ProjectID <= 0 || model.WeekNumber <= 0)
                return Response<int>.Failure("Invalid ClassStudent, Project, or Week.", "VALIDATION_ERROR");

            if (model.MarkedBy <= 0)
                return Response<int>.Failure("Invalid marker.", "VALIDATION_ERROR");

            try
            {
                // Check if already exists – if so, update instead of insert.
                bool exists = _dal.Exists(model.ClassStudentID, model.ProjectID, model.WeekNumber);

                int resultId;
                if (exists)
                {
                    // We need the existing ChecklistID to update. We'll fetch it first.
                    var existing = _dal.GetByStudentAndProject(model.ClassStudentID, model.ProjectID)
                                      .Find(e => e.WeekNumber == model.WeekNumber);
                    if (existing != null)
                    {
                        model.ChecklistID = existing.ChecklistID;
                        bool updated = _dal.Update(model);
                        resultId = updated ? model.ChecklistID : 0;
                    }
                    else
                    {
                        // Should not happen, but fallback to insert.
                        resultId = _dal.Create(model);
                    }
                }
                else
                {
                    resultId = _dal.Create(model);
                }

                return resultId > 0
                    ? Response<int>.Success(resultId, "Engagement checklist saved.")
                    : Response<int>.Failure("Failed to save engagement checklist.", "SAVE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error saving engagement: {ex.Message}", "BLL_ERROR");
            }
        }

        /// <summary>
        /// Gets all engagement checklists for a specific project and week.
        /// </summary>
        public Response<List<EngagementChecklist>> GetByProjectWeek(int projectId, int weekNumber)
        {
            if (projectId <= 0 || weekNumber <= 0)
                return Response<List<EngagementChecklist>>.Failure("Invalid project ID or week number.", "VALIDATION_ERROR");

            try
            {
                var data = _dal.GetByProjectWeek(projectId, weekNumber);
                return Response<List<EngagementChecklist>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<EngagementChecklist>>.Failure($"Error retrieving data: {ex.Message}", "DAL_ERROR");
            }
        }

        /// <summary>
        /// Gets engagement checklists for a specific student (ClassStudentID) and project.
        /// </summary>
        public Response<List<EngagementChecklist>> GetByStudentAndProject(int classStudentId, int projectId)
        {
            if (classStudentId <= 0 || projectId <= 0)
                return Response<List<EngagementChecklist>>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var data = _dal.GetByStudentAndProject(classStudentId, projectId);
                return Response<List<EngagementChecklist>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<EngagementChecklist>>.Failure($"Error retrieving data: {ex.Message}", "DAL_ERROR");
            }
        }

        /// <summary>
        /// Soft deletes a checklist.
        /// </summary>
        public Response<bool> Delete(int checklistId)
        {
            if (checklistId <= 0)
                return Response<bool>.Failure("Invalid checklist ID.", "VALIDATION_ERROR");

            try
            {
                bool result = _dal.SoftDelete(checklistId);
                return result
                    ? Response<bool>.Success(true, "Checklist deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "BLL_ERROR");
            }
        }
    }
}