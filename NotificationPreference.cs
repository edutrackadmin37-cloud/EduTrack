// ============================================================
// Models/NotificationPreference.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class NotificationPreference
    {
        public int PreferenceID { get; set; }
        public int UserID { get; set; }
        public bool EmailNotifications { get; set; }
        public bool SmsNotifications { get; set; }
        public bool InAppNotifications { get; set; }
        public bool ProjectUpdates { get; set; }
        public bool GradeAlerts { get; set; }
        public bool AttendanceAlerts { get; set; }
        public bool MessageAlerts { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}