// ============================================================
// DAL/SchoolYearDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SchoolYearDAL : BaseDAL
    {
        public List<SchoolYear> GetAll()
        {
            var list = new List<SchoolYear>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllSchoolYears"))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public SchoolYear GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSchoolYearById", new SqlParameter("@SchoolYearID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public List<SchoolYear> GetBySchool(int schoolId)
        {
            var list = new List<SchoolYear>();
            using (SqlDataReader r = ExecuteReader("sp_GetSchoolYearsBySchool", new SqlParameter("@SchoolID", schoolId)))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public SchoolYear GetCurrent(int schoolId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetCurrentSchoolYear", new SqlParameter("@SchoolID", schoolId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(SchoolYear model)
        {
            SqlParameter outId = new SqlParameter("@NewSchoolYearID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateSchoolYear",
                new SqlParameter("@SchoolID", model.SchoolID),
                new SqlParameter("@YearName", model.YearName),
                new SqlParameter("@StartDate", model.StartDate),
                new SqlParameter("@EndDate", model.EndDate),
                new SqlParameter("@IsCurrent", model.IsCurrent),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(SchoolYear model)
        {
            int rows = ExecuteNonQuery("sp_UpdateSchoolYear",
                new SqlParameter("@SchoolYearID", model.SchoolYearID),
                new SqlParameter("@YearName", (object)model.YearName ?? DBNull.Value),
                new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                new SqlParameter("@IsCurrent", model.IsCurrent));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteSchoolYear", new SqlParameter("@SchoolYearID", id)) > 0;
        }

        private SchoolYear Map(SqlDataReader r)
        {
            return new SchoolYear
            {
                SchoolYearID = GetValue<int>(r, "SchoolYearID"),
                SchoolID = GetValue<int>(r, "SchoolID"),
                YearName = GetValue<string>(r, "YearName"),
                StartDate = GetValue<DateTime>(r, "StartDate"),
                EndDate = GetValue<DateTime>(r, "EndDate"),
                IsCurrent = GetValue<bool>(r, "IsCurrent"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private SchoolYear MapWithContext(SqlDataReader r)
        {
            var sy = Map(r);
            sy.SchoolName = GetValue<string>(r, "SchoolName");
            return sy;
        }
    }
}