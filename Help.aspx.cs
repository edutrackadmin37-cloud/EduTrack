using System;
using System.Data.SqlClient;
using System.Configuration;

namespace EduTrack
{
    public partial class Help : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSupportContact();
            }
        }

        private void LoadSupportContact()
        {
            string email = "nyarkoakwasi36@gmail.com";

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["EduTrackDb"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 SettingValue FROM SystemSettings WHERE SettingName = @SettingName", conn))
                    {
                        cmd.Parameters.AddWithValue("@SettingName", "SupportEmail");
                        object obj = cmd.ExecuteScalar();
                        if (obj != null && !string.IsNullOrWhiteSpace(obj.ToString()))
                            email = obj.ToString().Trim();
                    }
                }
            }
            catch { /* Fallback to default */ }

            hlSupportEmail.Text = email;
            hlSupportEmail.NavigateUrl = "mailto:" + email;
            hlContactUs.NavigateUrl = ResolveUrl("~/Contact.aspx");
        }
    }
}