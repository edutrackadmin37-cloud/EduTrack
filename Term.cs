// ============================================================
// Models/Term.cs
// ============================================================
using System;

namespace EduTrack.Models
{
    public class Term
    {
        public int TermID { get; set; }
        public int SemesterID { get; set; }
        public string TermName { get; set; } // e.g., "Term 1", "Term 2"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}