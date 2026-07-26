// ============================================================
// BLL/AttendanceBLL.cs - UPDATED
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EduTrack.BLL
{
    public class AttendanceBLL
    {
        private readonly AttendanceDAL _attendanceDAL = new AttendanceDAL();
        private readonly ClassStudentDAL _classStudentDAL = new ClassStudentDAL();

        public Response<List<Attendance>> GetAttendanceByClassStudent(int classStudentId)
        {
            if (classStudentId <= 0)
                return Response<List<Attendance>>.Failure("Invalid ClassStudentID.", "VALIDATION_ERROR");

            List<Attendance> data = _attendanceDAL.GetAttendanceByClassStudent(classStudentId);
            return Response<List<Attendance>>.Success(data);
        }

        public Response<Attendance> GetAttendanceById(int attendanceId)
        {
            if (attendanceId <= 0)
                return Response<Attendance>.Failure("Invalid AttendanceID.", "VALIDATION_ERROR");

            Attendance data = _attendanceDAL.GetAttendanceById(attendanceId);
            if (data == null)
                return Response<Attendance>.Failure("Attendance record not found.", "NOT_FOUND");

            return Response<Attendance>.Success(data);
        }

        public Response<List<Attendance>> GetAttendanceByClassAndDate(int classId, DateTime attendanceDate)
        {
            if (classId <= 0)
                return Response<List<Attendance>>.Failure("Invalid ClassID.", "VALIDATION_ERROR");

            List<Attendance> data = _attendanceDAL.GetAttendanceByClassAndDate(classId, attendanceDate.Date);
            return Response<List<Attendance>>.Success(data);
        }

        public Response<int> CreateAttendance(Attendance attendance)
        {
            if (attendance == null)
                return Response<int>.Failure("Attendance data is required.", "VALIDATION_ERROR");

            // Try to obtain ClassStudentID
            int? classStudentId = TryGetInt(attendance, "ClassStudentID", "ClassStudentId");

            int? classId = null;
            int? studentId = null;
            if (!classStudentId.HasValue)
            {
                // Try to obtain ClassID and StudentID (backwards compatible)
                classId = TryGetInt(attendance, "ClassID", "ClassId");
                studentId = TryGetInt(attendance, "StudentID", "StudentId");
                if (!classId.HasValue || !studentId.HasValue)
                    return Response<int>.Failure("Either ClassStudentID or both ClassID and StudentID are required.", "VALIDATION_ERROR");

                // Find the ClassStudent record using ClassID and StudentID
                ClassStudent clsStudent = null;
                var classStudents = _classStudentDAL.GetByClass(classId.Value);
                foreach (var cs in classStudents)
                {
                    int? csStudentId = TryGetInt(cs, "StudentID", "StudentId");
                    if (csStudentId.HasValue && csStudentId.Value == studentId.Value)
                    {
                        // Optionally check IsDeleted/IsActive if present
                        bool isDeleted = TryGetBool(cs, "IsDeleted") ?? false;
                        bool isActive = TryGetBool(cs, "IsActive") ?? true;

                        if (isDeleted || !isActive)
                            return Response<int>.Failure("Class student record is invalid or inactive.", "VALIDATION_ERROR");

                        clsStudent = cs;
                        break;
                    }
                }

                if (clsStudent == null)
                    return Response<int>.Failure("Class student record not found for provided ClassID and StudentID.", "VALIDATION_ERROR");

                // Set ClassStudentID on attendance if property exists
                var foundClassStudentId = TryGetInt(clsStudent, "ClassStudentID", "ClassStudentId");
                if (foundClassStudentId.HasValue)
                {
                    TrySetInt(attendance, foundClassStudentId.Value, "ClassStudentID", "ClassStudentId");
                    classStudentId = foundClassStudentId;
                }
                else
                {
                    // If ClassStudentID not present on cs, try known ID names
                    int? anyId = TryGetInt(clsStudent, "ID", "Id");
                    if (anyId.HasValue)
                    {
                        TrySetInt(attendance, anyId.Value, "ClassStudentID", "ClassStudentId", "ID", "Id");
                        classStudentId = anyId;
                    }
                }

                if (!classStudentId.HasValue)
                    return Response<int>.Failure("Unable to determine ClassStudentID for attendance.", "VALIDATION_ERROR");
            }

            // Validate Attendance Status
            string status = TryGetString(attendance, "AttendanceStatus", "Status");
            if (string.IsNullOrWhiteSpace(status))
                return Response<int>.Failure("Attendance status is required.", "VALIDATION_ERROR");

            if (!IsValidStatus(status))
                return Response<int>.Failure("Status must be Present, Absent, Late, or Excused.", "VALIDATION_ERROR");

            // Validate MarkedBy (existing approach using reflection)
            if (!attendance.GetType().GetProperty("MarkedBy")?.CanRead ?? true)
                return Response<int>.Failure("Invalid MarkedBy user.", "VALIDATION_ERROR");
            var markedByValue = (int?)attendance.GetType().GetProperty("MarkedBy")?.GetValue(attendance);
            if (!markedByValue.HasValue || markedByValue.Value <= 0)
                return Response<int>.Failure("Invalid MarkedBy user.", "VALIDATION_ERROR");

            // Ensure AttendanceDate is set; set to today if default and property exists
            DateTime? attDate = TryGetDateTime(attendance, "AttendanceDate", "Date");
            if (!attDate.HasValue || attDate.Value == default(DateTime))
            {
                TrySetDate(attendance, DateTime.Today, "AttendanceDate", "Date");
            }

            int newId;
            try
            {
                newId = _attendanceDAL.CreateAttendance(attendance);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("UQ_Attendance_ClassStudent_Date", StringComparison.OrdinalIgnoreCase) >= 0)
                    return Response<int>.Failure("Attendance already marked for this student on this date.", "DUPLICATE");

                return Response<int>.Failure($"Failed to create attendance record: {ex.Message}", "CREATE_FAILED");
            }

            return newId > 0
                ? Response<int>.Success(newId, "Attendance recorded successfully.")
                : Response<int>.Failure("Unable to create attendance record.", "CREATE_FAILED");
        }

         
        public Response<bool> UpdateAttendance(Attendance attendance)
        {
            if (attendance == null)
                return Response<bool>.Failure("Invalid attendance data.", "VALIDATION_ERROR");

            int? attendanceId = TryGetInt(attendance, "AttendanceID", "AttendanceId", "ID", "Id");
            if (!attendanceId.HasValue || attendanceId.Value <= 0)
                return Response<bool>.Failure("Invalid attendance data.", "VALIDATION_ERROR");

            string status = TryGetString(attendance, "AttendanceStatus", "Status");
            if (!string.IsNullOrWhiteSpace(status) && !IsValidStatus(status))
                return Response<bool>.Failure("Status must be Present, Absent, Late, or Excused.", "VALIDATION_ERROR");

            Attendance existing = _attendanceDAL.GetAttendanceById(attendanceId.Value);
            if (existing == null)
                return Response<bool>.Failure("Attendance record not found.", "NOT_FOUND");

            bool ok = _attendanceDAL.UpdateAttendance(attendance);
            return ok
                ? Response<bool>.Success(true, "Attendance updated successfully.")
                : Response<bool>.Failure("Failed to update attendance.", "UPDATE_FAILED");
        }

        public Response<bool> SoftDeleteAttendance(int attendanceId)
        {
            if (attendanceId <= 0)
                return Response<bool>.Failure("Invalid AttendanceID.", "VALIDATION_ERROR");

            Attendance existing = _attendanceDAL.GetAttendanceById(attendanceId);
            if (existing == null)
                return Response<bool>.Failure("Attendance record not found.", "NOT_FOUND");

            bool ok = _attendanceDAL.SoftDeleteAttendance(attendanceId);
            return ok
                ? Response<bool>.Success(true, "Attendance deleted successfully.")
                : Response<bool>.Failure("Failed to delete attendance.", "DELETE_FAILED");
        }

        private bool IsValidStatus(string status)
        {
            return status.Equals("Present", StringComparison.OrdinalIgnoreCase)
                || status.Equals("Absent", StringComparison.OrdinalIgnoreCase)
                || status.Equals("Late", StringComparison.OrdinalIgnoreCase)
                || status.Equals("Excused", StringComparison.OrdinalIgnoreCase);
        }

        #region Reflection helpers

        private int? TryGetInt(object obj, params string[] names)
        {
            if (obj == null) return null;
            foreach (var name in names)
            {
                var prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;
                if (!prop.CanRead) continue;
                var val = prop.GetValue(obj);
                if (val == null) continue;
                if (val is int) return (int)val;
                if (val is long) return Convert.ToInt32((long)val);
                int parsed;
                if (int.TryParse(val.ToString(), out parsed))
                    return parsed;
            }
            return null;
        }

        private string TryGetString(object obj, params string[] names)
        {
            if (obj == null) return null;
            foreach (var name in names)
            {
                var prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;
                if (!prop.CanRead) continue;
                var val = prop.GetValue(obj);
                if (val == null) continue;
                return val.ToString();
            }
            return null;
        }

        private DateTime? TryGetDateTime(object obj, params string[] names)
        {
            if (obj == null) return null;
            foreach (var name in names)
            {
                var prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;
                if (!prop.CanRead) continue;
                var val = prop.GetValue(obj);
                if (val == null) continue;
                if (val is DateTime) return (DateTime)val;
                DateTime dt;
                if (DateTime.TryParse(val.ToString(), out dt))
                    return dt;
            }
            return null;
        }

        private bool? TryGetBool(object obj, params string[] names)
        {
            if (obj == null) return null;
            foreach (var name in names)
            {
                var prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;
                if (!prop.CanRead) continue;
                var val = prop.GetValue(obj);
                if (val == null) continue;
                if (val is bool) return (bool)val;
                bool b;
                if (bool.TryParse(val.ToString(), out b))
                    return b;
            }
            return null;
        }

        private void TrySetInt(object obj, int value, params string[] names)
        {
            if (obj == null) return;
            foreach (var name in names)
            {
                var prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;
                if (!prop.CanWrite) continue;
                var targetType = prop.PropertyType;
                try
                {
                    if (targetType == typeof(int) || targetType == typeof(int?))
                        prop.SetValue(obj, value);
                    else if (targetType == typeof(long) || targetType == typeof(long?))
                        prop.SetValue(obj, Convert.ToInt64(value));
                    else
                        prop.SetValue(obj, Convert.ChangeType(value, targetType));
                    return;
                }
                catch
                {
                    // ignore and try next
                }
            }
        }

        private void TrySetDate(object obj, DateTime value, params string[] names)
        {
            if (obj == null) return;
            foreach (var name in names)
            {
                var prop = obj.GetType().GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (prop == null) continue;
                if (!prop.CanWrite) continue;
                var targetType = prop.PropertyType;
                try
                {
                    if (targetType == typeof(DateTime) || targetType == typeof(DateTime?))
                        prop.SetValue(obj, value);
                    else
                        prop.SetValue(obj, Convert.ChangeType(value, targetType));
                    return;
                }
                catch
                {
                    // ignore and try next
                }
            }
        }

        #endregion
    }
}