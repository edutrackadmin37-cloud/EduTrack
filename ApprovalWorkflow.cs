// ============================================================
// Models/ApprovalWorkflow.cs
// ============================================================
using System;
using System.Collections.Generic;

namespace EduTrack.Models
{
    public class ApprovalWorkflow
    {
        public int WorkflowID { get; set; }
        public string WorkflowName { get; set; } // e.g., "Project Proposal Approval"
        public string EntityType { get; set; } // e.g., "Project", "LeaveRequest"
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public List<ApprovalStep> Steps { get; set; } = new List<ApprovalStep>();
    }
}