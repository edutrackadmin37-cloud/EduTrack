// ============================================================
// BLL/TermBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class TermBLL
    {
        private readonly TermDAL _dal = new TermDAL();

        public Response<List<Term>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<Term>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Term>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<Term> GetById(int id)
        {
            if (id <= 0) return Response<Term>.Failure("Invalid Term ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<Term>.Failure("Term not found.", "NOT_FOUND")
                    : Response<Term>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<Term>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Term>> GetBySemester(int semesterId)
        {
            if (semesterId <= 0) return Response<List<Term>>.Failure("Invalid Semester ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetBySemester(semesterId);
                return Response<List<Term>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<Term>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<Term> GetCurrent(int semesterId)
        {
            if (semesterId <= 0) return Response<Term>.Failure("Invalid Semester ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetCurrent(semesterId);
                return data == null
                    ? Response<Term>.Failure("No current term found.", "NOT_FOUND")
                    : Response<Term>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<Term>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(Term model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.TermName) || model.SemesterID <= 0)
                return Response<int>.Failure("Invalid term data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Term created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(Term model)
        {
            if (model == null || model.TermID <= 0)
                return Response<bool>.Failure("Invalid term data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Term updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Term ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Term deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}