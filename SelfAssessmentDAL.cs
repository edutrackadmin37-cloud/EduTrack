// ============================================================
// DAL/SelfAssessmentDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SelfAssessmentDAL : BaseDAL
    {
        public List<SelfAssessment> GetByStudent(int studentId)
        {
            var list = new List<SelfAssessment>();
            using (SqlDataReader r = ExecuteReader("sp_GetSelfAssessmentsByStudent", new SqlParameter("@StudentID", studentId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<SelfAssessment> GetByProject(int projectId)
        {
            var list = new List<SelfAssessment>();
            using (SqlDataReader r = ExecuteReader("sp_GetSelfAssessmentsByProject", new SqlParameter("@ProjectID", projectId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public SelfAssessment GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSelfAssessmentById", new SqlParameter("@SelfAssessmentID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public bool Exists(int studentId, int projectId)
        {
            using (SqlDataReader r = ExecuteReader("sp_CheckSelfAssessmentExists",
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@ProjectID", projectId)))
            {
                return r.Read();
            }
        }

        public int Create(SelfAssessment model)
        {
            SqlParameter outId = new SqlParameter("@NewSelfAssessmentID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateSelfAssessment",
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@ProjectID", model.ProjectID),
                new SqlParameter("@RubricID", (object)model.RubricID ?? DBNull.Value),
                new SqlParameter("@Score", model.Score),
                new SqlParameter("@Reflection", (object)model.Reflection ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(SelfAssessment model)
        {
            int rows = ExecuteNonQuery("sp_UpdateSelfAssessment",
                new SqlParameter("@SelfAssessmentID", model.SelfAssessmentID),
                new SqlParameter("@Score", model.Score),
                new SqlParameter("@Reflection", (object)model.Reflection ?? DBNull.Value));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteSelfAssessment", new SqlParameter("@SelfAssessmentID", id)) > 0;
        }

        private SelfAssessment Map(SqlDataReader r)
        {
            return new SelfAssessment
            {
                SelfAssessmentID = GetValue<int>(r, "SelfAssessmentID"),
                StudentID = GetValue<int>(r, "StudentID"),
                ProjectID = GetValue<int>(r, "ProjectID"),
                RubricID = GetValue<int?>(r, "RubricID"),
                Score = GetValue<int>(r, "Score"),
                Reflection = GetValue<string>(r, "Reflection"),
                AssessedAt = GetValue<DateTime>(r, "AssessedAt"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private SelfAssessment MapWithContext(SqlDataReader r)
        {
            var sa = Map(r);
            sa.StudentName = GetValue<string>(r, "StudentName");
            sa.ProjectTitle = GetValue<string>(r, "ProjectTitle");
            return sa;
        }
    }
}
