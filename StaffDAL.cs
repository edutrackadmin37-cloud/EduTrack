// ============================================================
// DAL/StaffDAL.cs
// ============================================================
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class StaffDAL : BaseDAL
    {
        public List<Staff> GetAll()
        {
            var list = new List<Staff>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllStaff"))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public Staff GetById(int id)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetStaffById", new SqlParameter("@StaffID", id)))
            {
                if (!r.Read()) return null;
                return MapWithContext(r);
            }
        }

        public List<Staff> GetBySchool(int schoolId)
        {
            var list = new List<Staff>();
            using (SqlDataReader r = ExecuteReader("sp_GetStaffBySchool", new SqlParameter("@SchoolID", schoolId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public List<Staff> GetByDepartment(int departmentId)
        {
            var list = new List<Staff>();
            using (SqlDataReader r = ExecuteReader("sp_GetStaffByDepartment", new SqlParameter("@DepartmentID", departmentId)))
            {
                while (r.Read())
                {
                    list.Add(MapWithContext(r));
                }
            }
            return list;
        }

        public int Create(Staff model)
        {
            SqlParameter outId = new SqlParameter("@NewStaffID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateStaff",
                new SqlParameter("@UserID", model.UserID),
                new SqlParameter("@SchoolID", (object)model.SchoolID ?? DBNull.Value),
                new SqlParameter("@StaffNumber", (object)model.StaffNumber ?? DBNull.Value),
                new SqlParameter("@Position", (object)model.Position ?? DBNull.Value),
                new SqlParameter("@DepartmentID", (object)model.DepartmentID ?? DBNull.Value),
                new SqlParameter("@HireDate", (object)model.HireDate ?? DBNull.Value),
                new SqlParameter("@IsActive", model.IsActive),
                outId);
            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Staff model)
        {
            int rows = ExecuteNonQuery("sp_UpdateStaff",
                new SqlParameter("@StaffID", model.StaffID),
                new SqlParameter("@UserID", model.UserID),
                new SqlParameter("@SchoolID", (object)model.SchoolID ?? DBNull.Value),
                new SqlParameter("@StaffNumber", (object)model.StaffNumber ?? DBNull.Value),
                new SqlParameter("@Position", (object)model.Position ?? DBNull.Value),
                new SqlParameter("@DepartmentID", (object)model.DepartmentID ?? DBNull.Value),
                new SqlParameter("@HireDate", (object)model.HireDate ?? DBNull.Value),
                new SqlParameter("@IsActive", model.IsActive));
            return rows > 0;
        }

        public bool SoftDelete(int id)
        {
            return ExecuteNonQuery("sp_SoftDeleteStaff", new SqlParameter("@StaffID", id)) > 0;
        }

        private Staff Map(SqlDataReader r)
        {
            return new Staff
            {
                StaffID = GetValue<int>(r, "StaffID"),
                UserID = GetValue<int>(r, "UserID"),
                SchoolID = GetValue<int?>(r, "SchoolID"),
                StaffNumber = GetValue<string>(r, "StaffNumber"),
                Position = GetValue<string>(r, "Position"),
                DepartmentID = GetValue<int?>(r, "DepartmentID"),
                HireDate = GetValue<DateTime?>(r, "HireDate"),
                IsActive = GetValue<bool>(r, "IsActive"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }

        private Staff MapWithContext(SqlDataReader r)
        {
            var staff = Map(r);
            staff.FullName = GetValue<string>(r, "FullName");
            staff.Email = GetValue<string>(r, "Email");
            staff.DepartmentName = GetValue<string>(r, "DepartmentName");
            staff.SchoolName = GetValue<string>(r, "SchoolName");
            return staff;
        }
    }
}