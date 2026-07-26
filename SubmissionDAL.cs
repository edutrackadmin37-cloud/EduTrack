using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SubmissionDAL : BaseDAL
    {
        public List<Submission> GetByAssignment(int assignmentId)
        {
            List<Submission> list = new List<Submission>();
            using (SqlDataReader r = ExecuteReader("sp_GetSubmissionsByAssignment", new SqlParameter("@AssignmentID", assignmentId)))
            {
                while (r.Read())
                {
                    list.Add(new Submission
                    {
                        SubmissionID = GetValue<int>(r, "SubmissionID"),
                        AssignmentID = GetValue<int>(r, "AssignmentID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        SubmissionDate = GetValue<DateTime>(r, "SubmissionDate"),
                        FilePath = GetValue<string>(r, "FilePath"),
                        Remarks = GetValue<string>(r, "Remarks"),
                        RubricTotalScore = GetValue<decimal?>(r, "RubricTotalScore"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        FullName = GetValue<string>(r, "FullName"),
                        Email = GetValue<string>(r, "Email")
                    });
                }
            }
            return list;
        }

        public Submission GetById(int submissionId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSubmissionById", new SqlParameter("@SubmissionID", submissionId)))
            {
                if (!r.Read()) return null;
                return new Submission
                {
                    SubmissionID = GetValue<int>(r, "SubmissionID"),
                    AssignmentID = GetValue<int>(r, "AssignmentID"),
                    StudentID = GetValue<int>(r, "StudentID"),
                    SubmissionDate = GetValue<DateTime>(r, "SubmissionDate"),
                    FilePath = GetValue<string>(r, "FilePath"),
                    Remarks = GetValue<string>(r, "Remarks"),
                    RubricTotalScore = GetValue<decimal?>(r, "RubricTotalScore"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public Submission GetByAssignmentAndStudent(int assignmentId, int studentId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSubmissionByAssignmentAndStudent",
                new SqlParameter("@AssignmentID", assignmentId),
                new SqlParameter("@StudentID", studentId)))
            {
                if (!r.Read()) return null;
                return new Submission
                {
                    SubmissionID = GetValue<int>(r, "SubmissionID"),
                    AssignmentID = GetValue<int>(r, "AssignmentID"),
                    StudentID = GetValue<int>(r, "StudentID"),
                    SubmissionDate = GetValue<DateTime>(r, "SubmissionDate"),
                    FilePath = GetValue<string>(r, "FilePath"),
                    Remarks = GetValue<string>(r, "Remarks"),
                    RubricTotalScore = GetValue<decimal?>(r, "RubricTotalScore"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(Submission model)
        {
            SqlParameter outId = new SqlParameter("@NewSubmissionID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateSubmission",
                new SqlParameter("@AssignmentID", model.AssignmentID),
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@FilePath", (object)model.FilePath ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)model.Remarks ?? DBNull.Value),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Submission model)
        {
            int rows = ExecuteNonQuery("sp_UpdateSubmission",
                new SqlParameter("@SubmissionID", model.SubmissionID),
                new SqlParameter("@FilePath", (object)model.FilePath ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)model.Remarks ?? DBNull.Value),
                new SqlParameter("@RubricTotalScore", (object)model.RubricTotalScore ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int submissionId)
        {
            return ExecuteNonQuery("sp_SoftDeleteSubmission", new SqlParameter("@SubmissionID", submissionId)) > 0;
        }
    }
}