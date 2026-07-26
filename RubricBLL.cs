using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class RubricBLL
    {
        private readonly RubricDAL _dal = new RubricDAL();

        public Response<List<Rubric>> GetAll()
        {
            return Response<List<Rubric>>.Success(_dal.GetAll());
        }

        public Response<Rubric> GetById(int rubricId)
        {
            if (rubricId <= 0)
                return Response<Rubric>.Failure("Invalid rubric ID.", "VALIDATION_ERROR");
            var rubric = _dal.GetById(rubricId);
            return rubric == null ? Response<Rubric>.Failure("Not found.", "NOT_FOUND") : Response<Rubric>.Success(rubric);
        }

        public Response<int> CreateRubric(Rubric rubric)
        {
            if (rubric == null || string.IsNullOrWhiteSpace(rubric.Title))
                return Response<int>.Failure("Rubric title is required.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(rubric);
                return id > 0 ? Response<int>.Success(id, "Rubric created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> UpdateRubric(Rubric rubric)
        {
            if (rubric == null || rubric.RubricID <= 0)
                return Response<bool>.Failure("Invalid rubric data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(rubric);
                return ok ? Response<bool>.Success(true, "Rubric updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDeleteRubric(int rubricId)
        {
            if (rubricId <= 0)
                return Response<bool>.Failure("Invalid rubric ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.SoftDelete(rubricId);
                return ok ? Response<bool>.Success(true, "Rubric deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}