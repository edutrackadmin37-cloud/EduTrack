// ============================================================
// BLL/SchoolReportBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;

namespace EduTrack.BLL
{
    public class SchoolReportBLL
    {
        private readonly SchoolReportDAL _dal = new SchoolReportDAL();

        public Response<SchoolReport> GetReport(int schoolId, int academicYearId)
        {
            if (schoolId <= 0 || academicYearId <= 0)
                return Response<SchoolReport>.Failure("Invalid IDs.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetReport(schoolId, academicYearId);
                return data == null
                    ? Response<SchoolReport>.Failure("No report data found.", "NOT_FOUND")
                    : Response<SchoolReport>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<SchoolReport>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}