using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ProgrammeDAL : BaseDAL
    {
        public List<Programme> GetAll()
        {
            List<Programme> list = new List<Programme>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllProgrammes"))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Programme GetById(int programmeId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetProgrammeById", new SqlParameter("@ProgrammeID", programmeId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Programme programme)
        {
            SqlParameter outId = new SqlParameter("@NewProgrammeID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            ExecuteNonQuery("sp_CreateProgramme",
                new SqlParameter("@ProgrammeName", programme.ProgrammeName),
                new SqlParameter("@Description", (object)programme.Description ?? DBNull.Value),
                new SqlParameter("@DepartmentID", programme.DepartmentID),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Programme programme)
        {
            int rows = ExecuteNonQuery("sp_UpdateProgramme",
                new SqlParameter("@ProgrammeID", programme.ProgrammeID),
                new SqlParameter("@ProgrammeName", (object)programme.ProgrammeName ?? DBNull.Value),
                new SqlParameter("@Description", (object)programme.Description ?? DBNull.Value),
                new SqlParameter("@DepartmentID", programme.DepartmentID)
            );
            return rows > 0;
        }

        public bool SoftDelete(int programmeId)
        {
            return ExecuteNonQuery("sp_SoftDeleteProgramme", new SqlParameter("@ProgrammeID", programmeId)) > 0;
        }

        private Programme Map(SqlDataReader r)
        {
            return new Programme
            {
                ProgrammeID = GetValue<int>(r, "ProgrammeID"),
                ProgrammeName = GetValue<string>(r, "ProgrammeName"),
                Description = GetValue<string>(r, "Description"),
                DepartmentID = GetValue<int>(r, "DepartmentID"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}