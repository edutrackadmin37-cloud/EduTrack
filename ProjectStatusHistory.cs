using System;

namespace EduTrack.Models
{
    public class ProjectStatusHistory
    {
        public int ProjectStatusHistoryID { get; set; }
        public int ProjectID { get; set; }
        public string Status { get; set; }
        public int ChangedBy { get; set; }
        public string Comments { get; set; }
        public DateTime ChangedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}