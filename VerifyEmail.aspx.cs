using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;

namespace EduTrack.Auth
{
    public partial class VerifyEmail : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                string email = Request.QueryString["email"];

                if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(email))
                {
                    ShowError("Invalid verification link. Missing token or email.");
                    return;
                }

                var userResp = _userBLL.GetUserByEmail(email);
                if (!userResp.IsSuccess || userResp.Data == null)
                {
                    ShowError("User not found.");
                    return;
                }

                var user = userResp.Data;
                if (string.IsNullOrWhiteSpace(user.ResetTokenHash))
                {
                    ShowError("No verification request found. Your email may already be verified.");
                    return;
                }

                if (PasswordHelper.VerifyToken(token, user.ResetTokenHash))
                {
                    user.Email = email; // Mark as verified
                    user.ResetTokenHash = null;
                    user.ResetTokenExpiry = null;

                    var updateResp = _userBLL.UpdateUser(user);
                    if (!updateResp.IsSuccess)
                    {
                        ShowError("Failed to verify email. Please try again.");
                        return;
                    }

                    divSuccess.Visible = true;
                    lblMessage.Visible = false;
                }
                else
                {
                    ShowError("Invalid or expired verification token.");
                }
            }
        }

        private void ShowError(string message)
        {
            divError.Visible = true;
            lblErrorDetail.Text = message;
            lblMessage.Visible = false;
        }
    }
}