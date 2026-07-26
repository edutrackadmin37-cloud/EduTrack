using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class TestResultDAL : BaseDAL
    {
        public List<TestResult> GetByTest(int testId)
        {
            List<TestResult> list = new List<TestResult>();
            using (SqlDataReader r = ExecuteReader("sp_GetTestResultsByTest", new SqlParameter("@TestID", testId)))
            {
                while (r.Read())
                {
                    list.Add(new TestResult
                    {
                        ResultID = GetValue<int>(r, "ResultID"),
                        TestID = GetValue<int>(r, "TestID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        TotalMarks = GetValue<decimal?>(r, "TotalMarks"),
                        Percentage = GetValue<decimal?>(r, "Percentage"),
                        ResultGrade = GetValue<string>(r, "ResultGrade"),
                        DateRecorded = GetValue<DateTime>(r, "DateRecorded"),
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

        public TestResult GetByTestAndStudent(int testId, int studentId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetTestResultByTestAndStudent",
                new SqlParameter("@TestID", testId),
                new SqlParameter("@StudentID", studentId)))
            {
                if (!r.Read()) return null;
                return new TestResult
                {
                    ResultID = GetValue<int>(r, "ResultID"),
                    TestID = GetValue<int>(r, "TestID"),
                    StudentID = GetValue<int>(r, "StudentID"),
                    TotalMarks = GetValue<decimal?>(r, "TotalMarks"),
                    Percentage = GetValue<decimal?>(r, "Percentage"),
                    ResultGrade = GetValue<string>(r, "ResultGrade"),
                    DateRecorded = GetValue<DateTime>(r, "DateRecorded"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(TestResult model)
        {
            SqlParameter outId = new SqlParameter("@NewResultID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateTestResult",
                new SqlParameter("@TestID", model.TestID),
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@TotalMarks", (object)model.TotalMarks ?? DBNull.Value),
                new SqlParameter("@Percentage", (object)model.Percentage ?? DBNull.Value),
                new SqlParameter("@ResultGrade", (object)model.ResultGrade ?? DBNull.Value),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(TestResult model)
        {
            int rows = ExecuteNonQuery("sp_UpdateTestResult",
                new SqlParameter("@ResultID", model.ResultID),
                new SqlParameter("@TotalMarks", (object)model.TotalMarks ?? DBNull.Value),
                new SqlParameter("@Percentage", (object)model.Percentage ?? DBNull.Value),
                new SqlParameter("@ResultGrade", (object)model.ResultGrade ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int resultId) => ExecuteNonQuery("sp_SoftDeleteTestResult", new SqlParameter("@ResultID", resultId)) > 0;
    }
}