using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EduTrack.Models;

namespace EduTrack.DAL
{
    public class TeacherPerformanceReport
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int AcademicYearID { get; set; }
        public decimal OverallAverageGrade { get; set; }
        public decimal OverallPassRate { get; set; }
        public int TotalStudents { get; set; }
        public DateTime GeneratedDate { get; set; }
        }
}