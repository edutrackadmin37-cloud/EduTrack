using EduTrack.BLL;
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class Classes : System.Web.UI.Page
    {
        private readonly ClassBLL _classBLL = new ClassBLL();
        private readonly AcademicYearBLL _ayBLL = new AcademicYearBLL();
        private readonly ProgrammeBLL _pBLL = new ProgrammeBLL();
        private readonly StreamBLL _sBLL = new StreamBLL();
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator") Response.Redirect("~/Auth/Login.aspx");
            if (!IsPostBack) { LoadDropdowns(); LoadGrid(); }
        }

        private void LoadDropdowns()
        {
            ddlAcademicYear.DataSource = _ayBLL.GetAll().Data;
            ddlAcademicYear.DataBind();
            ddlProgramme.DataSource = _pBLL.GetAll().Data;
            ddlProgramme.DataBind();
            ddlStream.DataSource = _sBLL.GetAll().Data;
            ddlStream.DataBind();
            var teachers = _userBLL.GetAllUsers().Data.Where(u => u.Role == "Teacher" && u.IsActive && u.IsApproved).ToList();
            ddlTeacher.DataSource = teachers;
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, new ListItem("-- None --", "0"));
        }

        private void LoadGrid()
        {
            var data = _classBLL.GetClasses().Data;
            gvClasses.DataSource = data;
            gvClasses.DataBind();
        }

        protected void gvClasses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _classBLL.SoftDeleteClass(id);
                ShowToast(result.IsSuccess ? "Deleted." : result.Message, result.IsSuccess ? "success" : "error");
                LoadGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int id = int.Parse(hfClassID.Value);
            Class cls = new Class
            {
                ClassID = id,
                ClassName = txtClassName.Text.Trim(),
                AcademicYearID = int.Parse(ddlAcademicYear.SelectedValue),
                ProgrammeID = int.Parse(ddlProgramme.SelectedValue),
                StreamID = int.Parse(ddlStream.SelectedValue),
                ClassTeacherID = int.Parse(ddlTeacher.SelectedValue) > 0 ? int.Parse(ddlTeacher.SelectedValue) : (int?)null
            };
            Response<bool> result;
            if (id == 0) result = Response<bool>.Success(_classBLL.CreateClass(cls).Data > 0, "Class created.");
            else result = _classBLL.UpdateClass(cls);
            ShowToast(result.IsSuccess ? "Saved." : result.Message, result.IsSuccess ? "success" : "error");
            LoadGrid();
            ClientScript.RegisterStartupScript(GetType(), "closeModal", "$('#classModal').modal('hide');", true);
        }

        private void ShowToast(string message, string type)
        {
            // Registers a client-side script to show a toast notification
            ClientScript.RegisterStartupScript(
                GetType(),
                "showToast",
                $"showToast('{message.Replace("'", "\\'")}', '{type}');",
                true
            );
        }
    }
}