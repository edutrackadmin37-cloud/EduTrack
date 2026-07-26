// ============================================================
// DAL/AcademicCalendarDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class AcademicCalendarDAL : BaseDAL
    {
        public List<AcademicCalendar> GetAll()
        {
            var list = new List<AcademicCalendar>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllAcademicCalendars"))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<AcademicCalendar> GetBySchool(int schoolId)
        {
            var list = new List<AcademicCalendar>();
            using (SqlDataReader r = ExecuteReader("sp_GetAcademicCalendarsBySchool", new SqlParameter("@SchoolID", schoolId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public AcademicCalendar GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetAcademicCalendarById", new SqlParameter("@CalendarID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public int Create(AcademicCalendar model)
        {
            SqlParameter outId = new SqlParameter("@NewCalendarID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateAcademicCalendar",
                new SqlParameter("@SchoolID", model.SchoolID),
                new SqlParameter("@EventTitle", model.EventTitle),
                new SqlParameter("@EventDescription", (object)model.EventDescription ?? DBNull.Value),
                new SqlParameter("@EventDate", model.EventDate),
                new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                new SqlParameter("@EventType", (object)model.EventType ?? DBNull.Value),
                new SqlParameter("@IsPublic", model.IsPublic),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(AcademicCalendar model)
        {
            int rows = ExecuteNonQuery("sp_UpdateAcademicCalendar",
                new SqlParameter("@CalendarID", model.CalendarID),
                new SqlParameter("@EventTitle", (object)model.EventTitle ?? DBNull.Value),
                new SqlParameter("@EventDescription", (object)model.EventDescription ?? DBNull.Value),
                new SqlParameter("@EventDate", (object)model.EventDate ?? DBNull.Value),
                new SqlParameter("@EndDate", (object)model.EndDate ?? DBNull.Value),
                new SqlParameter("@EventType", (object)model.EventType ?? DBNull.Value),
                new SqlParameter("@IsPublic", model.IsPublic));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteAcademicCalendar", new SqlParameter("@CalendarID", id)) > 0;
        }

        private AcademicCalendar Map(SqlDataReader r)
        {
            return new AcademicCalendar
            {
                CalendarID = GetValue<int>(r, "CalendarID"),
                SchoolID = GetValue<int>(r, "SchoolID"),
                EventTitle = GetValue<string>(r, "EventTitle"),
                EventDescription = GetValue<string>(r, "EventDescription"),
                EventDate = GetValue<DateTime>(r, "EventDate"),
                EndDate = GetValue<DateTime?>(r, "EndDate"),
                EventType = GetValue<string>(r, "EventType"),
                IsPublic = GetValue<bool>(r, "IsPublic"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private AcademicCalendar MapWithContext(SqlDataReader r)
        {
            var cal = Map(r);
            cal.SchoolName = GetValue<string>(r, "SchoolName");
            return cal;
        }
    }
}