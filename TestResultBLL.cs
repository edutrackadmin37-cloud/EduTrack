using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class TestResultBLL
    {
        private readonly TestResultDAL _dal = new TestResultDAL();

        public Response<TestResult> GetLatestResult(int testId, int studentId)
        {
            if (testId <= 0 || studentId <= 0)
                return Response<TestResult>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var results = _dal.GetByTest(testId);
                var latest = results.Where(r => r.StudentID == studentId && !r.IsDeleted)
                                     .OrderByDescending(r => r.DateRecorded)
                                     .FirstOrDefault();
                return latest == null
                    ? Response<TestResult>.Failure("No result found.", "NOT_FOUND")
                    : Response<TestResult>.Success(latest);
            }
            catch (Exception ex)
            {
                return Response<TestResult>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<TestResult>> GetResultsByTestAndStudent(int testId, int studentId)
        {
            if (testId <= 0 || studentId <= 0)
                return Response<List<TestResult>>.Failure("Invalid IDs.", "VALIDATION_ERROR");

            try
            {
                var results = _dal.GetByTest(testId);
                var filtered = results.Where(r => r.StudentID == studentId && !r.IsDeleted).ToList();
                return Response<List<TestResult>>.Success(filtered);
            }
            catch (Exception ex)
            {
                return Response<List<TestResult>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SaveTestResult(TestResult result)
        {
            if (result == null || result.TestID <= 0 || result.StudentID <= 0)
                return Response<bool>.Failure("Invalid result data.", "VALIDATION_ERROR");

            try
            {
                // Check if exists, then update, else create
                var existing = _dal.GetByTestAndStudent(result.TestID, result.StudentID);
                if (existing != null)
                {
                    existing.TotalMarks = result.TotalMarks;
                    existing.Percentage = result.Percentage;
                    existing.ResultGrade = result.ResultGrade;
                    existing.DateRecorded = result.DateRecorded;
                    bool ok = _dal.Update(existing);
                    return ok ? Response<bool>.Success(true, "Result updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
                }
                else
                {
                    int id = _dal.Create(result);
                    return id > 0 ? Response<bool>.Success(true, "Result saved.") : Response<bool>.Failure("Save failed.", "CREATE_FAILED");
                }
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}