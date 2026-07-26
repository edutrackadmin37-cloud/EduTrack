using System;

namespace EduTrack.Models
{
    public class Programme
    {
        public int ProgrammeID { get; set; }
        public string ProgrammeName { get; set; }
        public string Description { get; set; }
        public int DepartmentID { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}