// ============================================================
// BLL/SemesterBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class SemesterBLL
    {
        private readonly SemesterDAL _dal = new SemesterDAL();

        public Response<List<Semester>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<Semester>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Semester>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<Semester> GetById(int id)
        {
            if (id <= 0) return Response<Semester>.Failure("Invalid Semester ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<Semester>.Failure("Semester not found.", "NOT_FOUND")
                    : Response<Semester>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<Semester>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Semester>> GetBySchoolYear(int schoolYearId)
        {
            if (schoolYearId <= 0) return Response<List<Semester>>.Failure("Invalid SchoolYear ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetBySchoolYear(schoolYearId);
                return Response<List<Semester>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Semester>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<Semester> GetCurrent(int schoolYearId)
        {
            if (schoolYearId <= 0) return Response<Semester>.Failure("Invalid SchoolYear ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetCurrent(schoolYearId);
                return data == null
                    ? Response<Semester>.Failure("No current semester found.", "NOT_FOUND")
                    : Response<Semester>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<Semester>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(Semester model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.SemesterName) || model.SchoolYearID <= 0)
                return Response<int>.Failure("Invalid semester data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Semester created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(Semester model)
        {
            if (model == null || model.SemesterID <= 0)
                return Response<bool>.Failure("Invalid semester data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Semester updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Semester ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Semester deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}