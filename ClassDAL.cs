// ============================================================
// DAL/ClassDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ClassDAL : BaseDAL
    {
        public List<Class> GetAll()
        {
            List<Class> list = new List<Class>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllClasses"))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public Class GetById(int classId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetClassById", new SqlParameter("@ClassID", classId)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public int Create(Class model)
        {
            SqlParameter outId = new SqlParameter("@NewClassID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            ExecuteNonQuery("sp_CreateClass",
                new SqlParameter("@ClassName", model.ClassName),
                new SqlParameter("@AcademicYearID", model.AcademicYearID),
                new SqlParameter("@ProgrammeID", model.ProgrammeID),
                new SqlParameter("@StreamID", model.StreamID),
                new SqlParameter("@ClassTeacherID", (object)model.ClassTeacherID ?? DBNull.Value),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Class model)
        {
            int rows = ExecuteNonQuery("sp_UpdateClass",
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@ClassName", model.ClassName),
                new SqlParameter("@AcademicYearID", model.AcademicYearID),
                new SqlParameter("@ProgrammeID", model.ProgrammeID),
                new SqlParameter("@StreamID", model.StreamID),
                new SqlParameter("@ClassTeacherID", (object)model.ClassTeacherID ?? DBNull.Value)
            );
            return rows > 0;
        }

        public ClassStudent GetClassStudentById(int classStudentId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetClassStudentById", new SqlParameter("@ClassStudentID", classStudentId)))
            {
                if (!r.Read()) return null;

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
        }

        public bool SoftDelete(int classId)
        {
            return ExecuteNonQuery("sp_SoftDeleteClass", new SqlParameter("@ClassID", classId)) > 0;
        }

        private Class Map(SqlDataReader r)
        {
            return new Class
            {
                ClassID = GetValue<int>(r, "ClassID"),
                ClassName = GetValue<string>(r, "ClassName"),
                AcademicYearID = GetValue<int>(r, "AcademicYearID"),
                ProgrammeID = GetValue<int>(r, "ProgrammeID"),
                StreamID = GetValue<int>(r, "StreamID"),
                ClassTeacherID = GetValue<int?>(r, "ClassTeacherID"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private Class MapWithContext(SqlDataReader r)
        {
            Class c = Map(r);
            c.YearName = GetValue<string>(r, "YearName");
            c.ProgrammeName = GetValue<string>(r, "ProgrammeName");
            c.StreamName = GetValue<string>(r, "StreamName");
            c.ClassTeacherName = GetValue<string>(r, "ClassTeacherName");
            return c;
        }
    }
}