// ============================================================
// BLL/SchoolYearBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class SchoolYearBLL
    {
        private readonly SchoolYearDAL _dal = new SchoolYearDAL();

        public Response<List<SchoolYear>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<SchoolYear>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<SchoolYear>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<SchoolYear> GetById(int id)
        {
            if (id <= 0) return Response<SchoolYear>.Failure("Invalid SchoolYear ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<SchoolYear>.Failure("SchoolYear not found.", "NOT_FOUND")
                    : Response<SchoolYear>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<SchoolYear>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<SchoolYear>> GetBySchool(int schoolId)
        {
            if (schoolId <= 0) return Response<List<SchoolYear>>.Failure("Invalid School ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetBySchool(schoolId);
                return Response<List<SchoolYear>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<SchoolYear>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<SchoolYear> GetCurrent(int schoolId)
        {
            if (schoolId <= 0) return Response<SchoolYear>.Failure("Invalid School ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetCurrent(schoolId);
                return data == null
                    ? Response<SchoolYear>.Failure("No current school year found.", "NOT_FOUND")
                    : Response<SchoolYear>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<SchoolYear>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(SchoolYear model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.YearName) || model.SchoolID <= 0)
                return Response<int>.Failure("Invalid school year data.", "VALIDATION_ERROR");

            try
            {
                // If setting as current, update others first (handled in DAL)
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "School year created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(SchoolYear model)
        {
            if (model == null || model.SchoolYearID <= 0)
                return Response<bool>.Failure("Invalid school year data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "School year updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid SchoolYear ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "School year deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}