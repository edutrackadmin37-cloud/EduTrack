// ============================================================
// DAL/TermDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class TermDAL : BaseDAL
    {
        public List<Term> GetAll()
        {
            var list = new List<Term>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllTerms"))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public Term GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetTermById", new SqlParameter("@TermID", id)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public List<Term> GetBySemester(int semesterId)
        {
            var list = new List<Term>();
            using (SqlDataReader r = ExecuteReader("sp_GetTermsBySemester", new SqlParameter("@SemesterID", semesterId)))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public Term GetCurrent(int semesterId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetCurrentTerm", new SqlParameter("@SemesterID", semesterId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Term model)
        {
            SqlParameter outId = new SqlParameter("@NewTermID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateTerm",
                new SqlParameter("@SemesterID", model.SemesterID),
                new SqlParameter("@TermName", model.TermName),
                new SqlParameter("@StartDate", model.StartDate),
                new SqlParameter("@EndDate", model.EndDate),
                new SqlParameter("@IsCurrent", model.IsCurrent),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Term model)
        {
            int rows = ExecuteNonQuery("sp_UpdateTerm",
                new SqlParameter("@TermID", model.TermID),
                new SqlParameter("@TermName", (object)model.TermName ?? DBNull.Value),
                new SqlParameter("@StartDate", (object)model.StartDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                new SqlParameter("@IsCurrent", model.IsCurrent));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteTerm", new SqlParameter("@TermID", id)) > 0;
        }

        private Term Map(SqlDataReader r)
        {
            return new Term
            {
                TermID = GetValue<int>(r, "TermID"),
                SemesterID = GetValue<int>(r, "SemesterID"),
                TermName = GetValue<string>(r, "TermName"),
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