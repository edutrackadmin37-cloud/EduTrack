using System;

namespace EduTrack
{
    public partial class Documentation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblLastUpdated.Text = DateTime.Now.ToString("MMMM yyyy");
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Client-side search is handled via JavaScript.
        }
    }
}