// ============================================================
// DAL/TimetableDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class TimetableDAL : BaseDAL
    {
        public List<Timetable> GetByClass(int classId, int semesterId)
        {
            var list = new List<Timetable>();
            using (SqlDataReader r = ExecuteReader("sp_GetTimetableByClassSemester",
                new SqlParameter("@ClassID", classId),
                new SqlParameter("@SemesterID", semesterId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }
        // Add this inside TimetableDAL class
        public List<Timetable> GetAll()
        {
            var list = new List<Timetable>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllTimetables"))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }
        public List<Timetable> GetByTeacher(int teacherId, int semesterId)
        {
            var list = new List<Timetable>();
            using (SqlDataReader r = ExecuteReader("sp_GetTimetableByTeacher",
                new SqlParameter("@TeacherID", teacherId),
                new SqlParameter("@SemesterID", semesterId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public Timetable GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetTimetableById", new SqlParameter("@TimetableID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public int Create(Timetable model)
        {
            SqlParameter outId = new SqlParameter("@NewTimetableID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateTimetable",
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@SemesterID", model.SemesterID),
                new SqlParameter("@DayOfWeek", model.DayOfWeek),
                new SqlParameter("@StartTime", model.StartTime),
                new SqlParameter("@EndTime", model.EndTime),
                new SqlParameter("@SubjectID", model.SubjectID),
                new SqlParameter("@TeacherID", model.TeacherID),
                new SqlParameter("@Room", (object)model.Room ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Timetable model)
        {
            int rows = ExecuteNonQuery("sp_UpdateTimetable",
                new SqlParameter("@TimetableID", model.TimetableID),
                new SqlParameter("@ClassID", model.ClassID),
                new SqlParameter("@SemesterID", model.SemesterID),
                new SqlParameter("@DayOfWeek", (object)model.DayOfWeek ?? DBNull.Value),
                new SqlParameter("@StartTime", (object)model.StartTime ?? DBNull.Value),
                new SqlParameter("@EndTime", (object)model.EndTime ?? DBNull.Value),
                new SqlParameter("@SubjectID", model.SubjectID),
                new SqlParameter("@TeacherID", model.TeacherID),
                new SqlParameter("@Room", (object)model.Room ?? DBNull.Value));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteTimetable", new SqlParameter("@TimetableID", id)) > 0;
        }

        private Timetable Map(SqlDataReader r)
        {
            return new Timetable
            {
                TimetableID = GetValue<int>(r, "TimetableID"),
                ClassID = GetValue<int>(r, "ClassID"),
                SemesterID = GetValue<int>(r, "SemesterID"),
                DayOfWeek = GetValue<string>(r, "DayOfWeek"),
                StartTime = GetValue<string>(r, "StartTime"),
                EndTime = GetValue<string>(r, "EndTime"),
                SubjectID = GetValue<int>(r, "SubjectID"),
                TeacherID = GetValue<int>(r, "TeacherID"),
                Room = GetValue<string>(r, "Room"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private Timetable MapWithContext(SqlDataReader r)
        {
            var t = Map(r);
            t.ClassName = GetValue<string>(r, "ClassName");
            t.SubjectName = GetValue<string>(r, "SubjectName");
            t.TeacherName = GetValue<string>(r, "TeacherName");
            return t;
        }
    }
}