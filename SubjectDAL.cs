using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SubjectDAL : BaseDAL
    {
        public List<Subject> GetAll()
        {
            List<Subject> list = new List<Subject>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllSubjects"))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Subject GetById(int subjectId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSubjectById", new SqlParameter("@SubjectID", subjectId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Subject subject)
        {
            SqlParameter outId = new SqlParameter("@NewSubjectID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            ExecuteNonQuery("sp_CreateSubject",
                new SqlParameter("@SubjectName", subject.SubjectName),
                new SqlParameter("@SubjectCode", (object)subject.SubjectCode ?? DBNull.Value),
                new SqlParameter("@Description", (object)subject.Description ?? DBNull.Value),
                new SqlParameter("@IsCore", subject.IsCore),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Subject subject)
        {
            int rows = ExecuteNonQuery("sp_UpdateSubject",
                new SqlParameter("@SubjectID", subject.SubjectID),
                new SqlParameter("@SubjectName", (object)subject.SubjectName ?? DBNull.Value),
                new SqlParameter("@SubjectCode", (object)subject.SubjectCode ?? DBNull.Value),
                new SqlParameter("@Description", (object)subject.Description ?? DBNull.Value),
                new SqlParameter("@IsCore", subject.IsCore)
            );
            return rows > 0;
        }

        public bool SoftDelete(int subjectId)
        {
            return ExecuteNonQuery("sp_SoftDeleteSubject", new SqlParameter("@SubjectID", subjectId)) > 0;
        }

        private Subject Map(SqlDataReader r)
        {
            return new Subject
            {
                SubjectID = GetValue<int>(r, "SubjectID"),
                SubjectName = GetValue<string>(r, "SubjectName"),
                SubjectCode = GetValue<string>(r, "SubjectCode"),
                Description = GetValue<string>(r, "Description"),
                IsCore = GetValue<bool>(r, "IsCore"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}