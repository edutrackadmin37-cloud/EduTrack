using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Schools : System.Web.UI.Page
    {
        private readonly SchoolBLL _schoolBLL = new SchoolBLL();
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadHeadmasters();
                LoadGrid();
            }
        }

        private void LoadHeadmasters()
        {
            // Assuming GetAllUsers() returns all users and you need to filter by role "Headmaster"
            var usersResponse = _userBLL.GetAllUsers();
            var users = usersResponse.IsSuccess
                ? usersResponse.Data.FindAll(u => u.Role == "Headmaster")
                : null;
            ddlHeadmaster.DataSource = users;
            ddlHeadmaster.DataBind();
            ddlHeadmaster.Items.Insert(0, new ListItem("-- None --", "0"));
        }

        private void LoadGrid()
        {
            var data = _schoolBLL.GetAll();
            gvSchools.DataSource = data.IsSuccess ? data.Data : null;
            gvSchools.DataBind();
        }

        protected void gvSchools_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _schoolBLL.SoftDelete(id);
                ShowToast(result.IsSuccess ? "School deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = string.IsNullOrEmpty(hfSchoolID.Value) ? 0 : int.Parse(hfSchoolID.Value);

            School school = new School
            {
                SchoolID = id,
                SchoolName = txtSchoolName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                PhoneNumber = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Website = txtWebsite.Text.Trim(),
                HeadmasterID = int.Parse(ddlHeadmaster.SelectedValue) > 0 ? int.Parse(ddlHeadmaster.SelectedValue) : (int?)null
            };

            Response<int> result;
            if (id == 0)
            {
                result = _schoolBLL.Create(school);
            }
            else
            {
                var updateResult = _schoolBLL.Update(school);
                result = updateResult.IsSuccess ? Response<int>.Success(id, "School updated.") : Response<int>.Failure(updateResult.Message, "UPDATE_FAILED");
            }

            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadGrid();
                ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#schoolModal').modal('hide');", true);
            }
            else
            {
                ShowToast(result.Message, "error");
            }
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}