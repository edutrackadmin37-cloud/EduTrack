using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class RubricDAL : BaseDAL
    {
        public List<Rubric> GetAll()
        {
            List<Rubric> list = new List<Rubric>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllRubrics"))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Rubric GetById(int rubricId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetRubricById", new SqlParameter("@RubricID", rubricId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Rubric model)
        {
            SqlParameter outId = new SqlParameter("@NewRubricID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateRubric",
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Rubric model)
        {
            int rows = ExecuteNonQuery("sp_UpdateRubric",
                new SqlParameter("@RubricID", model.RubricID),
                new SqlParameter("@Title", (object)model.Title ?? DBNull.Value),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int rubricId)
        {
            return ExecuteNonQuery("sp_SoftDeleteRubric", new SqlParameter("@RubricID", rubricId)) > 0;
        }

        private Rubric Map(SqlDataReader r)
        {
            return new Rubric
            {
                RubricID = GetValue<int>(r, "RubricID"),
                Title = GetValue<string>(r, "Title"),
                Description = GetValue<string>(r, "Description"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}