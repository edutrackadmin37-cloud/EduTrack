using System;

namespace EduTrack.Models
{
    public class ProgrammeSubject
    {
        public int ProgrammeSubjectID { get; set; }
        public int ProgrammeID { get; set; }
        public int SubjectID { get; set; }
        public bool IsElective { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public bool? SubjectIsCore { get; set; }
    }
}