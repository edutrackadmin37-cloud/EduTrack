// ============================================================
// Models/DiscussionBoard.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class DiscussionBoard
    {
        public int DiscussionID { get; set; }
        public int SubjectID { get; set; }
        public int? ClassID { get; set; }
        public int? ProjectID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string SubjectName { get; set; }
        public string PostedByName { get; set; }
        public string ClassName { get; set; }
        public string ProjectTitle { get; set; }
    }
}