// ============================================================
// DAL/ContinuousAssessmentDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ContinuousAssessmentDAL : BaseDAL
    {
        public List<ContinuousAssessment> GetByStudent(int studentId)
        {
            var list = new List<ContinuousAssessment>();
            using (SqlDataReader r = ExecuteReader("sp_GetContinuousAssessmentsByStudent", new SqlParameter("@StudentID", studentId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<ContinuousAssessment> GetByClassSubject(int classId, int subjectId, int academicYearId)
        {
            var list = new List<ContinuousAssessment>();
            using (SqlDataReader r = ExecuteReader("sp_GetContinuousAssessmentsByClassSubject",
                new SqlParameter("@ClassID", classId),
                new SqlParameter("@SubjectID", subjectId),
                new SqlParameter("@AcademicYearID", academicYearId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public ContinuousAssessment GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetContinuousAssessmentById", new SqlParameter("@ContinuousAssessmentID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public ContinuousAssessment GetByStudentSubject(int studentId, int subjectId, int academicYearId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetContinuousAssessmentByStudentSubject",
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@SubjectID", subjectId),
                new SqlParameter("@AcademicYearID", academicYearId)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public int Create(ContinuousAssessment model)
        {
            SqlParameter outId = new SqlParameter("@NewContinuousAssessmentID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateContinuousAssessment",
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@SubjectID", model.SubjectID),
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@AcademicYearID", model.AcademicYearID),
                new SqlParameter("@CA1", model.CA1),
                new SqlParameter("@CA2", model.CA2),
                new SqlParameter("@CA3", model.CA3),
                new SqlParameter("@CA4", model.CA4),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(ContinuousAssessment model)
        {
            int rows = ExecuteNonQuery("sp_UpdateContinuousAssessment",
                new SqlParameter("@ContinuousAssessmentID", model.ContinuousAssessmentID),
                new SqlParameter("@CA1", model.CA1),
                new SqlParameter("@CA2", model.CA2),
                new SqlParameter("@CA3", model.CA3),
                new SqlParameter("@CA4", model.CA4));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteContinuousAssessment", new SqlParameter("@ContinuousAssessmentID", id)) > 0;
        }

        private ContinuousAssessment Map(SqlDataReader r)
        {
            return new ContinuousAssessment
            {
                ContinuousAssessmentID = GetValue<int>(r, "ContinuousAssessmentID"),
                StudentID = GetValue<int>(r, "StudentID"),
                SubjectID = GetValue<int>(r, "SubjectID"),
                ClassID = GetValue<int>(r, "ClassID"),
                AcademicYearID = GetValue<int>(r, "AcademicYearID"),
                CA1 = GetValue<decimal>(r, "CA1"),
                CA2 = GetValue<decimal>(r, "CA2"),
                CA3 = GetValue<decimal>(r, "CA3"),
                CA4 = GetValue<decimal>(r, "CA4"),
                TotalCA = GetValue<decimal>(r, "TotalCA"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private ContinuousAssessment MapWithContext(SqlDataReader r)
        {
            var ca = Map(r);
            ca.StudentName = GetValue<string>(r, "StudentName");
            ca.SubjectName = GetValue<string>(r, "SubjectName");
            return ca;
        }
    }
}