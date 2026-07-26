// ============================================================
// DAL/ReportCardDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ReportCardDAL : BaseDAL
    {
        public List<ReportCard> GetByStudent(int studentId, int? academicYearId = null)
        {
            var list = new List<ReportCard>();
            using (SqlDataReader r = ExecuteReader("sp_GetReportCardsByStudent",
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@AcademicYearID", (object)academicYearId ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public ReportCard GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetReportCardById", new SqlParameter("@ReportCardID", id)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(ReportCard model)
        {
            SqlParameter outId = new SqlParameter("@NewReportCardID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateReportCard",
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@AcademicYearID", model.AcademicYearID),
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@GeneratedBy", model.GeneratedBy),
                new SqlParameter("@ReportType", model.ReportType),
                new SqlParameter("@FilePath", (object)model.FilePath ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(ReportCard model)
        {
            int rows = ExecuteNonQuery("sp_UpdateReportCard",
                new SqlParameter("@ReportCardID", model.ReportCardID),
                new SqlParameter("@FilePath", (object)model.FilePath ?? DBNull.Value));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteReportCard", new SqlParameter("@ReportCardID", id)) > 0;
        }

        private ReportCard Map(SqlDataReader r)
        {
            return new ReportCard
            {
                ReportCardID = GetValue<int>(r, "ReportCardID"),
                StudentID = GetValue<int>(r, "StudentID"),
                AcademicYearID = GetValue<int>(r, "AcademicYearID"),
                ClassID = GetValue<int>(r, "ClassID"),
                GeneratedDate = GetValue<DateTime>(r, "GeneratedDate"),
                GeneratedBy = GetValue<string>(r, "GeneratedBy"),
                ReportType = GetValue<string>(r, "ReportType"),
                FilePath = GetValue<string>(r, "FilePath"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted"),
                StudentName = GetValue<string>(r, "StudentName"),
                ClassName = GetValue<string>(r, "ClassName")
            };
        }
    }
}