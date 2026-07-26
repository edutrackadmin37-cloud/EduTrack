using System;

namespace EduTrack.Models
{
    public class Attendance
    {
        public int AttendanceID { get; set; }
        public int ClassStudentID { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string Status { get; set; }
        public int MarkedBy { get; set; }
        public string Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Additional properties for queries that include student info
        public int? StudentID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}