using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using EduTrack.Services;
using System;
using System.Configuration;
using System.Web;

namespace EduTrack.Auth
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly string _platformUrl;

        public ForgotPassword()
        {
            _platformUrl = ConfigurationManager.AppSettings["PlatformURL"] ?? "http://localhost/EduTrack";

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
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                lblMessage.Visible = false;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim();

            // Show success message regardless (security best practice)
            ShowMessage("If a user with that email exists, you will receive a password reset link shortly.", "alert alert-success");
            btnReset.Enabled = false;

            var userResp = _userBLL.GetUserByEmail(email);
            if (!userResp.IsSuccess || userResp.Data == null)
                return;

            var user = userResp.Data;

            // Verify user is approved and active
            if (!user.IsActive || !user.IsApproved || user.ApprovalStatus != "Approved")
                return;

            // Generate reset token
            string token = PasswordHelper.GenerateResetToken();
            string tokenHash = PasswordHelper.HashToken(token);

            user.ResetTokenHash = tokenHash;
            user.ResetTokenExpiry = DateTime.Now.AddHours(1);

            var updateResp = _userBLL.UpdateUser(user);
            if (!updateResp.IsSuccess)
                return;

            // Build reset URL with proper encoding
            string resetUrl = GetResetUrl(user.UserID, user.Email, token);

            // Send SMS if phone provided
            if (!string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                try
                {
                    _smsService.SendSmsAsync(txtPhone.Text.Trim(),
                        $"EduTrack Password Reset: {resetUrl}");
                }
                catch { /* Log silently */ }
            }

            // Send email
            string emailBody = $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h2>Password Reset Request</h2>
                    <p>Hello {user.FullName},</p>
                    <p>We received a request to reset your password. Click the link below to proceed:</p>
                    <p><a href='{resetUrl}' style='background: #667eea; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Reset Password</a></p>
                    <p>Or copy this link: <br/>{resetUrl}</p>
                    <p><strong>This link expires in 1 hour.</strong></p>
                    <p>If you did not request this, please ignore this email.</p>
                    <br/>
                    <p>Regards,<br/>EduTrack Team</p>
                </body>
                </html>";

            try
            {
                _emailService.SendEmailAsync(user.Email, "EduTrack Password Reset", emailBody);
            }
            catch { /* Log silently */ }
        }

        private string GetResetUrl(int userId, string email, string token)
        {
            string baseUrl = _platformUrl.TrimEnd('/');
            string encodedEmail = HttpUtility.UrlEncode(email);
            string encodedToken = HttpUtility.UrlEncode(token);

            return $"{baseUrl}/Auth/ResetPassword.aspx?uid={userId}&email={encodedEmail}&token={encodedToken}";
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}