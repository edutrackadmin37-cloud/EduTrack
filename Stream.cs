using System;

namespace EduTrack.Models
{
    public class Stream
    {
        public int StreamID { get; set; }
        public string StreamName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}