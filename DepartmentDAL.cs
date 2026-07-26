using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class DepartmentDAL : BaseDAL
    {
        public List<Department> GetAll()
        {
            List<Department> list = new List<Department>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllDepartments"))
            {
                while (r.Read()) list.Add(Map(r));
            }
            return list;
        }

        public Department GetById(int departmentId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetDepartmentById", new SqlParameter("@DepartmentID", departmentId)))
            {
                if (!r.Read()) return null;
                return Map(r);
            }
        }

        public int Create(Department department)
        {
            SqlParameter outId = new SqlParameter("@NewDepartmentID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            ExecuteNonQuery("sp_CreateDepartment",
                new SqlParameter("@DepartmentName", department.DepartmentName),
                new SqlParameter("@Description", (object)department.Description ?? DBNull.Value),
                new SqlParameter("@HeadOfDepartmentID", (object)department.HeadOfDepartmentID ?? DBNull.Value),
                outId
            );

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(Department department)
        {
            int rows = ExecuteNonQuery("sp_UpdateDepartment",
                new SqlParameter("@DepartmentID", department.DepartmentID),
                new SqlParameter("@DepartmentName", (object)department.DepartmentName ?? DBNull.Value),
                new SqlParameter("@Description", (object)department.Description ?? DBNull.Value),
                new SqlParameter("@HeadOfDepartmentID", (object)department.HeadOfDepartmentID ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int departmentId)
        {
            return ExecuteNonQuery("sp_SoftDeleteDepartment", new SqlParameter("@DepartmentID", departmentId)) > 0;
        }

        private Department Map(SqlDataReader r)
        {
            return new Department
            {
                DepartmentID = GetValue<int>(r, "DepartmentID"),
                DepartmentName = GetValue<string>(r, "DepartmentName"),
                Description = GetValue<string>(r, "Description"),
                HeadOfDepartmentID = GetValue<int?>(r, "HeadOfDepartmentID"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}