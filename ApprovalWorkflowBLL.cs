// ============================================================
// BLL/ApprovalWorkflowBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ApprovalWorkflowBLL
    {
        private readonly ApprovalWorkflowDAL _dal = new ApprovalWorkflowDAL();

        public Response<List<ApprovalWorkflow>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<ApprovalWorkflow>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<ApprovalWorkflow>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ApprovalWorkflow> GetById(int id)
        {
            if (id <= 0) return Response<ApprovalWorkflow>.Failure("Invalid Workflow ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<ApprovalWorkflow>.Failure("Workflow not found.", "NOT_FOUND")
                    : Response<ApprovalWorkflow>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<ApprovalWorkflow>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<ApprovalStep>> GetSteps(int workflowId)
        {
            if (workflowId <= 0) return Response<List<ApprovalStep>>.Failure("Invalid Workflow ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetSteps(workflowId);
                return Response<List<ApprovalStep>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<ApprovalStep>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> CreateWorkflow(ApprovalWorkflow model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.WorkflowName) || string.IsNullOrWhiteSpace(model.EntityType))
                return Response<int>.Failure("Invalid workflow data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.CreateWorkflow(model);
                return id > 0
                    ? Response<int>.Success(id, "Workflow created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> UpdateWorkflow(ApprovalWorkflow model)
        {
            if (model == null || model.WorkflowID <= 0)
                return Response<bool>.Failure("Invalid workflow data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.UpdateWorkflow(model);
                return ok
                    ? Response<bool>.Success(true, "Workflow updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> CreateStep(ApprovalStep model)
        {
            if (model == null || model.WorkflowID <= 0 || string.IsNullOrWhiteSpace(model.StepName))
                return Response<int>.Failure("Invalid step data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.CreateStep(model);
                return id > 0
                    ? Response<int>.Success(id, "Approval step created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> UpdateStep(ApprovalStep model)
        {
            if (model == null || model.StepID <= 0)
                return Response<bool>.Failure("Invalid step data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.UpdateStep(model);
                return ok
                    ? Response<bool>.Success(true, "Approval step updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDeleteWorkflow(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Workflow ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDeleteWorkflow(id);
                return ok
                    ? Response<bool>.Success(true, "Workflow deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDeleteStep(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Step ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDeleteStep(id);
                return ok
                    ? Response<bool>.Success(true, "Step deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}