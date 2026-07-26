using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Auth
{
    public partial class Register : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // If user is already authenticated, redirect to dashboard
                if (SessionManager.GetCurrentUser() != null)
                {
                    Response.Redirect(ResolveUrl("~/Default.aspx"));
                    return;
                }

                ddlRole.SelectedValue = "Student";
            }
        }

        protected void cvTerms_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = chkTerms.Checked;
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
            if (!Page.IsValid) return;

            string password = txtPassword.Text.Trim();
            if (!ValidationHelper.IsValidPassword(password))
            {
                ShowMessage("Password does not meet requirements.", "alert alert-danger");
                return;
            }

            string email = txtEmail.Text.Trim();
            var exists = _userBLL.GetUserByEmail(email);
            if (exists.IsSuccess && exists.Data != null)
            {
                ShowMessage("A user with this email already exists.", "alert alert-warning");
                return;
            }

            var user = new User
            {
                FullName = txtFullName.Text.Trim(),
                Email = email,
                PhoneNumber = txtPhone.Text.Trim(),
                Role = ddlRole.SelectedValue,
                IsActive = true,
                IsApproved = false,
                ApprovalStatus = "Pending"
            };

            var createResp = _userBLL.Register(user, password);
            if (!createResp.IsSuccess)
            {
                ShowMessage(createResp.Message, "alert alert-danger");
                return;
            }

            ShowMessage("Registration successful! Please check your email to verify your account. You will be notified when an admin approves your account.", "alert alert-success");

            // Disable form controls
            btnRegister.Enabled = false;
            txtFullName.Enabled = false;
            txtEmail.Enabled = false;
            txtPhone.Enabled = false;
            txtPassword.Enabled = false;
            txtConfirmPassword.Enabled = false;
            ddlRole.Enabled = false;
            chkTerms.Enabled = false;

            // Redirect to login after 3 seconds
            ClientScript.RegisterStartupScript(GetType(), "redirect",
                $"setTimeout(function(){{window.location='{ResolveUrl("~/Auth/Login.aspx?registered=1")}'}}, 3000);", true);
        }

        private void ShowMessage(string message, string cssClass)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = cssClass;
            lblMessage.Visible = true;
        }
    }
}