using System;

namespace EduTrack.Models
{
    public class StudentAnswer
    {
        public int AnswerID { get; set; }
        public int TestID { get; set; }
        public int QuestionID { get; set; }
        public int StudentID { get; set; }
        public string AnswerText { get; set; }
        public decimal MarksObtained { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public string QuestionText { get; set; }
        public string QuestionType { get; set; }
        public int? Marks { get; set; }
        public string CorrectAnswer { get; set; }
    }
}