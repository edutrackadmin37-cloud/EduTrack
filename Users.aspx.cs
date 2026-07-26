using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();
        private int AdminId => ((User)Session["User"])?.UserID ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack) LoadUsers();
        }

        private void LoadUsers()
        {
            var resp = _userBLL.GetAllUsers();
            gvUsers.DataSource = resp.IsSuccess ? resp.Data : null;
            gvUsers.DataBind();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int userId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "Approve")
            {
                var result = _userBLL.ApproveUser(userId, AdminId);
                ShowToast(result.IsSuccess ? "User approved." : result.Message, result.IsSuccess ? "success" : "error");
            }
            else if (e.CommandName == "Reject")
            {
                var result = _userBLL.RejectUser(userId);
                ShowToast(result.IsSuccess ? "User rejected." : result.Message, result.IsSuccess ? "success" : "error");
            }
            else if (e.CommandName == "Delete")
            {
                var result = _userBLL.SoftDeleteUser(userId);
                ShowToast(result.IsSuccess ? "User deleted." : result.Message, result.IsSuccess ? "success" : "error");
            }

            LoadUsers();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = int.TryParse(hfUserId.Value, out int parsed) ? parsed : 0;

            User user = new User
            {
                UserID = id,
                FullName = txtFullName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Role = ddlRole.SelectedValue,
                IsActive = chkIsActive.Checked
            };

            if (id == 0)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    ShowToast("Password required for new user.", "error");
                    return;
                }
                if (!ValidationHelper.IsValidPassword(txtPassword.Text))
                {
                    ShowToast("Password does not meet complexity.", "error");
                    return;
                }
                user.PasswordHash = PasswordHelper.HashPassword(txtPassword.Text);
                user.IsApproved = true;
                user.ApprovalStatus = "Approved";

                var result = _userBLL.Register(user, txtPassword.Text);
                ShowToast(result.IsSuccess ? "User created." : result.Message, result.IsSuccess ? "success" : "error");
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    if (!ValidationHelper.IsValidPassword(txtPassword.Text))
                    {
                        ShowToast("Password does not meet complexity.", "error");
                        return;
                    }
                    user.PasswordHash = PasswordHelper.HashPassword(txtPassword.Text);
                }

                var result = _userBLL.UpdateUser(user);
                ShowToast(result.IsSuccess ? "User updated." : result.Message, result.IsSuccess ? "success" : "error");
            }

            LoadUsers();
            ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#userModal').modal('hide');", true);
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}