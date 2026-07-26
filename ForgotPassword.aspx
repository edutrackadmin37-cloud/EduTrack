<%@ Page Title="Forgot Password - EduTrack" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="EduTrack.Auth.ForgotPassword" %>
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
        .form-control { border-radius: 12px; border: 2px solid #e9ecef; padding: 0.75rem 1rem; }
        .form-control:focus { border-color: #667eea; box-shadow: 0 0 0 0.25rem rgba(102,126,234,0.15); }
        .btn-auth { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border: none; border-radius: 12px; padding: 0.85rem; font-weight: 700; color: #fff; width: 100%; }
        .btn-auth:hover { transform: translateY(-2px); box-shadow: 0 10px 25px rgba(102,126,234,0.6); }
        .auth-link { color: #667eea; text-decoration: none; font-weight: 600; }
        .auth-link:hover { color: #764ba2; }
        .alert { border-radius: 12px; border: none; }
    </style>

    <div class="auth-container">
        <div class="auth-card">
            <div class="auth-header">
                <div class="auth-logo">
                    <img src="<%= ResolveUrl("~/Image/DVT-0185.jpg") %>" alt="EduTrack Logo" />
                </div>
                <h2>Forgot Password</h2>
                <p>We'll send you a reset link</p>
            </div>
            <div class="auth-body">
                <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block" Visible="false" />

                <div class="mb-3">
                    <label class="form-label fw-semibold">Email Address <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter your email" MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required" CssClass="text-danger small" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Enter a valid email address" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label class="form-label fw-semibold">Phone Number (optional)</label>
                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="e.g. 1234567890" MaxLength="20" />
                    <asp:RegularExpressionValidator ID="revPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="Only digits allowed (10-15 digits)" ValidationExpression="^[0-9]{10,15}$" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <asp:Button ID="btnReset" runat="server" Text="Send Reset Link" CssClass="btn-auth" OnClick="btnReset_Click" />

                <div class="text-center mt-4">
                    <a href="Login.aspx" class="auth-link">Back to Login</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>