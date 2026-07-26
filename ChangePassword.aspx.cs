using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;

namespace EduTrack.Auth
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMessage.Visible = false;

                string token = Request.QueryString["token"];
                string uid = Request.QueryString["uid"];
                bool force = Request.QueryString["force"] == "1";

                if (force)
                {
                    var currentUser = SessionManager.GetCurrentUser();
                    if (currentUser != null)
                    {
                        hfToken.Value = "force";
                        hfUserId.Value = currentUser.UserID.ToString();
                        divCurrent.Visible = true;
                        pInstruction.InnerText = "You must change your password before continuing.";
                        return;
                    }
                    // Force redirect to login if not authenticated
                    Response.Redirect(ResolveUrl("~/Auth/Login.aspx"));
                    return;
                }

                // Password reset via token
                if (!string.IsNullOrWhiteSpace(token) && !string.IsNullOrWhiteSpace(uid) && int.TryParse(uid, out int userId))
                {
                    hfUserId.Value = userId.ToString();
                    hfToken.Value = token;
                    divCurrent.Visible = false;
                    pInstruction.InnerText = "Create a new password for your account.";
                    return;
                }

                // Regular password change (authenticated user)
                var user = SessionManager.GetCurrentUser();
                if (user == null)
                {
                    Response.Redirect(ResolveUrl("~/Auth/Login.aspx"));
                    return;
                }

                hfUserId.Value = user.UserID.ToString();
                divCurrent.Visible = true;
                pInstruction.InnerText = "Enter your current password and choose a new password.";
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string newPassword = txtNewPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                ShowMessage("Passwords do not match.", "alert alert-danger");
                return;
            }

            if (!ValidationHelper.IsValidPassword(newPassword))
            {
                ShowMessage("Password does not meet complexity requirements.", "alert alert-danger");
                return;
            }

            bool tokenMode = !string.IsNullOrWhiteSpace(hfToken.Value) && hfToken.Value != "force";

            if (!int.TryParse(hfUserId.Value, out int userId))
            {
                ShowMessage("Invalid user ID.", "alert alert-danger");
                return;
            }

            var userResp = _userBLL.GetUserById(userId);
            if (!userResp.IsSuccess || userResp.Data == null)
            {
                ShowMessage("User not found.", "alert alert-danger");
                return;
            }

            var user = userResp.Data;

            if (tokenMode)
            {
                // Verify reset token
                if (string.IsNullOrWhiteSpace(user.ResetTokenHash) || !user.ResetTokenExpiry.HasValue)
                {
                    ShowMessage("No reset request found for this user.", "alert alert-danger");
                    return;
                }

                if (user.ResetTokenExpiry.Value < DateTime.Now)
                {
                    ShowMessage("Your password reset token has expired.", "alert alert-danger");
                    return;
                }

                if (!PasswordHelper.VerifyToken(hfToken.Value, user.ResetTokenHash))
                {
                    ShowMessage("Invalid reset token.", "alert alert-danger");
                    return;
                }
            }
            else
            {
                // Verify current password
                if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
                {
                    ShowMessage("Current password is required.", "alert alert-danger");
                    return;
                }

                if (!PasswordHelper.VerifyPassword(txtCurrentPassword.Text, user.PasswordHash))
                {
                    ShowMessage("Current password is incorrect.", "alert alert-danger");
                    return;
                }
            }

            // Update password
            user.PasswordHash = PasswordHelper.HashPassword(newPassword);
            user.ResetTokenHash = null;
            user.ResetTokenExpiry = null;
            user.ResetTemporaryPassword = null;

            var updateResp = _userBLL.UpdateUser(user);
            if (!updateResp.IsSuccess)
            {
                ShowMessage(updateResp.Message, "alert alert-danger");
                return;
            }

            ShowMessage("Password changed successfully.", "alert alert-success");
            btnChange.Enabled = false;

            // Redirect after 2 seconds
            string redirectUrl = tokenMode ? "~/Auth/Login.aspx" : "~/Default.aspx";
            ClientScript.RegisterStartupScript(GetType(), "redirect", $"setTimeout(function(){{window.location='{ResolveUrl(redirectUrl)}'}}, 2000);", true);
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}