using System;

namespace EduTrack.Models
{
    public class EngagementChecklist
    {
        public int ChecklistID { get; set; }
        public int ClassStudentID { get; set; }
        public int ProjectID { get; set; }
        public int WeekNumber { get; set; }
        public bool Participation { get; set; }
        public bool Questioning { get; set; }
        public bool ProblemSolving { get; set; }
        public bool Collaboration { get; set; }
        public bool TaskCompletion { get; set; }
        public bool Motivation { get; set; }
        public int MarkedBy { get; set; }
        public DateTime MarkedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}