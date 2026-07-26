using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Departments : System.Web.UI.Page
    {
        private readonly DepartmentBLL _deptBLL = new DepartmentBLL();
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
                LoadHeads();
                LoadGrid();
            }
        }

        private void LoadHeads()
        {
            var users = _userBLL.GetAllUsers();
            ddlHead.DataSource = users.IsSuccess ? users.Data.FindAll(u => u.Role == "Teacher") : null;
            ddlHead.DataBind();
            ddlHead.Items.Insert(0, new ListItem("-- None --", "0"));
        }

        private void LoadGrid()
        {
            var data = _deptBLL.GetAll();
            gvDepartments.DataSource = data.IsSuccess ? data.Data : null;
            gvDepartments.DataBind();
        }

        protected void gvDepartments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _deptBLL.SoftDelete(id);
                ShowToast(result.IsSuccess ? "Department deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = string.IsNullOrEmpty(hfDeptID.Value) ? 0 : int.Parse(hfDeptID.Value);

            Department dept = new Department
            {
                DepartmentID = id,
                DepartmentName = txtDeptName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                HeadOfDepartmentID = int.Parse(ddlHead.SelectedValue) > 0 ? int.Parse(ddlHead.SelectedValue) : (int?)null
            };

            Response<int> result;
            if (id == 0)
            {
                result = _deptBLL.Create(dept);
            }
            else
            {
                var updateResult = _deptBLL.Update(dept);
                result = updateResult.IsSuccess ? Response<int>.Success(id, "Department updated.") : Response<int>.Failure(updateResult.Message, "UPDATE_FAILED");
            }

            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadGrid();
                ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#deptModal').modal('hide');", true);
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