// ============================================================
// Models/SelfAssessment.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class SelfAssessment
    {
        public int SelfAssessmentID { get; set; }
        public int StudentID { get; set; }
        public int ProjectID { get; set; }
        public int? RubricID { get; set; }
        public int Score { get; set; }
        public string Reflection { get; set; }
        public DateTime AssessedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string StudentName { get; set; }
        public string ProjectTitle { get; set; }
    }
}