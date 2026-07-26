// ============================================================
// Models/Timetable.cs
// ============================================================
using System;
using System.Collections.Generic;

namespace EduTrack.Models
{
    public class Timetable
    {
        public int TimetableID { get; set; }
        public int ClassID { get; set; }
        public int SemesterID { get; set; }
        public string DayOfWeek { get; set; } // Monday, Tuesday...
        public string StartTime { get; set; } // HH:mm
        public string EndTime { get; set; }
        public int SubjectID { get; set; }
        public int TeacherID { get; set; }
        public string Room { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
    }
}