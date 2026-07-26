using System;

namespace EduTrack.Models
{
    public class RubricCriterionLevel
    {
        public int CriterionLevelID { get; set; }
        public int CriterionID { get; set; }
        public string LevelName { get; set; }
        public decimal ScoreValue { get; set; }
        public string CriteriaDescription { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}