// ============================================================
// Models/School.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class School
    {
        public int SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string LogoPath { get; set; }
        public int? HeadmasterID { get; set; } // References UserID
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}