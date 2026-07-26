// ============================================================
// BLL/SchoolBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class SchoolBLL
    {
        private readonly SchoolDAL _dal = new SchoolDAL();

        public Response<List<School>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<School>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<School>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<School> GetById(int id)
        {
            if (id <= 0) return Response<School>.Failure("Invalid School ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<School>.Failure("School not found.", "NOT_FOUND")
                    : Response<School>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<School>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(School model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.SchoolName))
                return Response<int>.Failure("School name is required.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "School created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(School model)
        {
            if (model == null || model.SchoolID <= 0)
                return Response<bool>.Failure("Invalid school data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "School updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid School ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "School deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}