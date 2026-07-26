using System;

namespace EduTrack.Models
{
    public class Test
    {
        public int TestID { get; set; }
        public int ClassSubjectTeacherID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int? TotalMarks { get; set; }
        public int? Duration { get; set; }
        public int? PassingMarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public int? ClassID { get; set; }
        public int? SubjectID { get; set; }
        public int? TeacherID { get; set; }
    }
}