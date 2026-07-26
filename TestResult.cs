using System;

namespace EduTrack.Models
{
    public class TestResult
    {
        public int ResultID { get; set; }
        public int TestID { get; set; }
        public int StudentID { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? Percentage { get; set; }
        public string ResultGrade { get; set; }
        public DateTime DateRecorded { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
    }
}