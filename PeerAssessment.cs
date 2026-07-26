// ============================================================
// Models/PeerAssessment.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class PeerAssessment
    {
        public int PeerAssessmentID { get; set; }
        public int AssessorID { get; set; } // Student ID
        public int AssesseeID { get; set; } // Student ID
        public int ProjectID { get; set; }
        public int? RubricID { get; set; }
        public int Score { get; set; } // 1-5 or percentage
        public string Feedback { get; set; }
        public DateTime AssessedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string AssessorName { get; set; }
        public string AssesseeName { get; set; }
        public string ProjectTitle { get; set; }
    }
}