using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EduTrack.BLL
{
    public class TestBLL
    {
        private readonly TestDAL _testDAL = new TestDAL();
        private readonly ClassSubjectTeacherDAL _cstDAL = new ClassSubjectTeacherDAL();
        private readonly ClassDAL _classDAL = new ClassDAL();

        public Response<List<Test>> GetAllTests()
        {
            return Response<List<Test>>.Success(_testDAL.GetAll());
        }

        public Response<Test> GetTestById(int testId)
        {
            if (testId <= 0)
                return Response<Test>.Failure("Invalid test ID.", "VALIDATION_ERROR");
            var test = _testDAL.GetById(testId);
            return test == null ? Response<Test>.Failure("Test not found.", "NOT_FOUND") : Response<Test>.Success(test);
        }

        public Response<List<Test>> GetTestsByTeacher(int teacherId)
        {
            if (teacherId <= 0)
                return Response<List<Test>>.Failure("Invalid teacher ID.", "VALIDATION_ERROR");

            try
            {
                var allTests = _testDAL.GetAll();
                // Filter tests where the class-subject-teacher assignment has this teacher
                var cstIds = _cstDAL.GetAll().Where(cst => cst.TeacherID == teacherId).Select(cst => cst.ClassSubjectTeacherID).ToList();
                var tests = allTests.Where(t => cstIds.Contains(t.ClassSubjectTeacherID)).ToList();
                return Response<List<Test>>.Success(tests);
            }
            catch (Exception ex)
            {
                return Response<List<Test>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Test>> GetAvailableTests(int studentId)
        {
            if (studentId <= 0)
                return Response<List<Test>>.Failure("Invalid student ID.", "VALIDATION_ERROR");

            try
            {
                // Get classes the student is enrolled in
                var classStudentDAL = new ClassStudentDAL();
                var enrolled = classStudentDAL.GetByStudent(studentId);
                var classIds = enrolled.Select(cs => cs.ClassID).Distinct().ToList();

                // Get ClassSubjectTeacher for those classes
                var allCst = _cstDAL.GetAll();
                var cstIds = allCst.Where(cst => classIds.Contains(cst.ClassID)).Select(cst => cst.ClassSubjectTeacherID).ToList();

                var allTests = _testDAL.GetAll();
                var tests = allTests.Where(t => cstIds.Contains(t.ClassSubjectTeacherID) && !t.IsDeleted).ToList();
                return Response<List<Test>>.Success(tests);
            }
            catch (Exception ex)
            {
                return Response<List<Test>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> CreateTest(Test test, int teacherId)
        {
            if (test == null || test.ClassSubjectTeacherID <= 0 || string.IsNullOrWhiteSpace(test.Title))
                return Response<int>.Failure("Invalid test data.", "VALIDATION_ERROR");

            try
            {
                // Ensure the teacher is assigned to this class/subject
                var cst = _cstDAL.GetById(test.ClassSubjectTeacherID);
                if (cst == null || cst.TeacherID != teacherId)
                    return Response<int>.Failure("You are not authorized to create tests for this class/subject.", "AUTHORIZATION_ERROR");

                int id = _testDAL.Create(test);
                return id > 0 ? Response<int>.Success(id, "Test created.") : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> UpdateTest(Test test)
        {
            if (test == null || test.TestID <= 0)
                return Response<bool>.Failure("Invalid test data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _testDAL.Update(test);
                return ok ? Response<bool>.Success(true, "Test updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDeleteTest(int testId)
        {
            if (testId <= 0)
                return Response<bool>.Failure("Invalid test ID.", "VALIDATION_ERROR");

            try
            {
                bool ok = _testDAL.SoftDelete(testId);
                return ok ? Response<bool>.Success(true, "Test deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<Test>> GetTestsWithResults(int studentId)
        {
            // Get tests that the student has taken (results exist)
            // We'll use TestResultDAL
            var resultDAL = new TestResultDAL();
            // We'll simplify: get all tests and filter by results
            var allTests = _testDAL.GetAll();
            var tests = new List<Test>();
            foreach (var t in allTests)
            {
                var results = resultDAL.GetByTest(t.TestID);
                if (results.Any(r => r.StudentID == studentId && !r.IsDeleted))
                    tests.Add(t);
            }
            return Response<List<Test>>.Success(tests);
        }
    }
}