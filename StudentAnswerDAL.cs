using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class StudentAnswerDAL : BaseDAL
    {
        public List<StudentAnswer> GetByTestAndStudent(int testId, int studentId)
        {
            List<StudentAnswer> list = new List<StudentAnswer>();
            using (SqlDataReader r = ExecuteReader("sp_GetStudentAnswersByTestAndStudent",
                new SqlParameter("@TestID", testId),
                new SqlParameter("@StudentID", studentId)))
            {
                while (r.Read())
                {
                    list.Add(new StudentAnswer
                    {
                        AnswerID = GetValue<int>(r, "AnswerID"),
                        TestID = GetValue<int>(r, "TestID"),
                        QuestionID = GetValue<int>(r, "QuestionID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        AnswerText = GetValue<string>(r, "AnswerText"),
                        MarksObtained = GetValue<decimal>(r, "MarksObtained"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        QuestionText = GetValue<string>(r, "QuestionText"),
                        QuestionType = GetValue<string>(r, "QuestionType"),
                        Marks = GetValue<int?>(r, "Marks"),
                        CorrectAnswer = GetValue<string>(r, "CorrectAnswer")
                    });
                }
            }
            return list;
        }

        public StudentAnswer GetById(int answerId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetStudentAnswerById", new SqlParameter("@AnswerID", answerId)))
            {
                if (!r.Read()) return null;
                return new StudentAnswer
                {
                    AnswerID = GetValue<int>(r, "AnswerID"),
                    TestID = GetValue<int>(r, "TestID"),
                    QuestionID = GetValue<int>(r, "QuestionID"),
                    StudentID = GetValue<int>(r, "StudentID"),
                    AnswerText = GetValue<string>(r, "AnswerText"),
                    MarksObtained = GetValue<decimal>(r, "MarksObtained"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(StudentAnswer model)
        {
            SqlParameter outId = new SqlParameter("@NewAnswerID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateStudentAnswer",
                new SqlParameter("@TestID", model.TestID),
                new SqlParameter("@QuestionID", model.QuestionID),
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@AnswerText", (object)model.AnswerText ?? DBNull.Value),
                new SqlParameter("@MarksObtained", model.MarksObtained),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(StudentAnswer model)
        {
            int rows = ExecuteNonQuery("sp_UpdateStudentAnswer",
                new SqlParameter("@AnswerID", model.AnswerID),
                new SqlParameter("@AnswerText", (object)model.AnswerText ?? DBNull.Value),
                new SqlParameter("@MarksObtained", model.MarksObtained)
            );
            return rows > 0;
        }

        public bool SoftDelete(int answerId) => ExecuteNonQuery("sp_SoftDeleteStudentAnswer", new SqlParameter("@AnswerID", answerId)) > 0;
    }
}