using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Staff : System.Web.UI.Page
    {
        private readonly StaffBLL _staffBLL = new StaffBLL();
        private readonly UserBLL _userBLL = new UserBLL();
        private readonly DepartmentBLL _deptBLL = new DepartmentBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUsers();
                LoadDepartments();
                LoadGrid();
            }
        }

        private void LoadUsers()
        {
            var users = _userBLL.GetAllUsers();
            ddlUser.DataSource = users.IsSuccess ? users.Data : null;
            ddlUser.DataBind();
        }

        private void LoadDepartments()
        {
            var depts = _deptBLL.GetAll();
            ddlDepartment.DataSource = depts.IsSuccess ? depts.Data : null;
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new ListItem("-- None --", "0"));
        }

        private void LoadGrid()
        {
            var data = _staffBLL.GetAll();
            gvStaff.DataSource = data.IsSuccess ? data.Data : null;
            gvStaff.DataBind();
        }

        protected void gvStaff_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _staffBLL.SoftDelete(id);
                ShowToast(result.IsSuccess ? "Staff record deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = string.IsNullOrEmpty(hfStaffID.Value) ? 0 : int.Parse(hfStaffID.Value);

            EduTrack.Models.Staff staff = new EduTrack.Models.Staff
            {
                StaffID = id,
                UserID = int.Parse(ddlUser.SelectedValue),
                StaffNumber = txtStaffNumber.Text.Trim(),
                Position = txtPosition.Text.Trim(),
                DepartmentID = int.Parse(ddlDepartment.SelectedValue) > 0 ? int.Parse(ddlDepartment.SelectedValue) : (int?)null,
                HireDate = string.IsNullOrEmpty(txtHireDate.Text) ? (DateTime?)null : DateTime.Parse(txtHireDate.Text),
                IsActive = chkIsActive.Checked
            };

            Response<int> result;
            if (id == 0)
            {
                result = _staffBLL.Create(staff);
            }
            else
            {
                var updateResult = _staffBLL.Update(staff);
                result = updateResult.IsSuccess ? Response<int>.Success(id, "Staff record updated.") : Response<int>.Failure(updateResult.Message, "UPDATE_FAILED");
            }

            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadGrid();
                ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#staffModal').modal('hide');", true);
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