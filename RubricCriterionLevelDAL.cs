using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class RubricCriterionLevelDAL : BaseDAL
    {
        public List<RubricCriterionLevel> GetByCriterion(int criterionId)
        {
            List<RubricCriterionLevel> list = new List<RubricCriterionLevel>();
            using (SqlDataReader r = ExecuteReader("sp_GetLevelsByCriterion", new SqlParameter("@CriterionID", criterionId)))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public RubricCriterionLevel GetById(int criterionLevelId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetLevelById", new SqlParameter("@CriterionLevelID", criterionLevelId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(RubricCriterionLevel model)
        {
            SqlParameter outId = new SqlParameter("@NewCriterionLevelID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateLevel",
                new SqlParameter("@CriterionID", model.CriterionID),
                new SqlParameter("@LevelName", model.LevelName),
                new SqlParameter("@ScoreValue", model.ScoreValue),
                new SqlParameter("@CriteriaDescription", (object)model.CriteriaDescription ?? DBNull.Value),
                new SqlParameter("@DisplayOrder", model.DisplayOrder),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(RubricCriterionLevel model)
        {
            int rows = ExecuteNonQuery("sp_UpdateLevel",
                new SqlParameter("@CriterionLevelID", model.CriterionLevelID),
                new SqlParameter("@LevelName", (object)model.LevelName ?? DBNull.Value),
                new SqlParameter("@ScoreValue", model.ScoreValue),
                new SqlParameter("@CriteriaDescription", (object)model.CriteriaDescription ?? DBNull.Value),
                new SqlParameter("@DisplayOrder", model.DisplayOrder)
            );
            return rows > 0;
        }

        public bool SoftDelete(int criterionLevelId)
        {
            return ExecuteNonQuery("sp_SoftDeleteLevel", new SqlParameter("@CriterionLevelID", criterionLevelId)) > 0;
        }

        private RubricCriterionLevel Map(SqlDataReader r)
        {
            return new RubricCriterionLevel
            {
                CriterionLevelID = GetValue<int>(r, "CriterionLevelID"),
                CriterionID = GetValue<int>(r, "CriterionID"),
                LevelName = GetValue<string>(r, "LevelName"),
                ScoreValue = GetValue<decimal>(r, "ScoreValue"),
                CriteriaDescription = GetValue<string>(r, "CriteriaDescription"),
                DisplayOrder = GetValue<int>(r, "DisplayOrder"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}