using System;

namespace EduTrack.Models
{
    public class ClassSubjectTeacher
    {
        public int ClassSubjectTeacherID { get; set; }
        public int ClassID { get; set; }
        public int SubjectID { get; set; }
        public int TeacherID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
    }
}