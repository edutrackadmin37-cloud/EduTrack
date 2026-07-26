using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Programmes : System.Web.UI.Page
    {
        private readonly ProgrammeBLL _programmeBLL = new ProgrammeBLL();
        private readonly DepartmentBLL _departmentBLL = new DepartmentBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDepartments();
                LoadGrid();
            }
        }

        private void LoadDepartments()
        {
            var depts = _departmentBLL.GetAll();
            ddlDepartment.DataSource = depts.IsSuccess ? depts.Data : null;
            ddlDepartment.DataBind();
        }

        private void LoadGrid()
        {
            var data = _programmeBLL.GetAll();
            gvProgrammes.DataSource = data.IsSuccess ? data.Data : null;
            gvProgrammes.DataBind();
        }

        protected void gvProgrammes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _programmeBLL.SoftDelete(id);
                ShowToast(result.IsSuccess ? "Programme deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = string.IsNullOrEmpty(hfProgrammeID.Value) ? 0 : int.Parse(hfProgrammeID.Value);

            Programme programme = new Programme
            {
                ProgrammeID = id,
                ProgrammeName = txtProgrammeName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                DepartmentID = int.Parse(ddlDepartment.SelectedValue)
            };

            Response<int> result;
            if (id == 0)
            {
                result = _programmeBLL.Create(programme);
            }
            else
            {
                var updateResult = _programmeBLL.Update(programme);
                result = updateResult.IsSuccess ? Response<int>.Success(id, "Programme updated.") : Response<int>.Failure(updateResult.Message, "UPDATE_FAILED");
            }

            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadGrid();
                ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#programmeModal').modal('hide');", true);
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