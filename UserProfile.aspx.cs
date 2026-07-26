using EduTrack.BLL;
using EduTrack.Helpers;
using EduTrack.Models;
using System;
using System.Drawing;
using System.IO;

namespace EduTrack.Auth
{
    public partial class UserProfile : System.Web.UI.Page
    {
        private readonly UserBLL _userBLL = new UserBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure multipart form for file upload
            if (this.Form != null && string.IsNullOrWhiteSpace(this.Form.Enctype))
                this.Form.Enctype = "multipart/form-data";

            var user = SessionManager.GetCurrentUser();
            if (user == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadProfile(user);
            }
        }

        private void LoadProfile(User user)
        {
            txtFullName.Text = user.FullName;
            txtEmail.Text = user.Email;
            txtPhoneNumber.Text = user.PhoneNumber ?? "";
            txtDateOfBirth.Text = user.DateOfBirth?.ToString("yyyy-MM-dd") ?? "";
            ddlGender.SelectedValue = user.Gender ?? "";
            txtNationalID.Text = user.NationalID ?? "";
            txtEmergencyContact.Text = user.EmergencyContact ?? "";
            txtAddress.Text = user.Address ?? "";
            txtBio.Text = user.Bio ?? "";

            lblRole.Text = user.Role;

            // Set status badge
            string statusClass = user.ApprovalStatus?.ToLower() ?? "pending";
            spanStatus.Attributes["class"] = "status-badge status-" + statusClass;
            spanStatus.InnerHtml = user.ApprovalStatus ?? "Pending";

            // Set active badge
            string activeClass = user.IsActive ? "active" : "inactive";
            spanActive.Attributes["class"] = "status-badge status-" + activeClass;
            spanActive.InnerHtml = user.IsActive ? "Active" : "Inactive";

            lblJoinDate.Text = user.CreatedAt.ToString("d MMM yyyy");
            lblLastLogin.Text = user.LastLogin?.ToString("d MMM yyyy HH:mm") ?? "-";
            lblUpdatedOn.Text = user.UpdatedAt?.ToString("d MMM yyyy HH:mm") ?? "-";

            // Profile picture
            string pic = user.ProfilePicture ?? "";
            imgProfile.ImageUrl = (!string.IsNullOrWhiteSpace(pic) && File.Exists(Server.MapPath(pic)))
                ? ResolveUrl(pic)
                : ResolveUrl("~/Resources/default-avatar.png");
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ShowToast("Full name is required.", "error");
                return;
            }

            var user = SessionManager.GetCurrentUser();
            if (user == null)
            {
                Response.Redirect("~/Auth/Login.aspx");
                return;
            }

            // Check if email is already used by another user
            var existing = _userBLL.GetUserByEmail(email);
            if (existing.IsSuccess && existing.Data != null && existing.Data.UserID != user.UserID)
            {
                ShowToast("Email already in use by another account.", "error");
                return;
            }

            // Handle profile picture upload
            string newPicPath = null;
            if (fuProfile.HasFile)
            {
                var file = fuProfile.PostedFile;
                string ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                string[] allowed = { ".jpg", ".jpeg", ".png", ".gif" };

                if (Array.IndexOf(allowed, ext) < 0)
                {
                    ShowToast("Only JPG, PNG, GIF images allowed.", "error");
                    return;
                }

                if (file.ContentLength > 2 * 1024 * 1024)
                {
                    ShowToast("Image must be less than 2MB.", "error");
                    return;
                }

                if (!file.ContentType.StartsWith("image/"))
                {
                    ShowToast("Uploaded file is not a valid image.", "error");
                    return;
                }

                // Validate image is readable
                try
                {
                    using (Image.FromStream(file.InputStream, true, true)) { }
                }
                catch
                {
                    ShowToast("Uploaded file is not a valid image.", "error");
                    return;
                }
                file.InputStream.Position = 0;

                string saveDir = "~/Resources/ProfilePics/";
                string physicalDir = Server.MapPath(saveDir);
                if (!Directory.Exists(physicalDir))
                    Directory.CreateDirectory(physicalDir);

                string fileName = $"user_{user.UserID}_{Guid.NewGuid():N}{ext}";
                string virtualPath = saveDir + fileName;
                string physicalPath = Path.Combine(physicalDir, fileName);
                file.SaveAs(physicalPath);

                if (!File.Exists(physicalPath))
                {
                    ShowToast("Failed to save uploaded image.", "error");
                    return;
                }

                newPicPath = virtualPath;

                // Delete old picture if not default
                if (!string.IsNullOrWhiteSpace(user.ProfilePicture) &&
                    !user.ProfilePicture.Equals("~/Resources/default-avatar.png", StringComparison.OrdinalIgnoreCase))
                {
                    try { File.Delete(Server.MapPath(user.ProfilePicture)); } catch { }
                }
            }

            // Update user object
            user.FullName = fullName;
            user.Email = email;
            user.PhoneNumber = txtPhoneNumber.Text.Trim();
            user.DateOfBirth = string.IsNullOrEmpty(txtDateOfBirth.Text) ? (DateTime?)null : DateTime.Parse(txtDateOfBirth.Text);
            user.Gender = ddlGender.SelectedValue;
            user.NationalID = txtNationalID.Text.Trim();
            user.EmergencyContact = txtEmergencyContact.Text.Trim();
            user.Address = txtAddress.Text.Trim();
            user.Bio = txtBio.Text.Trim();
            if (newPicPath != null)
                user.ProfilePicture = newPicPath;

            var result = _userBLL.UpdateUser(user);
            if (!result.IsSuccess)
            {
                // Rollback uploaded file if update failed
                if (newPicPath != null)
                {
                    try { File.Delete(Server.MapPath(newPicPath)); } catch { }
                }
                ShowToast(result.Message, "error");
                return;
            }

            // Update session data
            Session["FullName"] = user.FullName;
            if (newPicPath != null)
                Session["ProfilePicture"] = newPicPath;

            ShowToast("Profile updated successfully!", "success");
            LoadProfile(user);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var user = SessionManager.GetCurrentUser();
            if (user != null)
                LoadProfile(user);
            else
                Response.Redirect("~/Auth/Login.aspx");
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Auth/ChangePassword.aspx");
        }

        private void ShowToast(string message, string type)
        {
            string safe = message.Replace("'", "\\'");
            ClientScript.RegisterStartupScript(GetType(), "toast", $"showToast('{safe}','{type}');", true);
        }
    }
}