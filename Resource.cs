using System;

namespace EduTrack.Models
{
    public class Resource
    {
        public int ResourceID { get; set; }
        public string ResourceTitle { get; set; }
        public string ResourceType { get; set; }
        public string ResourcePath { get; set; }
        public int UploadedBy { get; set; }
        public DateTime UploadDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string UploadedByName { get; set; }
    }
}