<%@ Page Title="My Profile - EduTrack" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="EduTrack.Auth.UserProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); min-height: 100vh; }
        .container { padding-top: 40px; padding-bottom: 40px; }
        .card { border: none; border-radius: 22px; background: #fff; box-shadow: 0 20px 65px rgba(0,0,0,0.35); padding: 2rem; }
        .profile-pic { width: 150px; height: 150px; object-fit: cover; border-radius: 50%; border: 4px solid #667eea; }
        .form-control, .form-select { border-radius: 12px; border: 2px solid #e9ecef; padding: 0.75rem 1rem; }
        .form-control:focus, .form-select:focus { border-color: #667eea; box-shadow: 0 0 0 0.25rem rgba(102,126,234,0.15); }
        .btn-gradient { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; border: none; border-radius: 12px; padding: 0.6rem 1.5rem; font-weight: 600; }
        .btn-gradient:hover { transform: translateY(-2px); box-shadow: 0 10px 25px rgba(102,126,234,0.6); color: white; }
        .btn-outline-gradient { background: transparent; color: #667eea; border: 2px solid #667eea; border-radius: 12px; padding: 0.6rem 1.5rem; font-weight: 600; }
        .btn-outline-gradient:hover { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; border-color: transparent; }
        .info-box { background: #f8f9fa; padding: 0.75rem 1rem; border-radius: 12px; margin-bottom: 0.5rem; }
        .info-box .label { font-size: 0.8rem; color: #6c757d; }
        .info-box .value { font-weight: 600; }
        .status-badge { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.8rem; font-weight: 600; display: inline-block; margin-right: 4px; }
        .status-approved { background: #28a745; color: white; }
        .status-pending { background: #ffc107; color: #856404; }
        .status-rejected { background: #dc3545; color: white; }
        .status-active { background: #28a745; color: white; }
        .status-inactive { background: #dc3545; color: white; }
        .alert { border-radius: 12px; border: none; }
        .toast-container { position: fixed; top: 20px; right: 20px; z-index: 9999; }
        .toast-message { padding: 1rem 1.5rem; border-radius: 10px; color: white; margin-bottom: 10px; box-shadow: 0 4px 16px rgba(0,0,0,0.15); animation: slideInRight 0.5s ease; display: flex; align-items: center; gap: 10px; }
        .toast-message.success { background: #28a745; }
        .toast-message.error { background: #dc3545; }
        .toast-message.warning { background: #ffc107; color: #333; }
        @keyframes slideInRight { from { transform: translateX(100%); opacity: 0; } to { transform: translateX(0); opacity: 1; } }
        @keyframes slideOutRight { from { transform: translateX(0); opacity: 1; } to { transform: translateX(100%); opacity: 0; } }
    </style>

    <div class="container">
        <h2 class="text-white mb-4"><i class="bi bi-person-circle me-2"></i>My Profile</h2>

        <div class="card">
            <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block" Visible="false" />

            <div class="row">
                <!-- Left Column -->
                <div class="col-md-4 text-center">
                    <div class="mb-3">
                        <asp:Image ID="imgProfile" runat="server" CssClass="profile-pic" AlternateText="Profile Picture" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-semibold"><i class="bi bi-camera me-1"></i>Change Picture</label>
                        <asp:FileUpload ID="fuProfile" runat="server" CssClass="form-control" accept="image/*" />
                        <small class="text-muted">JPG, PNG, GIF (Max 2MB)</small>
                    </div>

                    <div class="card bg-light mt-3 p-3">
                        <h6 class="text-primary"><i class="bi bi-shield-check me-1"></i>Account Info</h6>
                        <div class="info-box">
                            <div class="label">Role</div>
                            <div class="value"><asp:Label ID="lblRole" runat="server" /></div>
                        </div>
                        <div class="info-box">
                            <div class="label">Status</div>
                            <div class="value">
                                <span id="spanStatus" runat="server" class="status-badge"></span>
                                <span id="spanActive" runat="server" class="status-badge"></span>
                            </div>
                        </div>
                        <div class="info-box">
                            <div class="label">Registered</div>
                            <div class="value"><asp:Label ID="lblJoinDate" runat="server" /></div>
                        </div>
                        <div class="info-box">
                            <div class="label">Last Login</div>
                            <div class="value"><asp:Label ID="lblLastLogin" runat="server" /></div>
                        </div>
                        <div class="info-box">
                            <div class="label">Last Updated</div>
                            <div class="value"><asp:Label ID="lblUpdatedOn" runat="server" /></div>
                        </div>
                    </div>
                </div>

                <!-- Right Column -->
                <div class="col-md-8">
                    <h5 class="text-primary"><i class="bi bi-person-badge me-1"></i>Personal Information</h5>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">Full Name <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" MaxLength="100" />
                            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full name is required." CssClass="text-danger small" Display="Dynamic" ValidationGroup="Update" />
                            <asp:RegularExpressionValidator ID="revFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Only letters, spaces, digits, apostrophe, hyphen allowed" ValidationExpression="^[A-Za-z0-9\s'-]+$" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Update" />
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">Email <span class="text-danger">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" MaxLength="100" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="text-danger small" Display="Dynamic" ValidationGroup="Update" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Invalid email format." ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Update" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">Phone Number</label>
                            <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" MaxLength="20" />
                            <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhoneNumber" ErrorMessage="Only digits allowed (10-15 digits)" ValidationExpression="^[0-9]{10,15}$" CssClass="text-danger small" Display="Dynamic" ValidationGroup="Update" />
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">Date of Birth</label>
                            <asp:TextBox ID="txtDateOfBirth" runat="server" CssClass="form-control" TextMode="Date" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">Gender</label>
                            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select">
                                <asp:ListItem Text="-- Select --" Value="" />
                                <asp:ListItem Text="Male" Value="Male" />
                                <asp:ListItem Text="Female" Value="Female" />
                                <asp:ListItem Text="Other" Value="Other" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6 mb-3">
                            <label class="form-label fw-semibold">National ID</label>
                            <asp:TextBox ID="txtNationalID" runat="server" CssClass="form-control" MaxLength="50" />
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-semibold">Emergency Contact</label>
                        <asp:TextBox ID="txtEmergencyContact" runat="server" CssClass="form-control" MaxLength="50" placeholder="Name & Phone" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-semibold">Address</label>
                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" MaxLength="200" />
                    </div>

                    <div class="mb-3">
                        <label class="form-label fw-semibold">Bio</label>
                        <asp:TextBox ID="txtBio" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" MaxLength="200" />
                        <small class="text-muted">Max 200 characters</small>
                    </div>

                    <hr />
                    <div class="d-flex gap-2 justify-content-end">
                        <asp:Button ID="btnUpdate" runat="server" Text="Update Profile" CssClass="btn btn-gradient" OnClick="btnUpdate_Click" ValidationGroup="Update" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-outline-gradient" OnClick="btnCancel_Click" CausesValidation="false" />
                    </div>
                </div>
            </div>

            <!-- Security Section -->
            <div class="card mt-4 p-3">
                <h5><i class="bi bi-lock me-1"></i>Security</h5>
                <asp:Button ID="btnChangePassword" runat="server" Text="Change Password" CssClass="btn btn-warning" OnClick="btnChangePassword_Click" CausesValidation="false" />
            </div>
        </div>
    </div>

    <script>
        function showToast(message, type) {
            var container = document.getElementById('toastContainer');
            if (!container) { container = document.createElement('div'); container.id = 'toastContainer'; container.className = 'toast-container'; document.body.appendChild(container); }
            var toast = document.createElement('div');
            toast.className = 'toast-message ' + type;
            toast.innerHTML = '<i class="bi bi-' + (type === 'success' ? 'check-circle' : type === 'error' ? 'x-circle' : 'exclamation-triangle') + '"></i> ' + message;
            container.appendChild(toast);
            setTimeout(function () { toast.style.animation = 'slideOutRight 0.5s ease'; setTimeout(function () { toast.remove(); }, 500); }, 4000);
        }
    </script>
</asp:Content>