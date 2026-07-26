// ============================================================
// DAL/SemesterDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SemesterDAL : BaseDAL
    {
        public List<Semester> GetAll()
        {
            var list = new List<Semester>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllSemesters"))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public Semester GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSemesterById", new SqlParameter("@SemesterID", id)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public List<Semester> GetBySchoolYear(int schoolYearId)
        {
            var list = new List<Semester>();
            using (SqlDataReader r = ExecuteReader("sp_GetSemestersBySchoolYear", new SqlParameter("@SchoolYearID", schoolYearId)))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public Semester GetCurrent(int schoolYearId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetCurrentSemester", new SqlParameter("@SchoolYearID", schoolYearId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Semester model)
        {
            SqlParameter outId = new SqlParameter("@NewSemesterID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateSemester",
                new SqlParameter("@SchoolYearID", model.SchoolYearID),
                new SqlParameter("@SemesterName", model.SemesterName),
                new SqlParameter("@StartDate", model.StartDate),
                new SqlParameter("@EndDate", model.EndDate),
                new SqlParameter("@IsCurrent", model.IsCurrent),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Semester model)
        {
            int rows = ExecuteNonQuery("sp_UpdateSemester",
                new SqlParameter("@SemesterID", model.SemesterID),
                new SqlParameter("@SemesterName", (object)model.SemesterName ?? DBNull.Value),
                new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                new SqlParameter("@IsCurrent", model.IsCurrent));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteSemester", new SqlParameter("@SemesterID", id)) > 0;
        }

        private Semester Map(SqlDataReader r)
        {
            return new Semester
            {
                SemesterID = GetValue<int>(r, "SemesterID"),
                SchoolYearID = GetValue<int>(r, "SchoolYearID"),
                SemesterName = GetValue<string>(r, "SemesterName"),
                StartDate = GetValue<DateTime>(r, "StartDate"),
                EndDate = GetValue<DateTime>(r, "EndDate"),
                IsCurrent = GetValue<bool>(r, "IsCurrent"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}