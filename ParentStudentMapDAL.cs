using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class ParentStudentMapDAL : BaseDAL
    {
        public List<ParentStudentMap> GetAll()
        {
            List<ParentStudentMap> list = new List<ParentStudentMap>();
            using (SqlDataReader r = ExecuteReader("sp_GetParentStudentMap"))
            {
                while (r.Read())
                {
                    list.Add(new ParentStudentMap
                    {
                        MapID = GetValue<int>(r, "MapID"),
                        ParentID = GetValue<int>(r, "ParentID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        ParentName = GetValue<string>(r, "ParentName"),
                        StudentName = GetValue<string>(r, "StudentName")
                    });
                }
            }
            return list;
        }

        public List<ParentStudentMap> GetChildrenForParent(int parentId)
        {
            List<ParentStudentMap> list = new List<ParentStudentMap>();
            using (SqlDataReader r = ExecuteReader("sp_GetChildrenForParent", new SqlParameter("@ParentID", parentId)))
            {
                while (r.Read())
                {
                    list.Add(new ParentStudentMap
                    {
                        MapID = GetValue<int>(r, "MapID"),
                        ParentID = GetValue<int>(r, "ParentID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        FullName = GetValue<string>(r, "FullName"),
                        Email = GetValue<string>(r, "Email"),
                        DateOfBirth = GetValue<DateTime?>(r, "DateOfBirth")
                    });
                }
            }
            return list;
        }

        public List<ParentStudentMap> GetParentsForStudent(int studentId)
        {
            List<ParentStudentMap> list = new List<ParentStudentMap>();
            using (SqlDataReader r = ExecuteReader("sp_GetParentsForStudent", new SqlParameter("@StudentID", studentId)))
            {
                while (r.Read())
                {
                    list.Add(new ParentStudentMap
                    {
                        MapID = GetValue<int>(r, "MapID"),
                        ParentID = GetValue<int>(r, "ParentID"),
                        StudentID = GetValue<int>(r, "StudentID"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        FullName = GetValue<string>(r, "FullName"),
                        Email = GetValue<string>(r, "Email"),
                        PhoneNumber = GetValue<string>(r, "PhoneNumber")
                    });
                }
            }
            return list;
        }

        public int Create(int parentId, int studentId)
        {
            SqlParameter outId = new SqlParameter("@NewMapID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateParentStudentMap",
                new SqlParameter("@ParentID", parentId),
                new SqlParameter("@StudentID", studentId),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool SoftDelete(int mapId) => ExecuteNonQuery("sp_SoftDeleteParentStudentMap", new SqlParameter("@MapID", mapId)) > 0;
    }
}