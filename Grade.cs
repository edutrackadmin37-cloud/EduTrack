using System;

namespace EduTrack.Models
{
    public class Grade
    {
        public int GradeID { get; set; }
        public int SubmissionID { get; set; }
        public int StudentID { get; set; }
        public decimal? GradeValue { get; set; }
        public string Remarks { get; set; }
        public DateTime DateGraded { get; set; }
        public int GradedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}