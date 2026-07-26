// ============================================================
// DAL/SchoolDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SchoolDAL : BaseDAL
    {
        public List<School> GetAll()
        {
            var list = new List<School>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllSchools"))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public School GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSchoolById", new SqlParameter("@SchoolID", id)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(School model)
        {
            SqlParameter outId = new SqlParameter("@NewSchoolID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateSchool",
                new SqlParameter("@SchoolName", model.SchoolName),
                new SqlParameter("@Address", (object)model.Address ?? DBNull.Value),
                new SqlParameter("@PhoneNumber", (object)model.PhoneNumber ?? DBNull.Value),
                new SqlParameter("@Email", (object)model.Email ?? DBNull.Value),
                new SqlParameter("@Website", (object)model.Website ?? DBNull.Value),
                new SqlParameter("@LogoPath", (object)model.LogoPath ?? DBNull.Value),
                new SqlParameter("@HeadmasterID", (object)model.HeadmasterID ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(School model)
        {
            int rows = ExecuteNonQuery("sp_UpdateSchool",
                new SqlParameter("@SchoolID", model.SchoolID),
                new SqlParameter("@SchoolName", (object)model.SchoolName ?? DBNull.Value),
                new SqlParameter("@Address", (object)model.Address ?? DBNull.Value),
                new SqlParameter("@PhoneNumber", (object)model.PhoneNumber ?? DBNull.Value),
                new SqlParameter("@Email", (object)model.Email ?? DBNull.Value),
                new SqlParameter("@Website", (object)model.Website ?? DBNull.Value),
                new SqlParameter("@LogoPath", (object)model.LogoPath ?? DBNull.Value),
                new SqlParameter("@HeadmasterID", (object)model.HeadmasterID ?? DBNull.Value));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteSchool", new SqlParameter("@SchoolID", id)) > 0;
        }

        private School Map(SqlDataReader r)
        {
            return new School
            {
                SchoolID = GetValue<int>(r, "SchoolID"),
                SchoolName = GetValue<string>(r, "SchoolName"),
                Address = GetValue<string>(r, "Address"),
                PhoneNumber = GetValue<string>(r, "PhoneNumber"),
                Email = GetValue<string>(r, "Email"),
                Website = GetValue<string>(r, "Website"),
                LogoPath = GetValue<string>(r, "LogoPath"),
                HeadmasterID = GetValue<int?>(r, "HeadmasterID"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}