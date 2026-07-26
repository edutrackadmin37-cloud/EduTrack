using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace EduTrack
{
    public partial class Contact : System.Web.UI.Page
    {
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string subject = txtSubject.Text.Trim();
            string message = txtMessage.Text.Trim();

            if (!Regex.IsMatch(name, @"^[A-Za-z\s'-]+$"))
            {
                ShowToast("Name must contain only letters and spaces.", "error");
                return;
            }

            try
            {
                var parsed = new MailAddress(email);
                if (!string.Equals(parsed.Address, email, StringComparison.OrdinalIgnoreCase))
                {
                    ShowToast("Please enter a valid email address.", "error");
                    return;
                }
            }
            catch
            {
                ShowToast("Please enter a valid email address.", "error");
                return;
            }

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(message))
            {
                ShowToast("Subject and message are required.", "error");
                return;
            }

            ShowToast("Thank you! Your message has been sent successfully.", "success");
            txtName.Text = "";
            txtEmail.Text = "";
            txtSubject.Text = "";
            txtMessage.Text = "";
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}