<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="EduTrack.Contact" %>
<asp:Content ID="ContactContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; }
        .card-glass { background: rgba(255,255,255,0.95); backdrop-filter: blur(10px); border: 1px solid rgba(255,255,255,0.3); border-radius: 16px; box-shadow: 0 8px 32px rgba(0,0,0,0.08); transition: all 0.3s ease; }
        .card-glass:hover { transform: translateY(-5px); box-shadow: 0 16px 48px rgba(0,0,0,0.12); }
        .btn-gradient { background: var(--primary-gradient); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
        .form-control, .form-select { border-radius: 10px; border: 2px solid #e9ecef; padding: 0.75rem 1rem; transition: 0.25s; }
        .form-control:focus, .form-select:focus { border-color: #667eea; box-shadow: 0 0 0 0.2rem rgba(102,126,234,0.15); }
        .form-label { font-weight: 600; font-size: 0.9rem; margin-bottom: 0.45rem; }
        .contact-icon { width: 50px; height: 50px; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 1.5rem; background: var(--primary-gradient); color: white; flex-shrink: 0; }
        .social-btn { width: 48px; height: 48px; border-radius: 50%; display: inline-flex; align-items: center; justify-content: center; font-size: 1.3rem; transition: all 0.3s; color: white; text-decoration: none; }
        .social-btn:hover { transform: translateY(-3px) scale(1.05); }
        .toast-container { position: fixed; top: 20px; right: 20px; z-index: 9999; }
        .toast-message { padding: 1rem 1.5rem; border-radius: 10px; color: white; margin-bottom: 10px; box-shadow: 0 4px 16px rgba(0,0,0,0.15); animation: slideInRight 0.5s ease; display: flex; align-items: center; gap: 10px; }
        .toast-message.success { background: #28a745; }
        .toast-message.error { background: #dc3545; }
        @keyframes slideInRight { from { transform: translateX(100%); opacity: 0; } to { transform: translateX(0); opacity: 1; } }
        @keyframes slideOutRight { from { transform: translateX(0); opacity: 1; } to { transform: translateX(100%); opacity: 0; } }
        .ratio-16x9 { position: relative; width: 100%; padding-bottom: 56.25%; }
        .ratio-16x9 iframe { position: absolute; top: 0; left: 0; width: 100%; height: 100%; border-radius: 12px; border: none; }
    </style>

    <div class="container py-4">
        <div class="text-center mb-5">
            <h1 class="display-4 fw-bold text-primary"><i class="bi bi-envelope-fill me-2"></i>Contact Us</h1>
            <p class="lead text-muted">We'd love to hear from you!</p>
        </div>

        <div class="row g-4">
            <!-- Contact Form -->
            <div class="col-lg-6">
                <div class="card-glass p-4">
                    <h4 class="text-primary"><i class="bi bi-send me-2"></i>Send Us a Message</h4>
                    <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block" Visible="false" />

                    <div class="mb-3">
                        <label class="form-label"><i class="bi bi-person me-1"></i>Your Name <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Enter your name" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Contact" />
                        <asp:RegularExpressionValidator ID="revName" runat="server" ControlToValidate="txtName" ValidationExpression="^[A-Za-z\s'-]+$" ErrorMessage="Only letters and spaces allowed" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Contact" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label"><i class="bi bi-envelope me-1"></i>Email Address <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="you@example.com" MaxLength="100" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Contact" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" ErrorMessage="Invalid email format" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Contact" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label"><i class="bi bi-tag me-1"></i>Subject <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="What is this about?" MaxLength="200" />
                        <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="Subject is required" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Contact" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label"><i class="bi bi-chat-left-text me-1"></i>Message <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Your message here..." MaxLength="1000" />
                        <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage" ErrorMessage="Message is required" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Contact" />
                    </div>

                    <asp:Button ID="btnSend" runat="server" Text="Send Message" CssClass="btn btn-gradient w-100" OnClick="btnSend_Click" ValidationGroup="Contact" />
                </div>
            </div>

            <!-- Contact Information -->
            <div class="col-lg-6">
                <div class="card-glass p-4 h-100">
                    <h4 class="text-success"><i class="bi bi-info-circle me-2"></i>Get In Touch</h4>

                    <div class="d-flex gap-3 mb-4">
                        <div class="contact-icon"><i class="bi bi-geo-alt-fill"></i></div>
                        <div>
                            <h6 class="fw-bold mb-1">Address</h6>
                            <p class="text-muted mb-0">Near Kwame Danso Senior High Technical School<br />Learning City B/E Region, BA402 @Kalip<br />Ghana</p>
                        </div>
                    </div>

                    <div class="d-flex gap-3 mb-4">
                        <div class="contact-icon" style="background: linear-gradient(135deg, #0d6efd, #0a58ca);"><i class="bi bi-envelope-fill"></i></div>
                        <div>
                            <h6 class="fw-bold mb-1">Email</h6>
                            <p class="mb-0"><a href="mailto:nyarkoakwasi36@gmail.com" class="text-decoration-none">nyarkoakwasi36@gmail.com</a></p>
                            <p class="mb-0"><a href="mailto:nyarkoakwasi247@outlook.com" class="text-decoration-none">nyarkoakwasi247@outlook.com</a></p>
                        </div>
                    </div>

                    <div class="d-flex gap-3 mb-4">
                        <div class="contact-icon" style="background: linear-gradient(135deg, #28a745, #20c997);"><i class="bi bi-telephone-fill"></i></div>
                        <div>
                            <h6 class="fw-bold mb-1">Phone</h6>
                            <p class="mb-0 text-muted">+233 54 371 3237</p>
                            <p class="mb-0 text-muted">+233 50 358 9382</p>
                        </div>
                    </div>

                    <div class="d-flex gap-3 mb-4">
                        <div class="contact-icon" style="background: linear-gradient(135deg, #ffc107, #fd7e14);"><i class="bi bi-clock-fill"></i></div>
                        <div>
                            <h6 class="fw-bold mb-1">Business Hours</h6>
                            <p class="text-muted mb-0">Mon - Fri: 8:00 AM - 9:00 PM</p>
                            <p class="text-muted mb-0">Sat: 9:00 AM - 3:00 PM</p>
                            <p class="text-muted mb-0">Sun: 12:00 PM - 4:00 PM</p>
                        </div>
                    </div>

                    <div class="text-center pt-3 border-top">
                        <h6 class="fw-bold mb-3">Follow Us</h6>
                        <div class="d-flex justify-content-center gap-2">
                            <a href="https://www.facebook.com/timothyakwasinyarko" target="_blank" class="social-btn" style="background: #1877f2;"><i class="bi bi-facebook"></i></a>
                            <a href="https://twitter.com/@linguistic_37" target="_blank" class="social-btn" style="background: #000;"><i class="bi bi-twitter-x"></i></a>
                            <a href="https://www.linkedin.com/in/timothy-akwasi-nyarko-22a364357" target="_blank" class="social-btn" style="background: #0a66c2;"><i class="bi bi-linkedin"></i></a>
                            <a href="https://github.com/linguistic247" target="_blank" class="social-btn" style="background: #333;"><i class="bi bi-github"></i></a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Map -->
        <div class="mt-4">
            <div class="card-glass p-2">
                <div class="ratio-16x9">
                    <iframe src="https://maps.google.com/maps?q=Kwame%20Danso%20Ghana&t=&z=13&ie=UTF8&iwloc=&output=embed" allowfullscreen loading="lazy"></iframe>
                </div>
            </div>
        </div>
    </div>

    <script>
        function showToast(message, type) {
            var container = document.getElementById('toastContainer');
            if (!container) { container = document.createElement('div'); container.id = 'toastContainer'; container.className = 'toast-container'; document.body.appendChild(container); }
            var toast = document.createElement('div');
            toast.className = 'toast-message ' + type;
            toast.innerHTML = '<i class="bi bi-' + (type === 'success' ? 'check-circle' : 'error' ? 'x-circle' : 'info-circle') + '"></i> ' + message;
            container.appendChild(toast);
            setTimeout(function () { toast.style.animation = 'slideOutRight 0.5s ease'; setTimeout(function () { toast.remove(); }, 500); }, 4000);
        }
    </script>
</asp:Content>