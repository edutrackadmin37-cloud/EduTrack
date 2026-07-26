using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Subjects : System.Web.UI.Page
    {
        private readonly SubjectBLL _subjectBLL = new SubjectBLL();

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
            var data = _subjectBLL.GetAllSubjects();
            gvSubjects.DataSource = data.IsSuccess ? data.Data : null;
            gvSubjects.DataBind();
        }

        protected void gvSubjects_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _subjectBLL.SoftDeleteSubject(id);
                ShowToast(result.IsSuccess ? "Subject deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = string.IsNullOrEmpty(hfSubjectID.Value) ? 0 : int.Parse(hfSubjectID.Value);

            Subject subject = new Subject
            {
                SubjectID = id,
                SubjectName = txtSubjectName.Text.Trim(),
                SubjectCode = txtSubjectCode.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                IsCore = chkIsCore.Checked
            };

            Response<int> result;
            if (id == 0)
            {
                result = _subjectBLL.CreateSubject(subject);
            }
            else
            {
                var updateResult = _subjectBLL.UpdateSubject(subject);
                result = updateResult.IsSuccess ? Response<int>.Success(id, "Subject updated.") : Response<int>.Failure(updateResult.Message, "UPDATE_FAILED");
            }

            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadGrid();
                ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#subjectModal').modal('hide');", true);
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