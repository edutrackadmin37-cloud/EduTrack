using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;
using System.Web;
using System.Web.Security;

namespace EduTrack.Auth
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["registered"] == "1")
                    ShowMessage("Registration successful. Please wait for admin approval.", "alert alert-success");

                if (Request.QueryString["reset"] == "1")
                    ShowMessage("Password reset successful. Please login with your new password.", "alert alert-success");

                if (Request.QueryString["verified"] == "1")
                    ShowMessage("Email verified successfully. You can now login.", "alert alert-success");

                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
                    RedirectToDashboard();
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (!Page.IsValid) return;

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            Response<User> auth = _userBLL.Authenticate(email, password);
            if (!auth.IsSuccess || auth.Data == null)
            {
                ShowMessage(auth.Message, "alert alert-danger");
                return;
            }

            if (!auth.Data.IsApproved || auth.Data.ApprovalStatus != "Approved")
            {
                ShowMessage("Your account is pending admin approval or has been rejected.", "alert alert-warning");
                return;
            }

            if (!auth.Data.IsActive)
            {
                ShowMessage("Your account has been deactivated. Please contact support.", "alert alert-danger");
                return;
            }

            if (string.IsNullOrEmpty(auth.Data.Email))
            {
                ShowMessage("Please verify your email address before logging in.", "alert alert-warning");
                return;
            }

            SessionManager.LoginUser(auth.Data, chkRememberMe.Checked);

            // Check for forced password change
            if (!string.IsNullOrEmpty(auth.Data.ResetTemporaryPassword) && auth.Data.ResetTemporaryPassword == password)
            {
                Response.Redirect(ResolveUrl("~/Auth/ChangePassword.aspx?force=1"));
                return;
            }

            RedirectToDashboard();
        }

        private void RedirectToDashboard()
        {
            string role = Session["Role"]?.ToString() ?? "";
            string redirectUrl = "~/Default.aspx";

            switch (role)
            {
                case "SystemAdministrator":
                    redirectUrl = "~/Admin/Dashboard.aspx";
                    break;
                case "Teacher":
                    redirectUrl = "~/Teacher/Dashboard.aspx";
                    break;
                case "Student":
                    redirectUrl = "~/Student/Dashboard.aspx";
                    break;
                case "Parent":
                    redirectUrl = "~/Parent/Dashboard.aspx";
                    break;
                case "Headmaster":
                    redirectUrl = "~/Headmaster/Dashboard.aspx";
                    break;
                case "AssistantHeadmaster":
                    redirectUrl = "~/AssistantHeadmaster/Dashboard.aspx";
                    break;
                case "AcademicCoordinator":
                    redirectUrl = "~/AcademicCoordinator/Dashboard.aspx";
                    break;
                case "HOD":
                    redirectUrl = "~/HOD/Dashboard.aspx";
                    break;
            }

            Response.Redirect(ResolveUrl(redirectUrl));
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}