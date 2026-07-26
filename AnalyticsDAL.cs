// ============================================================
// DAL/AnalyticsDAL.cs - UPDATED
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class AnalyticsDAL : BaseDAL
    {
        public List<SubjectSiloedPerformance> GetStudentSubjectPerformance(int studentId, int? subjectId = null, int? academicYearId = null)
        {
            var list = new List<SubjectSiloedPerformance>();
            using (SqlDataReader r = ExecuteReader("sp_GetStudentSubjectPerformance",
                new SqlParameter("@StudentID", studentId),
                new SqlParameter("@SubjectID", (object)subjectId ?? DBNull.Value),
                new SqlParameter("@AcademicYearID", (object)academicYearId ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(new SubjectSiloedPerformance
                    {
                        StudentID = GetValue<int>(r, "StudentID"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        AverageGrade = GetValue<decimal>(r, "AverageGrade"),
                        HighestGrade = GetValue<decimal>(r, "HighestGrade"),
                        LowestGrade = GetValue<decimal>(r, "LowestGrade"),
                        AssignmentsCompleted = GetValue<int>(r, "AssignmentsCompleted"),
                        TestsTaken = GetValue<int>(r, "TestsTaken"),
                        AttendanceRate = GetValue<decimal>(r, "AttendanceRate"),
                        EngagementScore = GetValue<decimal>(r, "EngagementScore"),
                        GradeLetter = GetValue<string>(r, "GradeLetter"),
                        Remarks = GetValue<string>(r, "Remarks")
                    });
                }
            }
            return list;
        }

        public List<SubjectSiloedPerformance> GetClassSubjectPerformance(int classId, int subjectId, int? academicYearId = null)
        {
            var list = new List<SubjectSiloedPerformance>();
            using (SqlDataReader r = ExecuteReader("sp_GetClassSubjectPerformance",
                new SqlParameter("@ClassID", classId),
                new SqlParameter("@SubjectID", subjectId),
                new SqlParameter("@AcademicYearID", (object)academicYearId ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(new SubjectSiloedPerformance
                    {
                        StudentID = GetValue<int>(r, "StudentID"),
                        StudentName = GetValue<string>(r, "StudentName"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        AverageGrade = GetValue<decimal>(r, "AverageGrade"),
                        HighestGrade = GetValue<decimal>(r, "HighestGrade"),
                        LowestGrade = GetValue<decimal>(r, "LowestGrade"),
                        AssignmentsCompleted = GetValue<int>(r, "AssignmentsCompleted"),
                        TestsTaken = GetValue<int>(r, "TestsTaken"),
                        AttendanceRate = GetValue<decimal>(r, "AttendanceRate"),
                        EngagementScore = GetValue<decimal>(r, "EngagementScore"),
                        GradeLetter = GetValue<string>(r, "GradeLetter"),
                        Remarks = GetValue<string>(r, "Remarks")
                    });
                }
            }
            return list;
        }

        public List<SubjectSiloedPerformance> GetTeacherSubjectPerformance(int teacherId, int subjectId, int? academicYearId = null)
        {
            var list = new List<SubjectSiloedPerformance>();
            using (SqlDataReader r = ExecuteReader("sp_GetTeacherSubjectPerformance",
                new SqlParameter("@TeacherID", teacherId),
                new SqlParameter("@SubjectID", subjectId),
                new SqlParameter("@AcademicYearID", (object)academicYearId ?? DBNull.Value)))
            {
                while (r.Read())
                {
                    list.Add(new SubjectSiloedPerformance
                    {
                        StudentID = GetValue<int>(r, "StudentID"),
                        StudentName = GetValue<string>(r, "StudentName"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        AverageGrade = GetValue<decimal>(r, "AverageGrade"),
                        HighestGrade = GetValue<decimal>(r, "HighestGrade"),
                        LowestGrade = GetValue<decimal>(r, "LowestGrade"),
                        AssignmentsCompleted = GetValue<int>(r, "AssignmentsCompleted"),
                        TestsTaken = GetValue<int>(r, "TestsTaken"),
                        AttendanceRate = GetValue<decimal>(r, "AttendanceRate"),
                        EngagementScore = GetValue<decimal>(r, "EngagementScore"),
                        GradeLetter = GetValue<string>(r, "GradeLetter"),
                        Remarks = GetValue<string>(r, "Remarks")
                    });
                }
            }
            return list;
        }
    }
}