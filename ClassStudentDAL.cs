// ============================================================
// DAL/ClassStudentDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ClassStudentDAL : BaseDAL
    {
        public ClassStudent GetClassStudentById(int classStudentId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetClassStudentById", new SqlParameter("@ClassStudentID", classStudentId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public List<ClassStudent> GetByClass(int classId)
        {
            var list = new List<ClassStudent>();
            using (SqlDataReader r = ExecuteReader("sp_GetClassStudentsByClass", new SqlParameter("@ClassID", classId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithUser(r));
                }
            }
            return list;
        }

        public List<ClassStudent> GetByStudent(int studentId)
        {
            var list = new List<ClassStudent>();
            using (SqlDataReader r = ExecuteReader("sp_GetClassStudentsByStudent", new SqlParameter("@StudentID", studentId)))
            {
                while (r.Read())
                {
                    list.Add(Map(r));
                }
            }
            return list;
        }

        public int Create(ClassStudent model)
        {
            SqlParameter outId = new SqlParameter("@NewClassStudentID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateClassStudent",
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@StudentID", model.StudentID),
                new SqlParameter("@IsActive", model.IsActive),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(ClassStudent model)
        {
            int rows = ExecuteNonQuery("sp_UpdateClassStudent",
                new SqlParameter("@ClassStudentID", model.ClassStudentID),
                new SqlParameter("@IsActive", model.IsActive));
            return rows > 0;
        }

        public bool SoftDelete(int classStudentId)
        {
            return ExecuteNonQuery("sp_SoftDeleteClassStudent", new SqlParameter("@ClassStudentID", classStudentId)) > 0;
        }

        private ClassStudent Map(SqlDataReader r)
        {
            return new ClassStudent
            {
                ClassStudentID = GetValue<int>(r, "ClassStudentID"),
                ClassID = GetValue<int>(r, "ClassID"),
                StudentID = GetValue<int>(r, "StudentID"),
                EnrollmentDate = GetValue<DateTime>(r, "EnrollmentDate"),
                IsActive = GetValue<bool>(r, "IsActive"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private ClassStudent MapWithUser(SqlDataReader r)
        {
            var cs = Map(r);
            cs.FullName = GetValue<string>(r, "FullName");
            cs.Email = GetValue<string>(r, "Email");
            cs.PhoneNumber = GetValue<string>(r, "PhoneNumber");
            return cs;
        }
    }
}