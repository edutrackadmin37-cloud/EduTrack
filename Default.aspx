<%@ Page Title="Home - EduTrack" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="EduTrack.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        /* Hero Section */
        .hero-section {
            background: var(--primary-gradient);
            color: white;
            padding: 5rem 0;
            border-radius: 0 0 50px 50px;
            margin-bottom: 3rem;
            text-align: center;
        }
        .hero-section h1 {
            font-size: 3.5rem;
            font-weight: 800;
            margin-bottom: 1rem;
        }
        .hero-section p {
            font-size: 1.25rem;
            opacity: 0.9;
            max-width: 700px;
            margin: 0 auto 2rem;
        }
        .hero-section .btn-hero {
            background: white;
            color: #667eea;
            border: none;
            border-radius: 50px;
            padding: 0.8rem 2.5rem;
            font-weight: 700;
            transition: all 0.3s;
        }
        .hero-section .btn-hero:hover {
            transform: translateY(-3px);
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
            color: #764ba2;
        }
        .hero-section .btn-hero-outline {
            background: transparent;
            color: white;
            border: 2px solid white;
            border-radius: 50px;
            padding: 0.8rem 2.5rem;
            font-weight: 700;
            transition: all 0.3s;
        }
        .hero-section .btn-hero-outline:hover {
            background: white;
            color: #667eea;
            transform: translateY(-3px);
        }

        /* Feature Cards */
        .feature-card {
            background: rgba(255,255,255,0.95);
            backdrop-filter: blur(10px);
            border: 1px solid rgba(255,255,255,0.3);
            border-radius: 16px;
            padding: 2rem 1.5rem;
            text-align: center;
            box-shadow: 0 8px 32px rgba(0,0,0,0.08);
            transition: transform 0.3s ease;
            height: 100%;
        }
        .feature-card:hover {
            transform: translateY(-8px);
            box-shadow: 0 16px 48px rgba(0,0,0,0.12);
        }
        .feature-card .icon {
            font-size: 3rem;
            background: var(--primary-gradient);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
            display: inline-block;
            margin-bottom: 1rem;
        }
        .feature-card h5 {
            font-weight: 700;
            margin-bottom: 0.5rem;
        }
        .feature-card p {
            color: #6c757d;
            font-size: 0.95rem;
        }

        /* Stats Section */
        .stats-section {
            background: #f8f9fa;
            padding: 3rem 0;
            border-radius: 30px;
            margin: 3rem 0;
        }
        .stat-item {
            text-align: center;
        }
        .stat-item .number {
            font-size: 2.8rem;
            font-weight: 700;
            background: var(--primary-gradient);
            -webkit-background-clip: text;
            -webkit-text-fill-color: transparent;
        }
        .stat-item .label {
            color: #6c757d;
            font-weight: 500;
        }

        /* CTA Section */
        .cta-section {
            background: var(--primary-gradient);
            color: white;
            padding: 3rem 2rem;
            border-radius: 30px;
            text-align: center;
        }
        .cta-section h3 {
            font-weight: 700;
        }
        .cta-section .btn-cta {
            background: white;
            color: #667eea;
            border: none;
            border-radius: 50px;
            padding: 0.8rem 2.5rem;
            font-weight: 700;
            transition: all 0.3s;
        }
        .cta-section .btn-cta:hover {
            transform: translateY(-3px);
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
            color: #764ba2;
        }
    </style>

    <!-- HERO SECTION -->
    <div class="hero-section">
        <div class="container">
            <h1>Welcome to EduTrack</h1>
            <p>Transform your school with project-based learning. Manage projects, assignments, teams, and assessments—all in one place.</p>
            <div>
                <asp:HyperLink ID="hlGetStarted" runat="server" NavigateUrl="~/Auth/Register.aspx" CssClass="btn-hero me-3"><i class="bi bi-rocket-takeoff"></i> Get Started</asp:HyperLink>
                <asp:HyperLink ID="hlLearnMore" runat="server" NavigateUrl="~/About.aspx" CssClass="btn-hero-outline"><i class="bi bi-info-circle"></i> Learn More</asp:HyperLink>
            </div>
        </div>
    </div>

    <!-- FEATURES SECTION -->
    <div class="container">
        <h2 class="text-center mb-4">Why EduTrack?</h2>
        <div class="row g-4">
            <div class="col-md-4">
                <div class="feature-card">
                    <div class="icon"><i class="bi bi-kanban"></i></div>
                    <h5>Project Management</h5>
                    <p>Create, manage, and track projects through their full lifecycle – from proposal to closure.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <div class="icon"><i class="bi bi-people"></i></div>
                    <h5>Team Collaboration</h5>
                    <p>Form teams, assign members, and collaborate seamlessly with built-in chat and discussion boards.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <div class="icon"><i class="bi bi-star"></i></div>
                    <h5>Rubric-Based Grading</h5>
                    <p>Grade projects and contributions with custom rubrics, ensuring fair and transparent assessment.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <div class="icon"><i class="bi bi-graph-up"></i></div>
                    <h5>Analytics &amp; Reports</h5>
                    <p>Generate detailed reports on student performance, engagement, and project progress.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <div class="icon"><i class="bi bi-calendar-check"></i></div>
                    <h5>Attendance Tracking</h5>
                    <p>Mark attendance with start times, grace periods, and automatic late detection.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <div class="icon"><i class="bi bi-shield-lock"></i></div>
                    <h5>Secure &amp; Reliable</h5>
                    <p>Enterprise-grade security with role-based access and audit logs.</p>
                </div>
            </div>
        </div>
    </div>

    <!-- STATS SECTION -->
    <div class="stats-section container">
        <div class="row">
            <div class="col-md-3 stat-item">
                <div class="number">1000+</div>
                <div class="label">Active Users</div>
            </div>
            <div class="col-md-3 stat-item">
                <div class="number">500+</div>
                <div class="label">Projects</div>
            </div>
            <div class="col-md-3 stat-item">
                <div class="number">5000+</div>
                <div class="label">Submissions</div>
            </div>
            <div class="col-md-3 stat-item">
                <div class="number">99.9%</div>
                <div class="label">Uptime</div>
            </div>
        </div>
    </div>

    <!-- CTA SECTION -->
    <div class="container cta-section mb-4">
        <h3>Ready to transform your school?</h3>
        <p class="mb-3">Join thousands of educators and students already using EduTrack.</p>
        <asp:HyperLink ID="hlCTARegister" runat="server" NavigateUrl="~/Auth/Register.aspx" CssClass="btn-cta"><i class="bi bi-person-plus"></i> Create Your Account</asp:HyperLink>
    </div>
</asp:Content>