using System;

namespace EduTrack.Models
{
    public class Project
    {
        public int ProjectID { get; set; }
        public int ClassSubjectTeacherID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Objectives { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int MaxTeamSize { get; set; }
        public bool AllowTeamFormation { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public int? ClassID { get; set; }
        public int? SubjectID { get; set; }
        public int? TeacherID { get; set; }
        public string CreatedByName { get; set; }
    }
}