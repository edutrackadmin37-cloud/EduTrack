using System;

namespace EduTrack.Models
{
    public class RubricCriterion
    {
        public int CriterionID { get; set; }
        public int RubricID { get; set; }
        public string CriterionName { get; set; }
        public string Description { get; set; }
        public decimal Weight { get; set; }
        public int DisplayOrder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}