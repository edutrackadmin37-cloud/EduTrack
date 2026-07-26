<%@ Page Title="Register - EduTrack" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="EduTrack.Auth.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        body { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); min-height: 100vh; }
        .auth-container { max-width: 480px; margin: 60px auto; padding: 0 20px; }
        .auth-card { border: none; border-radius: 22px; background: #fff; box-shadow: 0 20px 65px rgba(0,0,0,0.35); }
        .auth-header { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 2rem 2rem 1.5rem; text-align: center; color: white; border-radius: 22px 22px 0 0; }
        .auth-header h2 { font-weight: 700; margin-bottom: 0.25rem; }
        .auth-header p { margin: 0; opacity: 0.9; }
        .auth-logo { width: 100px; height: 100px; border-radius: 50%; background: white; margin: 0 auto 1rem; border: 4px solid rgba(255,255,255,0.3); overflow: hidden; box-shadow: 0 10px 28px rgba(0,0,0,0.25); }
        .auth-logo img { width: 100%; height: 100%; object-fit: cover; }
        .auth-body { padding: 2rem 2.5rem; }
        .form-control, .form-select { border-radius: 12px; border: 2px solid #e9ecef; padding: 0.75rem 1rem; }
        .form-control:focus, .form-select:focus { border-color: #667eea; box-shadow: 0 0 0 0.25rem rgba(102,126,234,0.15); }
        .btn-auth { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border: none; border-radius: 12px; padding: 0.85rem; font-weight: 700; color: #fff; width: 100%; }
        .btn-auth:hover { transform: translateY(-2px); box-shadow: 0 10px 25px rgba(102,126,234,0.6); }
        .auth-link { color: #667eea; text-decoration: none; font-weight: 600; }
        .auth-link:hover { color: #764ba2; }
        .alert { border-radius: 12px; border: none; }
        .password-requirements { font-size: 0.8rem; color: #6c757d; margin-top: 4px; }
        .password-requirements ul { padding-left: 1.2rem; margin-bottom: 0; }
        .password-requirements li { list-style-type: none; }
        .form-check-input:checked { background-color: #667eea; border-color: #667eea; }
    </style>

    <div class="auth-container">
        <div class="auth-card">
            <div class="auth-header">
                <div class="auth-logo">
                    <img src="<%= ResolveUrl("~/Image/DVT-0185.jpg") %>" alt="EduTrack Logo" />
                </div>
                <h2>Create Account</h2>
                <p>Join EduTrack – Project-Based Learning Platform</p>
            </div>
            <div class="auth-body">
                <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block" Visible="false" />

                <div class="mb-3">
                    <label class="form-label fw-semibold">Full Name <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter your full name" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Full name is required" CssClass="text-danger small" Display="Dynamic" />
                    <!-- Allow letters, spaces, digits, apostrophe, hyphen -->
                    <asp:RegularExpressionValidator ID="revFullName" runat="server" ControlToValidate="txtFullName" ErrorMessage="Only letters, spaces, digits, apostrophe, hyphen allowed" ValidationExpression="^[A-Za-z0-9\s'-]+$" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">Email Address <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter your email" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger small" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Enter a valid email address" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">Phone Number</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="e.g. 1234567890" MaxLength="20" />
                    <!-- Only digits allowed -->
                    <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Only digits allowed (10-15 digits)" ValidationExpression="^[0-9]{10,15}$" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">Password <span class="text-danger">*</span></label>
                    <div class="input-group">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create a password" MaxLength="100" />
                        <button type="button" class="btn btn-outline-secondary" onclick="togglePassword()" style="border-radius: 0 12px 12px 0;">
                            <i id="pwdIcon" class="bi bi-eye-slash"></i>
                        </button>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required" CssClass="text-danger small" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password must be at least 8 characters with upper, lower, digit, and special character" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$" CssClass="text-danger small" Display="Dynamic" />
                    <div class="password-requirements mt-1">
                        <ul>
                            <li id="reqLength"><i class="bi bi-x"></i> At least 8 characters</li>
                            <li id="reqUpper"><i class="bi bi-x"></i> At least 1 uppercase</li>
                            <li id="reqLower"><i class="bi bi-x"></i> At least 1 lowercase</li>
                            <li id="reqDigit"><i class="bi bi-x"></i> At least 1 number</li>
                            <li id="reqSpecial"><i class="bi bi-x"></i> At least 1 special character (!@#$%^&*)</li>
                        </ul>
                    </div>
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">Confirm Password <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm your password" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvConfirm" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Please confirm your password" CssClass="text-danger small" Display="Dynamic" />
                    <asp:CompareValidator ID="cvConfirm" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" Operator="Equal" ErrorMessage="Passwords do not match" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">Role <span class="text-danger">*</span></label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                        <asp:ListItem Value="Student">Student</asp:ListItem>
                        <asp:ListItem Value="Teacher">Teacher</asp:ListItem>
                        <asp:ListItem Value="Parent">Parent</asp:ListItem>
                        <asp:ListItem Value="Headmaster">Headmaster</asp:ListItem>
                        <asp:ListItem Value="AssistantHeadmaster">Assistant Headmaster</asp:ListItem>
                        <asp:ListItem Value="AcademicCoordinator">Academic Coordinator</asp:ListItem>
                        <asp:ListItem Value="HOD">HOD (Head of Department)</asp:ListItem>
                        <asp:ListItem Value="SystemAdministrator">System Administrator</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <div class="form-check">
                        <asp:CheckBox ID="chkTerms" runat="server" CssClass="form-check-input" />
                        <label class="form-check-label" for="chkTerms">
                            I agree to the <a href="#" class="auth-link">Terms of Service</a> and <a href="#" class="auth-link">Privacy Policy</a>
                        </label>
                        <asp:CustomValidator ID="cvTerms" runat="server" OnServerValidate="cvTerms_ServerValidate" ErrorMessage="You must agree to the terms" CssClass="text-danger small" Display="Dynamic" />
                    </div>
                </div>

                <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn-auth" OnClick="btnRegister_Click" />

                <div class="text-center mt-4">
                    <span class="text-muted">Already have an account?</span>
                    <a href="Login.aspx" class="auth-link">Sign in</a>
                </div>
            </div>
        </div>
    </div>

    <script>
        function togglePassword() {
            var pwd = document.getElementById('<%= txtPassword.ClientID %>');
            var icon = document.getElementById('pwdIcon');
            if (pwd.type === 'password') {
                pwd.type = 'text';
                icon.className = 'bi bi-eye';
            } else {
                pwd.type = 'password';
                icon.className = 'bi bi-eye-slash';
            }
        }

        document.addEventListener('DOMContentLoaded', function() {
            var pwd = document.getElementById('<%= txtPassword.ClientID %>');
            pwd.addEventListener('keyup', function () {
                var val = this.value;
                var checks = {
                    length: val.length >= 8,
                    upper: /[A-Z]/.test(val),
                    lower: /[a-z]/.test(val),
                    digit: /\d/.test(val),
                    special: /[!@#$%^&*]/.test(val)
                };
                updateRequirement('reqLength', checks.length);
                updateRequirement('reqUpper', checks.upper);
                updateRequirement('reqLower', checks.lower);
                updateRequirement('reqDigit', checks.digit);
                updateRequirement('reqSpecial', checks.special);
            });
        });

        function updateRequirement(id, valid) {
            var el = document.getElementById(id);
            if (!el) return;
            var icon = el.querySelector('i');
            if (valid) {
                icon.className = 'bi bi-check text-success';
                el.classList.add('text-success');
                el.classList.remove('text-muted');
            } else {
                icon.className = 'bi bi-x text-danger';
                el.classList.remove('text-success');
                el.classList.add('text-muted');
            }
        }
    </script>
</asp:Content>