using System;

namespace EduTrack.Models
{
    public class Class
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public int AcademicYearID { get; set; }
        public int ProgrammeID { get; set; }
        public int StreamID { get; set; }
        public int? ClassTeacherID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string YearName { get; set; }
        public string ProgrammeName { get; set; }
        public string StreamName { get; set; }
        public string ClassTeacherName { get; set; }
    }
}