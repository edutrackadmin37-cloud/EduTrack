// ============================================================
// BLL/DiscussionBoardBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class DiscussionBoardBLL
    {
        private readonly DiscussionBoardDAL _dal = new DiscussionBoardDAL();

        public Response<List<DiscussionBoard>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<DiscussionBoard>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<DiscussionBoard>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<DiscussionBoard>> GetBySubject(int subjectId)
        {
            if (subjectId <= 0) return Response<List<DiscussionBoard>>.Failure("Invalid Subject ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetBySubject(subjectId);
                return Response<List<DiscussionBoard>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<DiscussionBoard>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<DiscussionBoard>> GetByClass(int classId)
        {
            if (classId <= 0) return Response<List<DiscussionBoard>>.Failure("Invalid Class ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByClass(classId);
                return Response<List<DiscussionBoard>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<DiscussionBoard>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<DiscussionBoard>> GetByProject(int projectId)
        {
            if (projectId <= 0) return Response<List<DiscussionBoard>>.Failure("Invalid Project ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByProject(projectId);
                return Response<List<DiscussionBoard>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<DiscussionBoard>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<DiscussionBoard> GetById(int id)
        {
            if (id <= 0) return Response<DiscussionBoard>.Failure("Invalid Discussion ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<DiscussionBoard>.Failure("Discussion not found.", "NOT_FOUND")
                    : Response<DiscussionBoard>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<DiscussionBoard>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(DiscussionBoard model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Title) || model.SubjectID <= 0 || model.PostedBy <= 0)
                return Response<int>.Failure("Invalid discussion data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Discussion posted.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(DiscussionBoard model)
        {
            if (model == null || model.DiscussionID <= 0)
                return Response<bool>.Failure("Invalid discussion data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Discussion updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Discussion ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Discussion deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}