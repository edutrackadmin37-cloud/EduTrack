using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class AcademicYears : System.Web.UI.Page
    {
        private readonly AcademicYearBLL _ayBLL = new AcademicYearBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack) LoadGrid();
        }

        private void LoadGrid()
        {
            var data = _ayBLL.GetAll();
            gvAcademicYears.DataSource = data.IsSuccess ? data.Data : null;
            gvAcademicYears.DataBind();
        }

        protected void gvAcademicYears_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _ayBLL.SoftDelete(id);
                ShowToast(result.IsSuccess ? "Academic year deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = string.IsNullOrEmpty(hfAYID.Value) ? 0 : int.Parse(hfAYID.Value);

            AcademicYear ay = new AcademicYear
            {
                AcademicYearID = id,
                YearName = txtYearName.Text.Trim(),
                StartDate = DateTime.Parse(txtStartDate.Text),
                EndDate = DateTime.Parse(txtEndDate.Text),
                IsCurrent = chkIsCurrent.Checked
            };

            Response<int> result;
            if (id == 0)
            {
                result = _ayBLL.Create(ay);
            }
            else
            {
                var updateResult = _ayBLL.Update(ay);
                result = updateResult.IsSuccess ? Response<int>.Success(id, "Academic year updated.") : Response<int>.Failure(updateResult.Message, "UPDATE_FAILED");
            }

            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadGrid();
                ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#ayModal').modal('hide');", true);
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