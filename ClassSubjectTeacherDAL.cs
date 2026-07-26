using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ClassSubjectTeacherDAL : BaseDAL
    {
        public List<ClassSubjectTeacher> GetAll()
        {
            List<ClassSubjectTeacher> list = new List<ClassSubjectTeacher>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllClassSubjectTeachers"))
            {
                while (r.Read())
                {
                    list.Add(new ClassSubjectTeacher
                    {
                        ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                        ClassID = GetValue<int>(r, "ClassID"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        TeacherID = GetValue<int>(r, "TeacherID"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted")
                    });
                }
            }
            return list;
        }

        public ClassSubjectTeacher GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetClassSubjectTeacherById", new SqlParameter("@ClassSubjectTeacherID", id)))
            {
                if (!r.Read()) return null;
                return new ClassSubjectTeacher
                {
                    ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                    ClassID = GetValue<int>(r, "ClassID"),
                    SubjectID = GetValue<int>(r, "SubjectID"),
                    TeacherID = GetValue<int>(r, "TeacherID"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public List<ClassSubjectTeacher> GetByClass(int classId)
        {
            List<ClassSubjectTeacher> list = new List<ClassSubjectTeacher>();
            using (SqlDataReader r = ExecuteReader("sp_GetClassSubjectTeachersByClass", new SqlParameter("@ClassID", classId)))
            {
                while (r.Read())
                {
                    list.Add(new ClassSubjectTeacher
                    {
                        ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                        ClassID = GetValue<int>(r, "ClassID"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        TeacherID = GetValue<int>(r, "TeacherID"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        TeacherName = GetValue<string>(r, "TeacherName")
                    });
                }
            }
            return list;
        }

        public ClassSubjectTeacher GetByClassAndSubject(int classId, int subjectId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetClassSubjectTeacherByClassAndSubject",
                new SqlParameter("@ClassID", classId),
                new SqlParameter("@SubjectID", subjectId)))
            {
                if (!r.Read()) return null;
                return new ClassSubjectTeacher
                {
                    ClassSubjectTeacherID = GetValue<int>(r, "ClassSubjectTeacherID"),
                    ClassID = GetValue<int>(r, "ClassID"),
                    SubjectID = GetValue<int>(r, "SubjectID"),
                    TeacherID = GetValue<int>(r, "TeacherID"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(ClassSubjectTeacher model)
        {
            SqlParameter outId = new SqlParameter("@NewClassSubjectTeacherID", SqlDbType.Int) { Direction = ParameterDirection.Output };

            ExecuteNonQuery("sp_CreateClassSubjectTeacher",
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@SubjectID", model.SubjectID),
                new SqlParameter("@TeacherID", model.TeacherID),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(ClassSubjectTeacher model)
        {
            int rows = ExecuteNonQuery("sp_UpdateClassSubjectTeacher",
                new SqlParameter("@ClassSubjectTeacherID", model.ClassSubjectTeacherID),
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@SubjectID", model.SubjectID),
                new SqlParameter("@TeacherID", model.TeacherID)
            );
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteClassSubjectTeacher", new SqlParameter("@ClassSubjectTeacherID", id)) > 0;
        }
    }
}