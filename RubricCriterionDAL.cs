using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class RubricCriterionDAL : BaseDAL
    {
        public List<RubricCriterion> GetByRubric(int rubricId)
        {
            List<RubricCriterion> list = new List<RubricCriterion>();
            using (SqlDataReader r = ExecuteReader("sp_GetCriteriaByRubric", new SqlParameter("@RubricID", rubricId)))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public RubricCriterion GetById(int criterionId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetCriterionById", new SqlParameter("@CriterionID", criterionId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(RubricCriterion model)
        {
            SqlParameter outId = new SqlParameter("@NewCriterionID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateCriterion",
                new SqlParameter("@RubricID", model.RubricID),
                new SqlParameter("@CriterionName", model.CriterionName),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@Weight", model.Weight),
                new SqlParameter("@DisplayOrder", model.DisplayOrder),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(RubricCriterion model)
        {
            int rows = ExecuteNonQuery("sp_UpdateCriterion",
                new SqlParameter("@CriterionID", model.CriterionID),
                new SqlParameter("@CriterionName", (object)model.CriterionName ?? DBNull.Value),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@Weight", model.Weight),
                new SqlParameter("@DisplayOrder", model.DisplayOrder)
            );
            return rows > 0;
        }

        public bool SoftDelete(int criterionId)
        {
            return ExecuteNonQuery("sp_SoftDeleteCriterion", new SqlParameter("@CriterionID", criterionId)) > 0;
        }

        private RubricCriterion Map(SqlDataReader r)
        {
            return new RubricCriterion
            {
                CriterionID = GetValue<int>(r, "CriterionID"),
                RubricID = GetValue<int>(r, "RubricID"),
                CriterionName = GetValue<string>(r, "CriterionName"),
                Description = GetValue<string>(r, "Description"),
                Weight = GetValue<decimal>(r, "Weight"),
                DisplayOrder = GetValue<int>(r, "DisplayOrder"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}