// ============================================================
// BLL/UserBLL.cs – UPDATED with real email/SMS services
// ============================================================
using EduTrack.DAL;
using EduTrack.Helpers;
using EduTrack.Models;
using EduTrack.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace EduTrack.BLL
{
    public class UserBLL
    {
        private readonly UserDAL _userDAL = new UserDAL();
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly string _platformUrl;

        /// <summary>
        /// Initializes the BLL with email and SMS services based on Web.config settings.
        /// </summary>
        public UserBLL()
        {
            string emailProvider = ConfigurationManager.AppSettings["EmailProvider"]?.ToLower() ?? "smtp";
            if (emailProvider == "sendgrid")
                _emailService = new SendGridEmailService();
            else
                _emailService = new EmailService();

            string smsProvider = ConfigurationManager.AppSettings["SmsProvider"]?.ToLower() ?? "stub";
            if (smsProvider == "twilio")
                _smsService = new TwilioSmsService();
            else if (smsProvider == "hubtel")
                _smsService = new HubtelSmsService();
            else
                _smsService = new SmsServiceStub();

            _platformUrl = ConfigurationManager.AppSettings["PlatformURL"] ?? "http://localhost/EduTrack";
        }
        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        public Response<User> GetUserById(int userId)
        {
            if (userId <= 0) return Response<User>.Failure("Invalid user ID.", "VALIDATION_ERROR");
            User user = _userDAL.GetById(userId);
            return user == null ? Response<User>.Failure("User not found.", "NOT_FOUND") : Response<User>.Success(user);
        }

        /// <summary>
        /// Gets a user by email.
        /// </summary>
        public Response<User> GetUserByEmail(string email)
        {
            if (!ValidationHelper.IsValidEmail(email)) return Response<User>.Failure("Invalid email address.", "VALIDATION_ERROR");
            User user = _userDAL.GetByEmail(email);
            return user == null ? Response<User>.Failure("User not found.", "NOT_FOUND") : Response<User>.Success(user);
        }

        /// <summary>
        /// Gets all users (active and inactive, soft-deleted excluded).
        /// </summary>
        public Response<List<User>> GetAllUsers()
        {
            return Response<List<User>>.Success(_userDAL.GetAll());
        }

        /// <summary>
        /// Registers a new user (creates account, sends verification email).
        /// </summary>
        public Response<int> Register(User user, string plainPassword)
        {
            // ---- Full validation (restored) ----
            if (user == null)
                return Response<int>.Failure("User data is required.", "VALIDATION_ERROR");

            if (string.IsNullOrWhiteSpace(user.FullName) || !ValidationHelper.IsValidName(user.FullName))
                return Response<int>.Failure("Full name is invalid.", "VALIDATION_ERROR");

            if (!ValidationHelper.IsValidEmail(user.Email))
                return Response<int>.Failure("Invalid email address.", "VALIDATION_ERROR");

            if (!ValidationHelper.IsValidPassword(plainPassword))
                return Response<int>.Failure("Password does not meet complexity requirements.", "VALIDATION_ERROR");

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber) && !ValidationHelper.IsValidPhone(user.PhoneNumber))
                return Response<int>.Failure("Phone number is invalid.", "VALIDATION_ERROR");

            // Check if email already exists
            User existing = _userDAL.GetByEmail(user.Email);
            if (existing != null)
                return Response<int>.Failure("Email already exists.", "DUPLICATE_EMAIL");

            // Hash password
            user.PasswordHash = PasswordHelper.HashPassword(plainPassword);
            user.IsApproved = false;
            user.ApprovalStatus = "Pending";
            user.IsActive = true;
            user.CreatedAt = DateTime.Now;

            int userId = _userDAL.Create(user);
            if (userId <= 0)
                return Response<int>.Failure("Registration failed.", "CREATE_FAILED");

            // Send verification email (asynchronously, but we'll wait)
            string verificationLink = $"{_platformUrl}/Auth/VerifyEmail.aspx?email={Uri.EscapeDataString(user.Email)}";
            _emailService.SendVerificationEmailAsync(user.Email, verificationLink, user.FullName).Wait();

            return Response<int>.Success(userId, "Registration successful. Awaiting account approval.");
        }

        /// <summary>
        /// Authenticates a user with email and password.
        /// </summary>
        public Response<User> Authenticate(string email, string password)
        {
            if (!ValidationHelper.IsValidEmail(email)) return Response<User>.Failure("Invalid email.", "VALIDATION_ERROR");
            if (string.IsNullOrWhiteSpace(password)) return Response<User>.Failure("Password is required.", "VALIDATION_ERROR");

            User user = _userDAL.GetByEmail(email);
            if (user == null) return Response<User>.Failure("Invalid credentials.", "AUTH_FAILED");
            if (!user.IsActive) return Response<User>.Failure("Account is deactivated.", "ACCOUNT_INACTIVE");
            if (!user.IsApproved || user.ApprovalStatus != "Approved")
                return Response<User>.Failure("Account not approved.", "ACCOUNT_NOT_APPROVED");
            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
                return Response<User>.Failure("Invalid credentials.", "AUTH_FAILED");

            // Update last login
            user.LastLogin = DateTime.Now;
            _userDAL.Update(user);

            return Response<User>.Success(user, "Login successful.");
        }

        /// <summary>
        /// Approves a user account (sets IsApproved = true, ApprovalStatus = "Approved").
        /// </summary>
        public Response<bool> ApproveUser(int userId, int approvedByUserId)
        {
            User user = _userDAL.GetById(userId);
            if (user == null) return Response<bool>.Failure("User not found.", "NOT_FOUND");

            user.IsApproved = true;
            user.ApprovalStatus = "Approved";

            bool ok = _userDAL.Update(user);
            if (!ok) return Response<bool>.Failure("Approval failed.", "UPDATE_FAILED");

            // Send approval email
            _emailService.SendEmailAsync(user.Email, "Account Approved", "Your EduTrack account has been approved.").Wait();
            return Response<bool>.Success(true, "User approved.");
        }

        /// <summary>
        /// Rejects a user account (sets ApprovalStatus = "Rejected").
        /// </summary>
        public Response<bool> RejectUser(int userId)
        {
            User user = _userDAL.GetById(userId);
            if (user == null) return Response<bool>.Failure("User not found.", "NOT_FOUND");

            user.IsApproved = false;
            user.ApprovalStatus = "Rejected";

            bool ok = _userDAL.Update(user);
            if (!ok) return Response<bool>.Failure("Rejection failed.", "UPDATE_FAILED");

            // Send rejection email
            _emailService.SendEmailAsync(user.Email, "Account Rejected", "Your EduTrack account request was rejected.").Wait();
            return Response<bool>.Success(true, "User rejected.");
        }

        /// <summary>
        /// Updates a user's profile information.
        /// </summary>
        public Response<bool> UpdateUser(User user)
        {
            if (user == null || user.UserID <= 0) return Response<bool>.Failure("Invalid user data.", "VALIDATION_ERROR");
            bool ok = _userDAL.Update(user);
            return ok ? Response<bool>.Success(true, "User updated.") : Response<bool>.Failure("Update failed.", "UPDATE_FAILED");
        }

        /// <summary>
        /// Soft-deletes a user (sets IsDeleted = true).
        /// </summary>
        public Response<bool> SoftDeleteUser(int userId)
        {
            if (userId <= 0) return Response<bool>.Failure("Invalid user ID.", "VALIDATION_ERROR");
            bool ok = _userDAL.SoftDelete(userId);
            return ok ? Response<bool>.Success(true, "User deleted.") : Response<bool>.Failure("Delete failed.", "DELETE_FAILED");
        }

        /// <summary>
        /// Initiates password reset: generates token, stores it, and sends reset email.
        /// </summary>
        public Response<bool> SetPasswordResetToken(string email)
        {
            User user = _userDAL.GetByEmail(email);
            if (user == null) return Response<bool>.Failure("User not found.", "NOT_FOUND");

            string token = PasswordHelper.GenerateResetToken();
            string tokenHash = PasswordHelper.HashToken(token);
            user.ResetTokenHash = tokenHash;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1);

            bool ok = _userDAL.Update(user);
            if (!ok) return Response<bool>.Failure("Could not set reset token.", "UPDATE_FAILED");

            string resetLink = $"{_platformUrl}/Auth/ResetPassword.aspx?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email)}";
            _emailService.SendPasswordResetEmailAsync(user.Email, resetLink, user.FullName).Wait();

            return Response<bool>.Success(true, "Password reset link sent.");
        }

        /// <summary>
        /// Resets the password using a valid token.
        /// </summary>
        public Response<bool> ResetPassword(string email, string token, string newPassword)
        {
            if (!ValidationHelper.IsValidPassword(newPassword))
                return Response<bool>.Failure("Invalid password format.", "VALIDATION_ERROR");

            User user = _userDAL.GetByEmail(email);
            if (user == null) return Response<bool>.Failure("User not found.", "NOT_FOUND");

            if (string.IsNullOrWhiteSpace(user.ResetTokenHash) || !user.ResetTokenExpiry.HasValue || user.ResetTokenExpiry.Value < DateTime.Now)
                return Response<bool>.Failure("Reset token invalid or expired.", "TOKEN_INVALID");

            if (!PasswordHelper.VerifyToken(token, user.ResetTokenHash))
                return Response<bool>.Failure("Reset token invalid.", "TOKEN_INVALID");

            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            user.ResetTokenHash = null;
            user.ResetTokenExpiry = null;
            user.ResetTemporaryPassword = null;

            bool ok = _userDAL.Update(user);
            return ok ? Response<bool>.Success(true, "Password reset successful.") : Response<bool>.Failure("Password reset failed.", "UPDATE_FAILED");
        }
    }
}