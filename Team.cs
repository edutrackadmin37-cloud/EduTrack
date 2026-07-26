using System;

namespace EduTrack.Models
{
    public class Team
    {
        public int TeamID { get; set; }
        public int ProjectID { get; set; }
        public string TeamName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public int MemberCount { get; set; }
    }
}