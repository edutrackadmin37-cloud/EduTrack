using EduTrack.BLL;
using EduTrack.DAL;
using EduTrack.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace EduTrack.Admin
{
    public partial class ClassSubjectTeacher : System.Web.UI.Page
    {
        private readonly ClassBLL _classBLL = new ClassBLL();
        private readonly SubjectBLL _subjectBLL = new SubjectBLL();
        private readonly UserBLL _userBLL = new UserBLL();
        private readonly ClassSubjectTeacherBLL _cstBLL = new ClassSubjectTeacherBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadClasses();
                LoadSubjects();
                LoadTeachers();
                LoadAssignments();
            }
        }

        private void LoadClasses()
        {
            var classes = _classBLL.GetClasses();
            ddlClass.DataSource = classes.IsSuccess ? classes.Data : null;
            ddlClass.DataBind();
            ddlClass.Items.Insert(0, new ListItem("-- Select Class --", "0"));
        }

        private void LoadSubjects()
        {
            var subjects = _subjectBLL.GetAllSubjects();
            ddlSubject.DataSource = subjects.IsSuccess ? subjects.Data : null;
            ddlSubject.DataBind();
            ddlSubject.Items.Insert(0, new ListItem("-- Select Subject --", "0"));
        }

        private void LoadTeachers()
        {
            var teachers = _userBLL.GetAllUsers();
            if (teachers.IsSuccess && teachers.Data != null)
            {
                var teacherList = teachers.Data.Where(u => u.Role == "Teacher").ToList();
                ddlTeacher.DataSource = teacherList;
            }
            else
            {
                ddlTeacher.DataSource = null;
            }
            ddlTeacher.DataBind();
            ddlTeacher.Items.Insert(0, new ListItem("-- Select Teacher --", "0"));
        }

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAssignments();
        }

        private void LoadAssignments()
        {
            int classId = int.TryParse(ddlClass.SelectedValue, out int c) ? c : 0;
            if (classId == 0)
            {
                gvAssignments.DataSource = null;
                gvAssignments.DataBind();
                return;
            }

            var assignments = _cstBLL.GetByClass(classId);
            gvAssignments.DataSource = assignments.IsSuccess ? assignments.Data : null;
            gvAssignments.DataBind();
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            int classId = int.Parse(ddlClass.SelectedValue);
            int subjectId = int.Parse(ddlSubject.SelectedValue);
            int teacherId = int.Parse(ddlTeacher.SelectedValue);

            if (classId == 0 || subjectId == 0 || teacherId == 0)
            {
                ShowToast("Please select class, subject, and teacher.", "warning");
                return;
            }

            var result = _cstBLL.AssignTeacher(classId, subjectId, teacherId);
            if (result.IsSuccess)
            {
                ShowToast(result.Message, "success");
                LoadAssignments();
            }
            else
            {
                ShowToast(result.Message, "error");
            }
        }

        protected void gvAssignments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                var result = _cstBLL.RemoveAssignment(id);
                ShowToast(result.IsSuccess ? "Assignment removed." : result.Message, result.IsSuccess ? "success" : "error");
                LoadAssignments();
            }
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}