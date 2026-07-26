using System;

namespace EduTrack.Models
{
    public class Reflection
    {
        public int ReflectionID { get; set; }
        public int StudentID { get; set; }
        public int ProjectID { get; set; }
        public int WeekNumber { get; set; }
        public string Content { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string FullName { get; set; }
    }
}