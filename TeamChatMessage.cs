// ============================================================
// Models/TeamChatMessage.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class TeamChatMessage
    {
        public int MessageID { get; set; }
        public int TeamID { get; set; }
        public int SenderID { get; set; }
        public string MessageText { get; set; }
        public string FilePath { get; set; } // Optional file attachment
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string SenderName { get; set; }
        public string TeamName { get; set; }
    }
}