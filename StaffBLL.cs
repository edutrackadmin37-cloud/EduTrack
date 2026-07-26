// ============================================================
// BLL/StaffBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class StaffBLL
    {
        private readonly StaffDAL _dal = new StaffDAL();

        public Response<List<Staff>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<Staff>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Staff>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<Staff> GetById(int id)
        {
            if (id <= 0) return Response<Staff>.Failure("Invalid Staff ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<Staff>.Failure("Staff not found.", "NOT_FOUND")
                    : Response<Staff>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<Staff>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Staff>> GetBySchool(int schoolId)
        {
            if (schoolId <= 0) return Response<List<Staff>>.Failure("Invalid School ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetBySchool(schoolId);
                return Response<List<Staff>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Staff>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Staff>> GetByDepartment(int departmentId)
        {
            if (departmentId <= 0) return Response<List<Staff>>.Failure("Invalid Department ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByDepartment(departmentId);
                return Response<List<Staff>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Staff>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(Staff model)
        {
            if (model == null || model.UserID <= 0)
                return Response<int>.Failure("User ID is required.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Staff record created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(Staff model)
        {
            if (model == null || model.StaffID <= 0)
                return Response<bool>.Failure("Invalid staff data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Staff record updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Staff ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Staff record deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}