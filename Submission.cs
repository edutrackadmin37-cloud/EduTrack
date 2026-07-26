using System;

namespace EduTrack.Models
{
    public class Submission
    {
        public int SubmissionID { get; set; }
        public int AssignmentID { get; set; }
        public int StudentID { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
        public decimal? RubricTotalScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
    }
}