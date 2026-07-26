using EduTrack.BLL;
using EduTrack.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace EduTrack.Parent
{
    public partial class Reports : System.Web.UI.Page
    {
        private readonly ParentBLL _parentBLL = new ParentBLL();
        private readonly ReportBLL _reportBLL = new ReportBLL();
        private int ParentId => ((User)Session["User"])?.UserID ?? 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "Parent")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack) LoadChildren();
        }

        private void LoadChildren()
        {
            var children = _parentBLL.GetChildren(ParentId);
            ddlChild.DataSource = children.IsSuccess ? children.Data : null;
            ddlChild.DataBind();
            ddlChild.Items.Insert(0, new ListItem("-- Select Child --", "0"));
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            int childId = int.Parse(ddlChild.SelectedValue);
            if (childId == 0)
            {
                ShowToast("Please select a child.", "warning");
                return;
            }

            string reportType = ddlReportType.SelectedValue;
            divReportContent.Visible = true;

            var reportData = _parentBLL.GenerateReport(childId, reportType);
            if (reportData.IsSuccess && reportData.Data != null)
            {
                reportContent.InnerHtml = reportData.Data;
                ShowToast("Report generated successfully.", "success");
            }
            else
            {
                reportContent.InnerHtml = "<div class='empty-state'><i class='bi bi-file-text'></i><p>No data available for this report.</p></div>";
                ShowToast(reportData.Message, "warning");
            }
        }

        protected void btnExportPDF_Click(object sender, EventArgs e) => ExportReport("pdf");
        protected void btnExportExcel_Click(object sender, EventArgs e) => ExportReport("excel");

        private void ExportReport(string format)
        {
            int childId = int.Parse(ddlChild.SelectedValue);
            if (childId == 0)
            {
                ShowToast("Please select a child.", "warning");
                return;
            }

            string reportType = ddlReportType.SelectedValue;
            var reportData = _parentBLL.GenerateReport(childId, reportType);
            if (!reportData.IsSuccess || reportData.Data == null)
            {
                ShowToast("No data to export.", "warning");
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(reportData.Data);
            string contentType = format == "pdf" ? "application/pdf" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string ext = format == "pdf" ? "pdf" : "xlsx";

            Response.Clear();
            Response.AddHeader("content-disposition", $"attachment;filename=Report_{DateTime.Now:yyyyMMdd}.{ext}");
            Response.ContentType = contentType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}