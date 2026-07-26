using System;

namespace EduTrack.Models
{
    public class ProjectTeamMember
    {
        public int TeamMemberID { get; set; }
        public int TeamID { get; set; }
        public int StudentID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
    }
}