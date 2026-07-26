using System;

namespace EduTrack.Models
{
    public class ClassStudent
    {
        public int ClassStudentID { get; set; }
        public int ClassID { get; set; }
        public int StudentID { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}