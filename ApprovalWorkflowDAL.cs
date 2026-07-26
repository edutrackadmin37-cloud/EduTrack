// ============================================================
// DAL/ApprovalWorkflowDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ApprovalWorkflowDAL : BaseDAL
    {
        public List<ApprovalWorkflow> GetAll()
        {
            var list = new List<ApprovalWorkflow>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllWorkflows"))
            {
                while (r.Read())
                {
                    list.Add(MapWorkflow(r));
                }
            }
            return list;
        }

        public ApprovalWorkflow GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetWorkflowById", new SqlParameter("@WorkflowID", id)))
            {
                if (!r.Read()) return null;
                return MapWorkflow(r);
            }
        }

        public List<ApprovalStep> GetSteps(int workflowId)
        {
            var list = new List<ApprovalStep>();
            using (SqlDataReader r = ExecuteReader("sp_GetApprovalSteps", new SqlParameter("@WorkflowID", workflowId)))
            {
                while (r.Read())
                {
                    list.Add(MapStep(r));
                }
            }
            return list;
        }

        public int CreateWorkflow(ApprovalWorkflow model)
        {
            SqlParameter outId = new SqlParameter("@NewWorkflowID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateWorkflow",
                new SqlParameter("@WorkflowName", model.WorkflowName),
                new SqlParameter("@EntityType", model.EntityType),
                new SqlParameter("@IsActive", model.IsActive),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool UpdateWorkflow(ApprovalWorkflow model)
        {
            int rows = ExecuteNonQuery("sp_UpdateWorkflow",
                new SqlParameter("@WorkflowID", model.WorkflowID),
                new SqlParameter("@WorkflowName", (object)model.WorkflowName ?? DBNull.Value),
                new SqlParameter("@IsActive", model.IsActive));
            return rows > 0;
        }

        public int CreateStep(ApprovalStep model)
        {
            SqlParameter outId = new SqlParameter("@NewStepID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateApprovalStep",
                new SqlParameter("@WorkflowID", model.WorkflowID),
                new SqlParameter("@StepOrder", model.StepOrder),
                new SqlParameter("@StepName", model.StepName),
                new SqlParameter("@RequiredRole", (object)model.RequiredRole ?? DBNull.Value),
                new SqlParameter("@ApproverID", (object)model.ApproverID ?? DBNull.Value),
                new SqlParameter("@IsParallel", model.IsParallel),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool UpdateStep(ApprovalStep model)
        {
            int rows = ExecuteNonQuery("sp_UpdateApprovalStep",
                new SqlParameter("@StepID", model.StepID),
                new SqlParameter("@StepOrder", model.StepOrder),
                new SqlParameter("@StepName", (object)model.StepName ?? DBNull.Value),
                new SqlParameter("@RequiredRole", (object)model.RequiredRole ?? DBNull.Value),
                new SqlParameter("@ApproverID", (object)model.ApproverID ?? DBNull.Value),
                new SqlParameter("@IsParallel", model.IsParallel));
            return rows > 0;
        }

        public bool SoftDeleteWorkflow(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteWorkflow", new SqlParameter("@WorkflowID", id)) > 0;
        }

        public bool SoftDeleteStep(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteApprovalStep", new SqlParameter("@StepID", id)) > 0;
        }

        private ApprovalWorkflow MapWorkflow(SqlDataReader r)
        {
            return new ApprovalWorkflow
            {
                WorkflowID = GetValue<int>(r, "WorkflowID"),
                WorkflowName = GetValue<string>(r, "WorkflowName"),
                EntityType = GetValue<string>(r, "EntityType"),
                IsActive = GetValue<bool>(r, "IsActive"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private ApprovalStep MapStep(SqlDataReader r)
        {
            return new ApprovalStep
            {
                StepID = GetValue<int>(r, "StepID"),
                WorkflowID = GetValue<int>(r, "WorkflowID"),
                StepOrder = GetValue<int>(r, "StepOrder"),
                StepName = GetValue<string>(r, "StepName"),
                RequiredRole = GetValue<string>(r, "RequiredRole"),
                ApproverID = GetValue<int?>(r, "ApproverID"),
                IsParallel = GetValue<bool>(r, "IsParallel"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}