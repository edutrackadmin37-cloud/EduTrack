using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class TestDAL : BaseDAL
    {
        public List<Test> GetAll()
        {
            List<Test> list = new List<Test>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllTests"))
            {
                while (r.Read())
                {
                    list.Add(new Test
                    {
                        TestID = GetValue<int>(r, "TestID"),
                        ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                        Title = GetValue<string>(r, "Title"),
                        Description = GetValue<string>(r, "Description"),
                        DueDate = GetValue<DateTime?>(r, "DueDate"),
                        TotalMarks = GetValue<int?>(r, "TotalMarks"),
                        Duration = GetValue<int?>(r, "Duration"),
                        PassingMarks = GetValue<int?>(r, "PassingMarks"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        ClassID = GetValue<int?>(r, "ClassID"),
                        SubjectID = GetValue<int?>(r, "SubjectID"),
                        TeacherID = GetValue<int?>(r, "TeacherID")
                    });
                }
            }
            return list;
        }

        public Test GetById(int testId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetTestById", new SqlParameter("@TestID", testId)))
            {
                if (!r.Read()) return null;
                return new Test
                {
                    TestID = GetValue<int>(r, "TestID"),
                    ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                    Title = GetValue<string>(r, "Title"),
                    Description = GetValue<string>(r, "Description"),
                    DueDate = GetValue<DateTime?>(r, "DueDate"),
                    TotalMarks = GetValue<int?>(r, "TotalMarks"),
                    Duration = GetValue<int?>(r, "Duration"),
                    PassingMarks = GetValue<int?>(r, "PassingMarks"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public List<Test> GetByClassSubjectTeacher(int classSubjectTeacherId)
        {
            List<Test> list = new List<Test>();
            using (SqlDataReader r = ExecuteReader("sp_GetTestsByClassSubjectTeacher", new SqlParameter("@ClassSubjectTeacherID", classSubjectTeacherId)))
            {
                while (r.Read())
                {
                    list.Add(new Test
                    {
                        TestID = GetValue<int>(r, "TestID"),
                        ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                        Title = GetValue<string>(r, "Title"),
                        Description = GetValue<string>(r, "Description"),
                        DueDate = GetValue<DateTime?>(r, "DueDate"),
                        TotalMarks = GetValue<int?>(r, "TotalMarks"),
                        Duration = GetValue<int?>(r, "Duration"),
                        PassingMarks = GetValue<int?>(r, "PassingMarks"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }

        public int Create(Test model)
        {
            SqlParameter outId = new SqlParameter("@NewTestID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateTest",
                new SqlParameter("@ClassSubjectTeacherID", model.ClassSubjectTeacherID),
                new SqlParameter("@Title", model.Title),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@DueDate", (object)model.DueDate ?? DBNull.Value),
                new SqlParameter("@TotalMarks", (object)model.TotalMarks ?? DBNull.Value),
                new SqlParameter("@Duration", (object)model.Duration ?? DBNull.Value),
                new SqlParameter("@PassingMarks", (object)model.PassingMarks ?? DBNull.Value),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Test model)
        {
            int rows = ExecuteNonQuery("sp_UpdateTest",
                new SqlParameter("@TestID", model.TestID),
                new SqlParameter("@ClassSubjectTeacherID", model.ClassSubjectTeacherID),
                new SqlParameter("@Title", (object)model.Title ?? DBNull.Value),
                new SqlParameter("@Description", (object)model.Description ?? DBNull.Value),
                new SqlParameter("@DueDate", (object)model.DueDate ?? DBNull.Value),
                new SqlParameter("@TotalMarks", (object)model.TotalMarks ?? DBNull.Value),
                new SqlParameter("@Duration", (object)model.Duration ?? DBNull.Value),
                new SqlParameter("@PassingMarks", (object)model.PassingMarks ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int testId) => ExecuteNonQuery("sp_SoftDeleteTest", new SqlParameter("@TestID", testId)) > 0;
    }
}