using System;

namespace EduTrack.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovalStatus { get; set; }
        public bool IsActive { get; set; }
        public string ProfilePicture { get; set; }
        public string Bio { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string NationalID { get; set; }
        public string EmergencyContact { get; set; }
        public string ResetTokenHash { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public string ResetTemporaryPassword { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}