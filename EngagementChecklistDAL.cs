// ============================================================
// DAL/EngagementChecklistDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class EngagementChecklistDAL : BaseDAL
    {
        /// <summary>
        /// Creates a new engagement checklist record.
        /// </summary>
        public int Create(EngagementChecklist model)
        {
            SqlParameter outId = new SqlParameter("@NewChecklistID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            ExecuteNonQuery("sp_CreateEngagementChecklist",
                new SqlParameter("@ClassStudentID", model.ClassStudentID),
                new SqlParameter("@ProjectID", model.ProjectID),
                new SqlParameter("@WeekNumber", model.WeekNumber),
                new SqlParameter("@Participation", model.Participation),
                new SqlParameter("@Questioning", model.Questioning),
                new SqlParameter("@ProblemSolving", model.ProblemSolving),
                new SqlParameter("@Collaboration", model.Collaboration),
                new SqlParameter("@TaskCompletion", model.TaskCompletion),
                new SqlParameter("@Motivation", model.Motivation),
                new SqlParameter("@MarkedBy", model.MarkedBy),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        /// <summary>
        /// Retrieves engagement checklists for a specific project and week.
        /// </summary>
        public List<EngagementChecklist> GetByProjectWeek(int projectId, int weekNumber)
        {
            var list = new List<EngagementChecklist>();

            using (SqlDataReader r = ExecuteReader("sp_GetEngagementByProjectWeek",
                new SqlParameter("@ProjectID", projectId),
                new SqlParameter("@WeekNumber", weekNumber)))
            {
                while (r.Read())
                {
                    list.Add(new EngagementChecklist
                    {
                        ChecklistID = GetValue<int>(r, "ChecklistID"),
                        ClassStudentID = GetValue<int>(r, "ClassStudentID"),
                        ProjectID = GetValue<int>(r, "ProjectID"),
                        WeekNumber = GetValue<int>(r, "WeekNumber"),
                        Participation = GetValue<bool>(r, "Participation"),
                        Questioning = GetValue<bool>(r, "Questioning"),
                        ProblemSolving = GetValue<bool>(r, "ProblemSolving"),
                        Collaboration = GetValue<bool>(r, "Collaboration"),
                        TaskCompletion = GetValue<bool>(r, "TaskCompletion"),
                        Motivation = GetValue<bool>(r, "Motivation"),
                        MarkedBy = GetValue<int>(r, "MarkedBy"),
                        MarkedAt = GetValue<DateTime>(r, "MarkedAt"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }

            return list;
        }

        /// <summary>
        /// Retrieves all checklists for a specific student (via ClassStudentID) and project.
        /// </summary>
        public List<EngagementChecklist> GetByStudentAndProject(int classStudentId, int projectId)
        {
            var list = new List<EngagementChecklist>();

            using (SqlDataReader r = ExecuteReader("sp_GetEngagementByStudentProject",
                new SqlParameter("@ClassStudentID", classStudentId),
                new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }

            return list;
        }

        /// <summary>
        /// Checks if a checklist already exists for a given student, project, and week.
        /// </summary>
        public bool Exists(int classStudentId, int projectId, int weekNumber)
        {
            using (SqlDataReader r = ExecuteReader("sp_CheckEngagementExists",
                new SqlParameter("@ClassStudentID", classStudentId),
                new SqlParameter("@ProjectID", projectId),
                new SqlParameter("@WeekNumber", weekNumber)))
            {
                return r.Read(); // If any row, it exists.
            }
        }

        /// <summary>
        /// Updates an existing checklist (replaces all indicators).
        /// </summary>
        public bool Update(EngagementChecklist model)
        {
            int rows = ExecuteNonQuery("sp_UpdateEngagementChecklist",
                new SqlParameter("@ChecklistID", model.ChecklistID),
                new SqlParameter("@Participation", model.Participation),
                new SqlParameter("@Questioning", model.Questioning),
                new SqlParameter("@ProblemSolving", model.ProblemSolving),
                new SqlParameter("@Collaboration", model.Collaboration),
                new SqlParameter("@TaskCompletion", model.TaskCompletion),
                new SqlParameter("@Motivation", model.Motivation),
                new SqlParameter("@MarkedBy", model.MarkedBy)
            );

            return rows > 0;
        }

        /// <summary>
        /// Soft delete a checklist.
        /// </summary>
        public bool SoftDelete(int checklistId)
        {
            return ExecuteNonQuery("sp_SoftDeleteEngagementChecklist", new SqlParameter("@ChecklistID", checklistId)) > 0;
        }

        private EngagementChecklist Map(SqlDataReader r)
        {
            return new EngagementChecklist
            {
                ChecklistID = GetValue<int>(r, "ChecklistID"),
                ClassStudentID = GetValue<int>(r, "ClassStudentID"),
                ProjectID = GetValue<int>(r, "ProjectID"),
                WeekNumber = GetValue<int>(r, "WeekNumber"),
                Participation = GetValue<bool>(r, "Participation"),
                Questioning = GetValue<bool>(r, "Questioning"),
                ProblemSolving = GetValue<bool>(r, "ProblemSolving"),
                Collaboration = GetValue<bool>(r, "Collaboration"),
                TaskCompletion = GetValue<bool>(r, "TaskCompletion"),
                Motivation = GetValue<bool>(r, "Motivation"),
                MarkedBy = GetValue<int>(r, "MarkedBy"),
                MarkedAt = GetValue<DateTime>(r, "MarkedAt"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}