// ============================================================
// BLL/AcademicCalendarBLL.cs
// ============================================================
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Collections.Generic;

namespace EduTrack.BLL
{
    public class AcademicCalendarBLL
    {
        private readonly AcademicCalendarDAL _dal = new AcademicCalendarDAL();

        public Response<List<AcademicCalendar>> GetAll()
        {
            try
            {
                var data = _dal.GetAll();
                return Response<List<AcademicCalendar>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<AcademicCalendar>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<List<AcademicCalendar>> GetBySchool(int schoolId)
        {
            if (schoolId <= 0) return Response<List<AcademicCalendar>>.Failure("Invalid School ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetBySchool(schoolId);
                return Response<List<AcademicCalendar>>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<List<AcademicCalendar>>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<AcademicCalendar> GetById(int id)
        {
            if (id <= 0) return Response<AcademicCalendar>.Failure("Invalid Calendar ID.", "VALIDATION_ERROR");
            try
            {
                var data = _dal.GetById(id);
                return data == null
                    ? Response<AcademicCalendar>.Failure("Event not found.", "NOT_FOUND")
                    : Response<AcademicCalendar>.Success(data);
            }
            catch (Exception ex)
            {
                return Response<AcademicCalendar>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<int> Create(AcademicCalendar model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.EventTitle) || model.SchoolID <= 0)
                return Response<int>.Failure("Invalid event data.", "VALIDATION_ERROR");

            try
            {
                int id = _dal.Create(model);
                return id > 0
                    ? Response<int>.Success(id, "Event created.")
                    : Response<int>.Failure("Create failed.", "CREATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<int>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> Update(AcademicCalendar model)
        {
            if (model == null || model.CalendarID <= 0)
                return Response<bool>.Failure("Invalid event data.", "VALIDATION_ERROR");

            try
            {
                bool ok = _dal.Update(model);
                return ok
                    ? Response<bool>.Success(true, "Event updated.")
                    : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }

        public Response<bool> SoftDelete(int id)
        {
            if (id <= 0) return Response<bool>.Failure("Invalid Calendar ID.", "VALIDATION_ERROR");
            try
            {
                bool ok = _dal.SoftDelete(id);
                return ok
                    ? Response<bool>.Success(true, "Event deleted.")
                    : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
            }
            catch (Exception ex)
            {
                return Response<bool>.Failure($"Error: {ex.Message}", "DAL_ERROR");
            }
        }
    }
}