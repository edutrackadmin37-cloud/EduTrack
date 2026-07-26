using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class UserSubjectDAL : BaseDAL
    {
        public List<UserSubject> GetAll()
        {
            List<UserSubject> list = new List<UserSubject>();
            using (SqlDataReader r = ExecuteReader("sp_GetAllUserSubjects"))
            {
                while (r.Read())
                {
                    list.Add(new UserSubject
                    {
                        UserSubjectID = GetValue<int>(r, "UserSubjectID"),
                        UserID = GetValue<int>(r, "UserID"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        Role = GetValue<string>(r, "Role"),
                        Status = GetValue<string>(r, "Status"),
                        RequestedOn = GetValue<DateTime>(r, "RequestedOn"),
                        ApprovedOn = GetValue<DateTime?>(r, "ApprovedOn"),
                        ApprovedBy = GetValue<int?>(r, "ApprovedBy"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        FullName = GetValue<string>(r, "FullName"),
                        Email = GetValue<string>(r, "Email"),
                        SubjectName = GetValue<string>(r, "SubjectName")
                    });
                }
            }
            return list;
        }

        public List<UserSubject> GetByUser(int userId)
        {
            List<UserSubject> list = new List<UserSubject>();
            using (SqlDataReader r = ExecuteReader("sp_GetUserSubjectsByUser", new SqlParameter("@UserID", userId)))
            {
                while (r.Read())
                {
                    list.Add(new UserSubject
                    {
                        UserSubjectID = GetValue<int>(r, "UserSubjectID"),
                        UserID = GetValue<int>(r, "UserID"),
                        SubjectID = GetValue<int>(r, "SubjectID"),
                        Role = GetValue<string>(r, "Role"),
                        Status = GetValue<string>(r, "Status"),
                        RequestedOn = GetValue<DateTime>(r, "RequestedOn"),
                        ApprovedOn = GetValue<DateTime?>(r, "ApprovedOn"),
                        ApprovedBy = GetValue<int?>(r, "ApprovedBy"),
                        CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                        UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                        IsDeleted = GetValue<bool>(r, "IsDeleted"),
                        SubjectName = GetValue<string>(r, "SubjectName"),
                        SubjectCode = GetValue<string>(r, "SubjectCode")
                    });
                }
            }
            return list;
        }

        public UserSubject GetById(int userSubjectId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetUserSubjectById", new SqlParameter("@UserSubjectID", userSubjectId)))
            {
                if (!r.Read()) return null;
                return new UserSubject
                {
                    UserSubjectID = GetValue<int>(r, "UserSubjectID"),
                    UserID = GetValue<int>(r, "UserID"),
                    SubjectID = GetValue<int>(r, "SubjectID"),
                    Role = GetValue<string>(r, "Role"),
                    Status = GetValue<string>(r, "Status"),
                    RequestedOn = GetValue<DateTime>(r, "RequestedOn"),
                    ApprovedOn = GetValue<DateTime?>(r, "ApprovedOn"),
                    ApprovedBy = GetValue<int?>(r, "ApprovedBy"),
                    CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                    UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                    IsDeleted = GetValue<bool>(r, "IsDeleted")
                };
            }
        }

        public int Create(UserSubject model)
        {
            SqlParameter outId = new SqlParameter("@NewUserSubjectID", SqlDbType.Int) { Direction = ParameterDirection.Output };
            ExecuteNonQuery("sp_CreateUserSubject",
                new SqlParameter("@UserID", model.UserID),
                new SqlParameter("@SubjectID", model.SubjectID),
                new SqlParameter("@Role", model.Role),
                new SqlParameter("@Status", model.Status),
                outId
            );
            return Convert.ToInt32(outId.Value);
        }

        public bool UpdateStatus(int userSubjectId, string status, int? approvedBy)
        {
            int rows = ExecuteNonQuery("sp_UpdateUserSubjectStatus",
                new SqlParameter("@UserSubjectID", userSubjectId),
                new SqlParameter("@Status", status),
                new SqlParameter("@ApprovedBy", (object)approvedBy ?? DBNull.Value)
            );
            return rows > 0;
        }

        public bool SoftDelete(int userSubjectId) => ExecuteNonQuery("sp_SoftDeleteUserSubject", new SqlParameter("@UserSubjectID", userSubjectId)) > 0;
    }
}