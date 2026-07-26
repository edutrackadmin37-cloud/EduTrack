<%@ Page Title="Page Not Found" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Error404.aspx.cs" Inherits="EduTrack.Error404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .error-container {
            max-width: 600px;
            margin: 80px auto;
            text-align: center;
            padding: 2rem;
        }
        .error-icon { font-size: 5rem; color: #ffc107; display: block; margin-bottom: 1.5rem; }
        .error-code { font-size: 6rem; font-weight: 700; color: #6c757d; line-height: 1; }
        .error-title { font-size: 2.2rem; font-weight: 700; margin: 0.5rem 0; }
        .error-description { font-size: 1.1rem; color: #6c757d; margin-bottom: 2rem; }
        .btn-gradient { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
    </style>
    <div class="error-container">
        <span class="error-icon"><i class="bi bi-exclamation-triangle-fill"></i></span>
        <div class="error-code">404</div>
        <h1 class="error-title">Page Not Found</h1>
        <p class="error-description">The page you are looking for might have been removed, had its name changed, or is temporarily unavailable.</p>
        <a href="~/Default.aspx" class="btn btn-gradient"><i class="bi bi-house"></i> Go Home</a>
        <a href="javascript:history.back()" class="btn btn-outline-secondary ms-2"><i class="bi bi-arrow-left"></i> Go Back</a>
    </div>
</asp:Content>