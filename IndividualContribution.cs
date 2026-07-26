using System;

namespace EduTrack.Models
{
    public class IndividualContribution
    {
        public int IndividualContributionID { get; set; }
        public int TeamAssessmentID { get; set; }
        public int StudentID { get; set; }
        public decimal IndividualScore { get; set; }
        public string Feedback { get; set; }
        public int AssessedBy { get; set; }
        public DateTime AssessedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
    }
}