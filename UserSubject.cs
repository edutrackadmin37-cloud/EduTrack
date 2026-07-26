using System;

namespace EduTrack.Models
{
    public class UserSubject
    {
        public int UserSubjectID { get; set; }
        public int UserID { get; set; }
        public int SubjectID { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public DateTime RequestedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
    }
}