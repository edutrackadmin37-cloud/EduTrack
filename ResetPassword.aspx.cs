using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;

namespace EduTrack.Auth
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                string email = Request.QueryString["email"];
                string uidStr = Request.QueryString["uid"];

                // Validate parameters
                if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email) ||
                    string.IsNullOrWhiteSpace(uidStr) || !int.TryParse(uidStr, out int uid))
                {
                    ShowMessage("Invalid or missing password reset token.", "alert alert-danger");
                    btnReset.Enabled = false;
                    return;
                }

                var userResp = _userBLL.GetUserByEmail(email);
                if (!userResp.IsSuccess || userResp.Data == null)
                {
                    ShowMessage("User not found.", "alert alert-danger");
                    btnReset.Enabled = false;
                    return;
                }

                var user = userResp.Data;

                // Verify token exists
                if (string.IsNullOrWhiteSpace(user.ResetTokenHash) || !user.ResetTokenExpiry.HasValue)
                {
                    ShowMessage("No reset request found for this user.", "alert alert-danger");
                    btnReset.Enabled = false;
                    return;
                }

                // Check token expiry
                if (user.ResetTokenExpiry.Value < DateTime.Now)
                {
                    ShowMessage("Your password reset token has expired. Please request a new one.", "alert alert-danger");
                    btnReset.Enabled = false;
                    return;
                }

                // Store values for button click handler
                ViewState["Token"] = token;
                ViewState["Email"] = email;
                ViewState["UID"] = uid;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string token = ViewState["Token"]?.ToString();
            string email = ViewState["Email"]?.ToString();

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
            {
                ShowMessage("Invalid reset session. Please request a new password reset.", "alert alert-danger");
                return;
            }

            var userResp = _userBLL.GetUserByEmail(email);
            if (!userResp.IsSuccess || userResp.Data == null)
            {
                ShowMessage("User not found.", "alert alert-danger");
                return;
            }

            var user = userResp.Data;

            // Verify token validity
            if (!PasswordHelper.VerifyToken(token, user.ResetTokenHash))
            {
                ShowMessage("Invalid password reset token.", "alert alert-danger");
                return;
            }

            // Update password
            var resp = _userBLL.ResetPassword(email, token, txtPassword.Text.Trim());
            if (!resp.IsSuccess)
            {
                ShowMessage(resp.Message, "alert alert-danger");
                return;
            }

            // Redirect to login with success message
            Response.Redirect(ResolveUrl("~/Auth/Login.aspx?reset=1"));
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}