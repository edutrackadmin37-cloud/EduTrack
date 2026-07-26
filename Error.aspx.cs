using System;

namespace EduTrack
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string errorId = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(errorId))
                    lblErrorId.Text = errorId;
            }
        }
    }
}