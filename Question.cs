using System;

namespace EduTrack.Models
{
    public class Question
    {
        public int QuestionID { get; set; }
        public int TestID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int? Marks { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}