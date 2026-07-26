// ============================================================
// Models/ApprovalStep.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class ApprovalStep
    {
        public int StepID { get; set; }
        public int WorkflowID { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; } // e.g., "HOD Approval", "Headmaster Approval"
        public string RequiredRole { get; set; } // e.g., "HOD", "Headmaster"
        public int? ApproverID { get; set; } // Specific user or null for role-based
        public bool IsParallel { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}