// ============================================================
// Models/ProjectLifecycle.cs
// ============================================================
using System;
using System.Collections.Generic;

namespace EduTrack.Models
{
    public class ProjectLifecycle
    {
        public int ProjectID { get; set; }
        public string Status { get; set; } // Current status
        public DateTime StatusChangedAt { get; set; }
        public int ChangedBy { get; set; }
        public string Comments { get; set; }
        public List<ProjectStatusHistory> History { get; set; } = new List<ProjectStatusHistory>();
        public int DaysInCurrentStatus { get; set; }
        public bool IsOverdue { get; set; }
        public DateTime? ExpectedCompletionDate { get; set; }
    }
}