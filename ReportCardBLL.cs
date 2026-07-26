// ============================================================
// BLL/ReportCardBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class ReportCardBLL
    {
        private readonly ReportCardDAL _dal = new ReportCardDAL();

        public Response<List<ReportCard>> GetByStudent(int studentId, int? academicYearId = null)
        {
            if (studentId <= 0) return Response<List<ReportCard>>.Failure("Invalid Student ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetByStudent(studentId, academicYearId);
                return Response<List<ReportCard>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<ReportCard>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<ReportCard> GetById(int id)
        {
            if (id <= 0) return Response<ReportCard>.Failure("Invalid ReportCard ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<ReportCard>.Failure("Report card not found.", "NOT_FOUND")
                    : Response<ReportCard>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<ReportCard>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(ReportCard model)
        {
            if (model == null || model.StudentID <= 0 || model.AcademicYearID <= 0 || model.ClassID <= 0)
                return Response<int>.Failure("Invalid report card data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Report card generated.")
                    : Response<int>.Failure("Generation failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(ReportCard model)
        {
            if (model == null || model.ReportCardID <= 0)
                return Response<bool>.Failure("Invalid report card data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Report card updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid ReportCard ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Report card deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}