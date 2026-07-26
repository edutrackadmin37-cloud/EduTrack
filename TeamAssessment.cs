using System;

namespace EduTrack.Models
{
    public class TeamAssessment
    {
        public int TeamAssessmentID { get; set; }
        public int TeamID { get; set; }
        public int RubricID { get; set; }
        public decimal TeamScore { get; set; }
        public string Comments { get; set; }
        public int AssessedBy { get; set; }
        public DateTime AssessedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}