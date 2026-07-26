using System;

namespace EduTrack.Models
{
    public class ActivityLog
    {
        public int LogID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; }
        public DateTime ActionDate { get; set; }
        public string IPAddress { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }

        public string FullName { get; set; }
    }
}