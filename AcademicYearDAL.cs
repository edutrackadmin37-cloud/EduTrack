using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class AcademicYearDAL : BaseDAL
    {
        public List<AcademicYear> GetAll()
        {
            List<AcademicYear> list = new List<AcademicYear>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllAcademicYears"))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public AcademicYear GetById(int academicYearId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetAcademicYearById", new SqlParameter("@AcademicYearID", academicYearId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public AcademicYear GetCurrent()
        {
            using (SqlDataReader r = ExecuteReader("sp_GetCurrentAcademicYear"))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(AcademicYear year)
        {
            SqlParameter outId = new SqlParameter("@NewAcademicYearID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateAcademicYear",
                new SqlParameter("@YearName", year.YearName),
                new SqlParameter("@StartDate", year.StartDate),
                new SqlParameter("@EndDate", year.EndDate),
                new SqlParameter("@IsCurrent", year.IsCurrent),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(AcademicYear year)
        {
            int rows = ExecuteNonQuery("sp_UpdateAcademicYear",
                new SqlParameter("@AcademicYearID", year.AcademicYearID),
                new SqlParameter("@YearName", (object)year.YearName ?? DBNull.Value),
                new SqlParameter("@StartDate", year.StartDate),
                new SqlParameter("@EndDate", year.EndDate),
                new SqlParameter("@IsCurrent", year.IsCurrent)
            );
            return rows > 0;
        }

        // ============================================================
         // DAL/AcademicYearDAL.cs - Add this method
         // ============================================================
        public bool SoftDelete(int academicYearId)
        {
            return ExecuteNonQuery("sp_SoftDeleteAcademicYear", new SqlParameter("@AcademicYearID", academicYearId)) > 0;
        }

        private AcademicYear Map(SqlDataReader r)
        {
            return new AcademicYear
            {
                AcademicYearID = GetValue<int>(r, "AcademicYearID"),
                YearName = GetValue<string>(r, "YearName"),
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