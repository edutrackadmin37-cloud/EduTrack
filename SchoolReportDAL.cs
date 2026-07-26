// ============================================================
// DAL/SchoolReportDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class SchoolReportDAL : BaseDAL
    {
        public SchoolReport GetReport(int schoolId, int academicYearId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetSchoolReport",
                new SqlParameter("@SchoolID", schoolId),
                new SqlParameter("@AcademicYearID", academicYearId)))
            {
                if (!r.Read()) return null;

                var report = new SchoolReport
                {
                    SchoolID = GetValue<int>(r, "SchoolID"),
                    SchoolName = GetValue<string>(r, "SchoolName"),
                    AcademicYearID = GetValue<int>(r, "AcademicYearID"),
                    TotalStudents = GetValue<int>(r, "TotalStudents"),
                    TotalTeachers = GetValue<int>(r, "TotalTeachers"),
                    TotalClasses = GetValue<int>(r, "TotalClasses"),
                    TotalProjects = GetValue<int>(r, "TotalProjects"),
                    OverallAverageGrade = GetValue<decimal>(r, "OverallAverageGrade"),
                    OverallAttendanceRate = GetValue<decimal>(r, "OverallAttendanceRate"),
                    OverallPassRate = GetValue<decimal>(r, "OverallPassRate"),
                    GeneratedDate = GetValue<DateTime>(r, "GeneratedDate")
                };

                // Get programme summaries
                using (SqlDataReader r2 = ExecuteReader("sp_GetSchoolReportProgrammeSummary",
                    new SqlParameter("@SchoolID", schoolId),
                    new SqlParameter("@AcademicYearID", academicYearId)))
                {
                    while (r2.Read())
                    {
                        report.Programmes.Add(new ProgrammeSummary
                        {
                            ProgrammeID = GetValue<int>(r2, "ProgrammeID"),
                            ProgrammeName = GetValue<string>(r2, "ProgrammeName"),
                            StudentCount = GetValue<int>(r2, "StudentCount"),
                            AverageGrade = GetValue<decimal>(r2, "AverageGrade"),
                            PassRate = GetValue<decimal>(r2, "PassRate")
                        });
                    }
                }

                return report;
            }
        }
    }
}