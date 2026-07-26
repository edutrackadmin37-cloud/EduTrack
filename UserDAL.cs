using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EduTrack.DAL
{
    public class UserDAL : BaseDAL
    {
        public User GetById(int userId)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetUserById", new[] { new SqlParameter("@UserID", userId) }))
            {
                if (!r.Read())
                {
                    return null;
                }

                return MapToObject(r);
            }
        }

        public User GetByEmail(string email)
        {
            using (SqlDataReader r = ExecuteReader("sp_GetUserByEmail", new[] { new SqlParameter("@Email", email) }))
            {
                if (!r.Read())
                {
                    return null;
                }

                return MapToObject(r);
            }
        }

        public List<User> GetAll()
        {
            using (SqlDataReader r = ExecuteReader("sp_GetAllUsers"))
            {
                return MapToList(r);
            }
        }

        public int Create(User user)
        {
            SqlParameter outId = new SqlParameter("@NewUserID", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            ExecuteNonQuery("sp_CreateUser",
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Role", user.Role),
                new SqlParameter("@IsApproved", user.IsApproved),
                new SqlParameter("@ApprovalStatus", user.ApprovalStatus ?? "Pending"),
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@ProfilePicture", (object)user.ProfilePicture ?? DBNull.Value),
                new SqlParameter("@Bio", (object)user.Bio ?? DBNull.Value),
                new SqlParameter("@PhoneNumber", (object)user.PhoneNumber ?? DBNull.Value),
                new SqlParameter("@DateOfBirth", (object)user.DateOfBirth ?? DBNull.Value),
                new SqlParameter("@Address", (object)user.Address ?? DBNull.Value),
                new SqlParameter("@Gender", (object)user.Gender ?? DBNull.Value),
                new SqlParameter("@NationalID", (object)user.NationalID ?? DBNull.Value),
                new SqlParameter("@EmergencyContact", (object)user.EmergencyContact ?? DBNull.Value),
                outId);

            return Convert.ToInt32(outId.Value);
        }

        public bool Update(User user)
        {
            int rows = ExecuteNonQuery("sp_UpdateUser",
                new SqlParameter("@UserID", user.UserID),
                new SqlParameter("@Email", (object)user.Email ?? DBNull.Value),
                new SqlParameter("@PasswordHash", (object)user.PasswordHash ?? DBNull.Value),
                new SqlParameter("@FullName", (object)user.FullName ?? DBNull.Value),
                new SqlParameter("@Role", (object)user.Role ?? DBNull.Value),
                new SqlParameter("@IsApproved", user.IsApproved),
                new SqlParameter("@ApprovalStatus", (object)user.ApprovalStatus ?? DBNull.Value),
                new SqlParameter("@IsActive", user.IsActive),
                new SqlParameter("@ProfilePicture", (object)user.ProfilePicture ?? DBNull.Value),
                new SqlParameter("@Bio", (object)user.Bio ?? DBNull.Value),
                new SqlParameter("@PhoneNumber", (object)user.PhoneNumber ?? DBNull.Value),
                new SqlParameter("@DateOfBirth", (object)user.DateOfBirth ?? DBNull.Value),
                new SqlParameter("@Address", (object)user.Address ?? DBNull.Value),
                new SqlParameter("@Gender", (object)user.Gender ?? DBNull.Value),
                new SqlParameter("@NationalID", (object)user.NationalID ?? DBNull.Value),
                new SqlParameter("@EmergencyContact", (object)user.EmergencyContact ?? DBNull.Value),
                new SqlParameter("@LastLogin", (object)user.LastLogin ?? DBNull.Value));

            return rows > 0;
        }

        public bool SoftDelete(int userId)
        {
            return ExecuteNonQuery("sp_SoftDeleteUser", new[] { new SqlParameter("@UserID", userId) }) > 0;
        }

        private User MapToObject(SqlDataReader r)
        {
            return Map(r);
        }

        private List<User> MapToList(SqlDataReader r)
        {
            List<User> list = new List<User>();

            while (r.Read())
            {
                list.Add(Map(r));
            }

            return list;
        }

        private User Map(SqlDataReader r)
        {
            return new User
            {
                UserID = GetValue<int>(r, "UserID"),
                Email = GetValue<string>(r, "Email"),
                PasswordHash = GetValue<string>(r, "PasswordHash"),
                FullName = GetValue<string>(r, "FullName"),
                Role = GetValue<string>(r, "Role"),
                IsApproved = GetValue<bool>(r, "IsApproved"),
                ApprovalStatus = GetValue<string>(r, "ApprovalStatus"),
                IsActive = GetValue<bool>(r, "IsActive"),
                ProfilePicture = GetValue<string>(r, "ProfilePicture"),
                Bio = GetValue<string>(r, "Bio"),
                PhoneNumber = GetValue<string>(r, "PhoneNumber"),
                DateOfBirth = GetValue<DateTime?>(r, "DateOfBirth"),
                Address = GetValue<string>(r, "Address"),
                Gender = GetValue<string>(r, "Gender"),
                NationalID = GetValue<string>(r, "NationalID"),
                EmergencyContact = GetValue<string>(r, "EmergencyContact"),
                ResetTokenHash = GetValue<string>(r, "ResetTokenHash"),
                ResetTokenExpiry = GetValue<DateTime?>(r, "ResetTokenExpiry"),
                ResetTemporaryPassword = GetValue<string>(r, "ResetTemporaryPassword"),
                LastLogin = GetValue<DateTime?>(r, "LastLogin"),
                CreatedAt = GetValue<DateTime>(r, "CreatedAt"),
                UpdatedAt = GetValue<DateTime?>(r, "UpdatedAt"),
                IsDeleted = GetValue<bool>(r, "IsDeleted")
            };
        }
    }
}