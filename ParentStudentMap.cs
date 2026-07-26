using System;

namespace EduTrack.Models
{
    public class ParentStudentMap
    {
        public int MapID { get; set; }
        public int ParentID { get; set; }
        public int StudentID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string ParentName { get; set; }
        public string StudentName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}