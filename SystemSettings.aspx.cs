using EduTrack.BLL;
using EduTrack.Models;
using System;

namespace EduTrack.Admin
{
    public partial class SystemSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Role"]?.ToString() != "SystemAdministrator")
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadSettings();
            }
        }

        private void LoadSettings()
        {
            var settings = SettingsBLL.GetSettings();
            if (settings != null)
            {
                txtSiteName.Text = settings.SiteName;
                txtInstitutionName.Text = settings.InstitutionName;
                txtContactEmail.Text = settings.ContactEmail;
                txtPlatformURL.Text = settings.PlatformURL;
                txtSchoolYear.Text = settings.SchoolYear;
                ddlGradingScale.SelectedValue = settings.GradingScale ?? "percent";
                chkManualApproval.Checked = settings.ManualApproval;
                chkEnableChat.Checked = settings.EnableChat;
                chkPeerAssessment.Checked = settings.EnablePeerAssessment;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var settings = new SettingsDTO
            {
                SiteName = txtSiteName.Text.Trim(),
                InstitutionName = txtInstitutionName.Text.Trim(),
                ContactEmail = txtContactEmail.Text.Trim(),
                PlatformURL = txtPlatformURL.Text.Trim(),
                SchoolYear = txtSchoolYear.Text.Trim(),
                GradingScale = ddlGradingScale.SelectedValue,
                ManualApproval = chkManualApproval.Checked,
                EnableChat = chkEnableChat.Checked,
                EnablePeerAssessment = chkPeerAssessment.Checked
            };

            try
            {
                SettingsBLL.SaveSettings(settings);
                ShowToast("Settings saved successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowToast("Error saving settings: " + ex.Message, "error");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // Reset to defaults
            txtSiteName.Text = "EduTrack";
            txtInstitutionName.Text = "EduTrack Academy";
            txtContactEmail.Text = "edutrackadmin37@gmail.com";
            txtPlatformURL.Text = "http://localhost/EduTrack";
            txtSchoolYear.Text = "2025/2026";
            ddlGradingScale.SelectedValue = "percent";
            chkManualApproval.Checked = true;
            chkEnableChat.Checked = true;
            chkPeerAssessment.Checked = true;

            ShowToast("Settings reset to defaults.", "info");
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}