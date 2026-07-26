using System;

namespace EduTrack
{
    public partial class FAQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hlContact.NavigateUrl = ResolveUrl("~/Contact.aspx");
                hlHelp.NavigateUrl = ResolveUrl("~/Help.aspx");
                hlContactSupport.NavigateUrl = ResolveUrl("~/Contact.aspx");
            }
        }
    }
}