using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class QuestionDAL : BaseDAL
    {
        public List<Question> GetByTest(int testId)
        {
            List<Question> list = new List<Question>();
            using (SqlDataReader r = ExecuteReader("sp_GetQuestionsByTest", new SqlParameter("@TestID", testId)))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Question GetById(int questionId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetQuestionById", new SqlParameter("@QuestionID", questionId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Question model)
        {
            SqlParameter outId = new SqlParameter("@NewQuestionID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateQuestion",
                new SqlParameter("@TestID", model.TestID),
                new SqlParameter("@QuestionText", model.QuestionText),
                new SqlParameter("@QuestionType", model.QuestionType),
                new SqlParameter("@Marks", (object)model.Marks ?? DBNull.Value),
                new SqlParameter("@OptionA", (object)model.OptionA ?? DBNull.Value),
                new SqlParameter("@OptionB", (object)model.OptionB ?? DBNull.Value),
                new SqlParameter("@OptionC", (object)model.OptionC ?? DBNull.Value),
                new SqlParameter("@OptionD", (object)model.OptionD ?? DBNull.Value),
                new SqlParameter("@CorrectAnswer", (object)model.CorrectAnswer ?? DBNull.Value),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Question model)
        {
            int rows = ExecuteNonQuery("sp_UpdateQuestion",
                new SqlParameter("@QuestionID", model.QuestionID),
                new SqlParameter("@QuestionText", (object)model.QuestionText ?? DBNull.Value),
                new SqlParameter("@QuestionType", (object)model.QuestionType ?? DBNull.Value),
                new SqlParameter("@Marks", (object)model.Marks ?? DBNull.Value),
                new SqlParameter("@OptionA", (object)model.OptionA ?? DBNull.Value),
                new SqlParameter("@OptionB", (object)model.OptionB ?? DBNull.Value),
                new SqlParameter("@OptionC", (object)model.OptionC ?? DBNull.Value),
                new SqlParameter("@OptionD", (object)model.OptionD ?? DBNull.Value),
                new SqlParameter("@CorrectAnswer", (object)model.CorrectAnswer ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int questionId) => ExecuteNonQuery("sp_SoftDeleteQuestion", new SqlParameter("@QuestionID", questionId)) > 0;

        private Question Map(SqlDataReader r)
        {
            return new Question
            {
                QuestionID = GetValue<int>(r, "QuestionID"),
                TestID = GetValue<int>(r, "TestID"),
                QuestionText = GetValue<string>(r, "QuestionText"),
                QuestionType = GetValue<string>(r, "QuestionType"),
                Marks = GetValue<int?>(r, "Marks"),
                OptionA = GetValue<string>(r, "OptionA"),
                OptionB = GetValue<string>(r, "OptionB"),
                OptionC = GetValue<string>(r, "OptionC"),
                OptionD = GetValue<string>(r, "OptionD"),
                CorrectAnswer = GetValue<string>(r, "CorrectAnswer"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}