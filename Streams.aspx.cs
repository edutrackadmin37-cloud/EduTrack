using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Streams : System.Web.UI.Page
    {
        private readonly StreamBLL _streamBLL = new StreamBLL();

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
            var data = _streamBLL.GetAll();
            gvStreams.DataSource = data.IsSuccess ? data.Data : null;
            gvStreams.DataBind();
        }

        protected void gvStreams_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _streamBLL.SoftDelete(id);
                ShowToast(result.IsSuccess ? "Stream deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = string.IsNullOrEmpty(hfStreamID.Value) ? 0 : int.Parse(hfStreamID.Value);

            Stream stream = new Stream
            {
                StreamID = id,
                StreamName = txtStreamName.Text.Trim(),
                Description = txtDescription.Text.Trim()
            };

            Response<int> result;
            if (id == 0)
            {
                result = _streamBLL.Create(stream);
            }
            else
            {
                var updateResult = _streamBLL.Update(stream);
                result = updateResult.IsSuccess ? Response<int>.Success(id, "Stream updated.") : Response<int>.Failure(updateResult.Message, "UPDATE_FAILED");
            }

            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadGrid();
                ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#streamModal').modal('hide');", true);
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