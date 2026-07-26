using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class RubricLevelBLL
    {
        private readonly RubricCriterionLevelDAL _dal = new RubricCriterionLevelDAL();

        public Response<List<RubricCriterionLevel>> GetByCriterion(int criterionId)
        {
            if (criterionId <= 0)
                return Response<List<RubricCriterionLevel>>.Failure("Invalid criterion ID.", "VALIDATION_ERROR");
            return Response<List<RubricCriterionLevel>>.Success(_dal.GetByCriterion(criterionId));
        }

        public Response<RubricCriterionLevel> GetById(int levelId)
        {
            if (levelId <= 0)
                return Response<RubricCriterionLevel>.Failure("Invalid level ID.", "VALIDATION_ERROR");
            var item = _dal.GetById(levelId);
            return item == null ? Response<RubricCriterionLevel>.Failure("Not found.", "NOT_FOUND") : Response<RubricCriterionLevel>.Success(item);
        }

        public Response<int> Create(RubricCriterionLevel level)
        {
            if (level == null || level.CriterionID <= 0 || string.IsNullOrWhiteSpace(level.LevelName))
                return Response<int>.Failure("Invalid level data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(level);
                return id > 0 ? Response<int>.Success(id, "Level created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(RubricCriterionLevel level)
        {
            if (level == null || level.CriterionLevelID <= 0)
                return Response<bool>.Failure("Invalid level data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(level);
                return ok ? Response<bool>.Success(true, "Level updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int levelId)
        {
            if (levelId <= 0)
                return Response<bool>.Failure("Invalid level ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.SoftDelete(levelId);
                return ok ? Response<bool>.Success(true, "Level deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}