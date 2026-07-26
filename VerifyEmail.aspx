<%@ Page Title="Verify Email - EduTrack" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VerifyEmail.aspx.cs" Inherits="EduTrack.Auth.VerifyEmail" %>
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
        .auth-body { padding: 2rem 2.5rem; text-align: center; }
        .btn-auth { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); border: none; border-radius: 12px; padding: 0.85rem 2rem; font-weight: 700; color: #fff; display: inline-block; }
        .btn-auth:hover { transform: translateY(-2px); box-shadow: 0 10px 25px rgba(102,126,234,0.6); color: white; }
        .auth-link { color: #667eea; text-decoration: none; font-weight: 600; }
        .auth-link:hover { color: #764ba2; }
        .alert { border-radius: 12px; border: none; }
        .icon-large { font-size: 4rem; display: block; margin-bottom: 1rem; }
    </style>

    <div class="auth-container">
        <div class="auth-card">
            <div class="auth-header">
                <div class="auth-logo">
                    <img src="<%= ResolveUrl("~/Image/DVT-0185.jpg") %>" alt="EduTrack Logo" />
                </div>
                <h2>Email Verification</h2>
                <p>Confirm your email address</p>
            </div>
            <div class="auth-body">
                <asp:Label ID="lblMessage" runat="server" CssClass="alert d-block" Visible="false" />

                <div id="divSuccess" runat="server" visible="false">
                    <i class="bi bi-check-circle-fill text-success icon-large"></i>
                    <h4>Email Verified!</h4>
                    <p class="text-muted">Your email has been successfully verified.</p>
                    <a href="Login.aspx" class="btn-auth mt-3">Login Now</a>
                </div>

                <div id="divError" runat="server" visible="false">
                    <i class="bi bi-x-circle-fill text-danger icon-large"></i>
                    <h4>Verification Failed</h4>
                    <p class="text-muted"><asp:Label ID="lblErrorDetail" runat="server" /></p>
                    <a href="Login.aspx" class="auth-link">Back to Login</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>