using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class GradeDAL : BaseDAL
    {
        // NEW: Get all grades
        public List<Grade> GetAll()
        {
            var list = new List<Grade>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllGrades"))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public List<Grade> GetBySubmission(int submissionId)
        {
            List<Grade> list = new List<Grade>();
            using (SqlDataReader r = ExecuteReader("sp_GetGradesBySubmission", new SqlParameter("@SubmissionID", submissionId)))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Grade GetById(int gradeId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetGradeById", new SqlParameter("@GradeID", gradeId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Grade model)
        {
            SqlParameter outId = new SqlParameter("@NewGradeID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateGrade",
                new SqlParameter("@SubmissionID", model.SubmissionID),
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@GradeValue", (object)model.GradeValue ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)model.Remarks ?? DBNull.Value),
                new SqlParameter("@GradedBy", model.GradedBy),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Grade model)
        {
            int rows = ExecuteNonQuery("sp_UpdateGrade",
                new SqlParameter("@GradeID", model.GradeID),
                new SqlParameter("@GradeValue", (object)model.GradeValue ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)model.Remarks ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int gradeId)
        {
            return ExecuteNonQuery("sp_SoftDeleteGrade", new SqlParameter("@GradeID", gradeId)) > 0;
        }

        private Grade Map(SqlDataReader r)
        {
            return new Grade
            {
                GradeID = GetValue<int>(r, "GradeID"),
                SubmissionID = GetValue<int>(r, "SubmissionID"),
                StudentID = GetValue<int>(r, "StudentID"),
                GradeValue = GetValue<decimal?>(r, "GradeValue"),
                Remarks = GetValue<string>(r, "Remarks"),
                DateGraded = GetValue<DateTime>(r, "DateGraded"),
                GradedBy = GetValue<int>(r, "GradedBy"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}