// ============================================================
// Models/Staff.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        public int UserID { get; set; } // References Users table
        public int? SchoolID { get; set; }
        public string StaffNumber { get; set; }
        public string Position { get; set; } // e.g., Teacher, Headmaster, etc.
        public int? DepartmentID { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties (non‑database)
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DepartmentName { get; set; }
        public string SchoolName { get; set; }
    }
}