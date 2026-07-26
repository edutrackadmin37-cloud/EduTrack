using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class RubricCriterionBLL
    {
        private readonly RubricCriterionDAL _dal = new RubricCriterionDAL();

        public Response<List<RubricCriterion>> GetByRubric(int rubricId)
        {
            if (rubricId <= 0)
                return Response<List<RubricCriterion>>.Failure("Invalid rubric ID.", "VALIDATION_ERROR");
            return Response<List<RubricCriterion>>.Success(_dal.GetByRubric(rubricId));
        }

        public Response<RubricCriterion> GetById(int criterionId)
        {
            if (criterionId <= 0)
                return Response<RubricCriterion>.Failure("Invalid criterion ID.", "VALIDATION_ERROR");
            var item = _dal.GetById(criterionId);
            return item == null ? Response<RubricCriterion>.Failure("Not found.", "NOT_FOUND") : Response<RubricCriterion>.Success(item);
        }

        public Response<int> Create(RubricCriterion criterion)
        {
            if (criterion == null || criterion.RubricID <= 0 || string.IsNullOrWhiteSpace(criterion.CriterionName))
                return Response<int>.Failure("Invalid criterion data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(criterion);
                return id > 0 ? Response<int>.Success(id, "Criterion created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(RubricCriterion criterion)
        {
            if (criterion == null || criterion.CriterionID <= 0)
                return Response<bool>.Failure("Invalid criterion data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(criterion);
                return ok ? Response<bool>.Success(true, "Criterion updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int criterionId)
        {
            if (criterionId <= 0)
                return Response<bool>.Failure("Invalid criterion ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.SoftDelete(criterionId);
                return ok ? Response<bool>.Success(true, "Criterion deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}