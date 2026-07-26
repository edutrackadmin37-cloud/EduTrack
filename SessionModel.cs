using System;

namespace EduTrack.Models
{
    public class SessionModel
    {
        public int SessionID { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Room { get; set; }
        public string Status { get; set; }
        public string StatusClass { get; set; }
        public string BadgeClass { get; set; }
        public string CanJoin { get; set; }
        public string JoinText { get; set; } = string.Empty;
    }
}