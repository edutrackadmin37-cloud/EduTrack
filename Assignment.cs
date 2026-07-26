using System;

namespace EduTrack.Models
{
    public class Assignment
    {
        public int AssignmentID { get; set; }
        public int? ProjectID { get; set; }
        public int ClassSubjectTeacherID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? AssignedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int RubricID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public int? ClassID { get; set; }
        public int? SubjectID { get; set; }
        public int? TeacherID { get; set; }
        public string RubricTitle { get; set; }
    }
}