// ============================================================
// DAL/AttendanceDAL.cs – FINAL
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class AttendanceDAL : BaseDAL
    {
        public List<Attendance> GetAttendanceByClassStudent(int classStudentId)
        {
            var list = new List<Attendance>();
            using (SqlDataReader r = ExecuteReader("sp_GetAttendanceByClassStudent", new SqlParameter("@ClassStudentID", classStudentId)))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Attendance GetAttendanceById(int attendanceId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetAttendanceById", new SqlParameter("@AttendanceID", attendanceId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public List<Attendance> GetAttendanceByClassAndDate(int classId, DateTime attendanceDate)
        {
            var list = new List<Attendance>();
            using (SqlDataReader r = ExecuteReader("sp_GetAttendanceByClassAndDate",
                new SqlParameter("@ClassID", classId),
                new SqlParameter("@AttendanceDate", attendanceDate.Date)))
            {
                while (r.Read())
                {
                    var a = Map(r);
                    a.StudentID = GetValue<int?>(r, "StudentID");
                    a.FullName = GetValue<string>(r, "FullName");
                    list.Add(a);
                }
            }
            return list;
        }

        public int CreateAttendance(Attendance model)
        {
            SqlParameter outId = new SqlParameter("@NewAttendanceID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateAttendance",
                new SqlParameter("@ClassStudentID", model.ClassStudentID),
                new SqlParameter("@AttendanceDate", model.AttendanceDate.Date),
                new SqlParameter("@Status", model.Status),
                new SqlParameter("@MarkedBy", model.MarkedBy),
                new SqlParameter("@Remarks", (object)model.Remarks ?? DBNull.Value),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool UpdateAttendance(Attendance model)
        {
            int rows = ExecuteNonQuery("sp_UpdateAttendance",
                new SqlParameter("@AttendanceID", model.AttendanceID),
                new SqlParameter("@Status", (object)model.Status ?? DBNull.Value),
                new SqlParameter("@Remarks", (object)model.Remarks ?? DBNull.Value));
            return rows > 0;
        }

        public bool SoftDeleteAttendance(int attendanceId)
        {
            return ExecuteNonQuery("sp_SoftDeleteAttendance", new SqlParameter("@AttendanceID", attendanceId)) > 0;
        }

        private Attendance Map(SqlDataReader r)
        {
            return new Attendance
            {
                AttendanceID = GetValue<int>(r, "AttendanceID"),
                ClassStudentID = GetValue<int>(r, "ClassStudentID"),
                AttendanceDate = GetValue<DateTime>(r, "AttendanceDate"),
                Status = GetValue<string>(r, "Status"),
                MarkedBy = GetValue<int>(r, "MarkedBy"),
                Remarks = GetValue<string>(r, "Remarks"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}