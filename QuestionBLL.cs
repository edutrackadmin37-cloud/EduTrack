using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class QuestionBLL
    {
        private readonly QuestionDAL _dal = new QuestionDAL();

        public Response<List<Question>> GetQuestionsByTest(int testId)
        {
            if (testId <= 0)
                return Response<List<Question>>.Failure("Invalid test ID.", "VALIDATION_ERROR");

            try
            {
                var questions = _dal.GetByTest(testId);
                return Response<List<Question>>.Success(questions);
            }
            catch (Exception ex)
            {
                return Response<List<Question>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<Question> GetQuestionById(int questionId)
        {
            if (questionId <= 0)
                return Response<Question>.Failure("Invalid question ID.", "VALIDATION_ERROR");

            try
            {
                var question = _dal.GetById(questionId);
                return question == null
                    ? Response<Question>.Failure("Question not found.", "NOT_FOUND")
                    : Response<Question>.Success(question);
            }
            catch (Exception ex)
            {
                return Response<Question>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> CreateQuestion(Question question)
        {
            if (question == null || question.TestID <= 0 || string.IsNullOrWhiteSpace(question.QuestionText))
                return Response<int>.Failure("Invalid question data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(question);
                return id > 0 ? Response<int>.Success(id, "Question created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> UpdateQuestion(Question question)
        {
            if (question == null || question.QuestionID <= 0)
                return Response<bool>.Failure("Invalid question data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(question);
                return ok ? Response<bool>.Success(true, "Question updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDeleteQuestion(int questionId)
        {
            if (questionId <= 0)
                return Response<bool>.Failure("Invalid question ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.SoftDelete(questionId);
                return ok ? Response<bool>.Success(true, "Question deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SaveStudentAnswer(int testId, int questionId, int studentId, string answer, decimal marksObtained)
        {
            if (testId <= 0 || questionId <= 0 || studentId <= 0)
                return Response<bool>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                // Use StudentAnswerDAL
                var saDAL = new StudentAnswerDAL();
                var existing = saDAL.GetByTestAndStudent(testId, studentId);
                var existingAnswer = existing.FirstOrDefault(a => a.QuestionID == questionId);
                if (existingAnswer != null)
                {
                    existingAnswer.AnswerText = answer;
                    existingAnswer.MarksObtained = marksObtained;
                    bool ok = saDAL.Update(existingAnswer);
                    return ok ? Response<bool>.Success(true, "Answer updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
                }
                else
                {
                    var newAnswer = new StudentAnswer
                    {
                        TestID = testId,
                        QuestionID = questionId,
                        StudentID = studentId,
                        AnswerText = answer,
                        MarksObtained = marksObtained
                    };
                    int id = saDAL.Create(newAnswer);
                    return id > 0 ? Response<bool>.Success(true, "Answer saved.") : Response<bool>.Failure("Save failed.", "CREATE_FAILED");
                }
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<StudentAnswer>> GetStudentAnswers(int testId, int studentId)
        {
            if (testId <= 0 || studentId <= 0)
                return Response<List<StudentAnswer>>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var saDAL = new StudentAnswerDAL();
                var answers = saDAL.GetByTestAndStudent(testId, studentId);
                return Response<List<StudentAnswer>>.Success(answers);
            }
            catch (Exception ex)
            {
                return Response<List<StudentAnswer>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}