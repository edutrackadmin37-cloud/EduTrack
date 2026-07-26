using System;

namespace EduTrack.Models
{
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedDate { get; set; }
        public int? TargetClassID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string PostedByName { get; set; }
        public string TargetClassName { get; set; }
    }
}