<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="EduTrack.About" %>
<asp:Content ID="AboutContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; }
        .hero-section { background: var(--primary-gradient); padding: 4rem 0; color: white; border-radius: 0 0 50px 50px; margin-bottom: 2rem; }
        .card-glass { background: rgba(255,255,255,0.95); backdrop-filter: blur(10px); border: 1px solid rgba(255,255,255,0.3); border-radius: 16px; box-shadow: 0 8px 32px rgba(0,0,0,0.08); transition: all 0.3s ease; }
        .card-glass:hover { transform: translateY(-8px); box-shadow: 0 16px 48px rgba(0,0,0,0.12); }
        .feature-icon { width: 70px; height: 70px; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 2rem; background: var(--primary-gradient); color: white; margin: 0 auto 1rem; }
        .team-avatar { width: 130px; height: 130px; border-radius: 50%; object-fit: cover; border: 4px solid var(--primary-color); transition: all 0.3s ease; }
        .stat-number { font-size: 2.8rem; font-weight: 700; color: var(--primary-color); }
        .stat-label { color: #6c757d; font-weight: 500; }
        .btn-gradient { background: var(--primary-gradient); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
        .timeline-item { display: flex; gap: 1rem; padding: 1rem 0; border-bottom: 1px solid #e9ecef; }
        .timeline-item:last-child { border-bottom: none; }
        .timeline-dot { width: 14px; height: 14px; border-radius: 50%; background: var(--primary-color); flex-shrink: 0; margin-top: 4px; }
        .hover-lift { transition: transform 0.3s ease, box-shadow 0.3s ease; }
        .hover-lift:hover { transform: translateY(-10px); box-shadow: 0 20px 40px rgba(0,0,0,0.15) !important; }
        @media (max-width: 768px) { .hero-section h1 { font-size: 2.2rem; } }
    </style>

    <div class="container py-4">
        <!-- Hero Section -->
        <div class="hero-section text-center">
            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-lg-8">
                        <i class="bi bi-mortarboard-fill display-1 mb-3"></i>
                        <h1>About EduTrack</h1>
                        <p class="fs-4 opacity-75">Transforming Education Through Technology</p>
                        <div class="mx-auto" style="width: 80px; height: 3px; background: rgba(255,255,255,0.5); margin: 1.5rem auto;"></div>
                        <p class="fs-5">A comprehensive Project-Based Learning (PBL) management system designed to revolutionize the way educators, students, and parents interact with coursework.</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Mission & Vision -->
        <div class="row g-4 mb-5">
            <div class="col-md-6">
                <div class="card-glass p-4 text-center h-100">
                    <div class="feature-icon"><i class="bi bi-bullseye"></i></div>
                    <h3 class="text-primary">Our Mission</h3>
                    <p class="text-muted">To empower educators, inspire students, and engage parents through innovative project-based learning solutions that foster critical thinking, creativity, and lifelong learning.</p>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card-glass p-4 text-center h-100">
                    <div class="feature-icon" style="background: linear-gradient(135deg, #28a745, #20c997);"><i class="bi bi-eye"></i></div>
                    <h3 class="text-success">Our Vision</h3>
                    <p class="text-muted">Creating a world where every student has access to engaging, personalized, and effective educational experiences that prepare them for the challenges of tomorrow.</p>
                </div>
            </div>
        </div>

        <!-- What is EduTrack -->
        <div class="card-glass p-4 mb-5">
            <h3 class="text-primary"><i class="bi bi-question-circle me-2"></i>What is EduTrack?</h3>
            <p class="fs-5">EduTrack is a <strong>state-of-the-art learning management system</strong> specifically designed for project-based learning environments. We bridge the gap between traditional education and modern pedagogical approaches, providing tools that foster critical thinking, collaboration, and real-world problem-solving skills.</p>
            <p class="fs-5 mb-0">Our platform streamlines project management, assignment submissions, rubric-based assessments, and performance tracking, creating a seamless educational experience for students, teachers, parents, and administrators alike. Built with cutting-edge technology, EduTrack empowers teachers to create engaging project-based curricula while providing students with intuitive tools to collaborate, learn, and excel in the 21st-century classroom.</p>
        </div>

        <!-- What Sets EduTrack Apart -->
        <h3 class="text-center text-primary mb-4"><i class="bi bi-stars me-2"></i>What Sets EduTrack Apart</h3>
        <div class="row g-4 mb-5">
            <div class="col-md-4">
                <div class="card-glass p-4 text-center h-100">
                    <div class="feature-icon" style="background: linear-gradient(135deg, #667eea, #764ba2);"><i class="bi bi-people"></i></div>
                    <h5>Student‑Centric Design</h5>
                    <p class="text-muted">Every feature is designed with the student experience in mind, making learning intuitive, engaging, and accessible to all learners.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card-glass p-4 text-center h-100">
                    <div class="feature-icon" style="background: linear-gradient(135deg, #28a745, #20c997);"><i class="bi bi-graph-up-arrow"></i></div>
                    <h5>Data‑Driven Insights</h5>
                    <p class="text-muted">Real‑time analytics and dashboards help educators identify at‑risk students, track progress, and intervene early to ensure success.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card-glass p-4 text-center h-100">
                    <div class="feature-icon" style="background: linear-gradient(135deg, #ffc107, #fd7e14);"><i class="bi bi-shield-lock"></i></div>
                    <h5>Privacy &amp; Security</h5>
                    <p class="text-muted">Enterprise‑grade security protects student data, ensures compliance with educational regulations, and gives peace of mind to all users.</p>
                </div>
            </div>
        </div>

        <!-- The EduTrack Story (Restored Timeline) -->
        <h3 class="text-center text-primary mb-4"><i class="bi bi-book me-2"></i>The Story Behind EduTrack</h3>
        <div class="card-glass p-4 mb-5 bg-light">
            <div class="timeline-item">
                <div><span class="timeline-dot"></span></div>
                <div><p class="fs-5 mb-0">In today's fast-paced digital world, education needs tools that adapt to modern learning styles. EduTrack was born from a simple idea: <em>what if we could create a platform that makes project-based learning as intuitive as it is effective?</em></p></div>
            </div>
            <div class="timeline-item">
                <div><span class="timeline-dot"></span></div>
                <div><p class="fs-5 mb-0">We've built EduTrack to be more than just software – it's a comprehensive ecosystem where everyone thrives. Our platform combines powerful features with user-friendly design, ensuring that technology enhances education rather than complicates it.</p></div>
            </div>
            <div class="row g-4 mt-3">
                <div class="col-md-3"><div class="d-flex align-items-start"><i class="bi bi-arrow-right-circle-fill text-primary fs-3 me-3 mt-1"></i><div><strong>Teachers</strong> can design meaningful, project-based curricula with ease</div></div></div>
                <div class="col-md-3"><div class="d-flex align-items-start"><i class="bi bi-arrow-right-circle-fill text-success fs-3 me-3 mt-1"></i><div><strong>Students</strong> can engage deeply with their learning and track their growth</div></div></div>
                <div class="col-md-3"><div class="d-flex align-items-start"><i class="bi bi-arrow-right-circle-fill text-info fs-3 me-3 mt-1"></i><div><strong>Administrators</strong> can stay informed about academic progress</div></div></div>
                <div class="col-md-3"><div class="d-flex align-items-start"><i class="bi bi-arrow-right-circle-fill text-warning fs-3 me-3 mt-1"></i><div><strong>Parents</strong> can monitor their child's progress and stay connected</div></div></div>
            </div>
        </div>

        <!-- Who Benefits (Restored Detailed Cards) -->
        <h3 class="text-center text-primary mb-4"><i class="bi bi-people-fill me-2"></i>Who Benefits from EduTrack?</h3>
        <div class="row g-4 mb-5">
            <div class="col-lg-3 col-md-6">
                <div class="card-glass p-3 h-100 border-primary border-2">
                    <div class="card-header bg-primary text-white p-2 rounded-top" style="margin:-1rem -1rem 0 -1rem;"><h5 class="mb-0"><i class="bi bi-person-badge me-2"></i>For Teachers</h5></div>
                    <div class="card-body p-3"><ul class="list-unstyled mb-0"><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Create projects and assignments</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Design custom rubrics</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Grade submissions efficiently</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Track student progress</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Generate detailed reports</li><li class="mb-0"><i class="bi bi-check-circle-fill text-success"></i> Communicate seamlessly</li></ul></div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="card-glass p-3 h-100 border-success border-2">
                    <div class="card-header bg-success text-white p-2 rounded-top" style="margin:-1rem -1rem 0 -1rem;"><h5 class="mb-0"><i class="bi bi-person me-2"></i>For Students</h5></div>
                    <div class="card-body p-3"><ul class="list-unstyled mb-0"><li class="mb-2"><i class="bi bi-check-circle-fill text-primary"></i> Access assignments easily</li><li class="mb-2"><i class="bi bi-check-circle-fill text-primary"></i> Submit work online</li><li class="mb-2"><i class="bi bi-check-circle-fill text-primary"></i> Receive instant feedback</li><li class="mb-2"><i class="bi bi-check-circle-fill text-primary"></i> Track your grades</li><li class="mb-2"><i class="bi bi-check-circle-fill text-primary"></i> Reflect on learning journey</li><li class="mb-0"><i class="bi bi-check-circle-fill text-primary"></i> Stay engaged and motivated</li></ul></div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="card-glass p-3 h-100 border-info border-2">
                    <div class="card-header bg-info text-white p-2 rounded-top" style="margin:-1rem -1rem 0 -1rem;"><h5 class="mb-0"><i class="bi bi-shield-check me-2"></i>For Administrators</h5></div>
                    <div class="card-body p-3"><ul class="list-unstyled mb-0"><li class="mb-2"><i class="bi bi-check-circle-fill text-warning"></i> Manage users and classes</li><li class="mb-2"><i class="bi bi-check-circle-fill text-warning"></i> Monitor system performance</li><li class="mb-2"><i class="bi bi-check-circle-fill text-warning"></i> Generate comprehensive reports</li><li class="mb-2"><i class="bi bi-check-circle-fill text-warning"></i> Oversee academic ecosystem</li><li class="mb-2"><i class="bi bi-check-circle-fill text-warning"></i> Access analytics dashboard</li><li class="mb-0"><i class="bi bi-check-circle-fill text-warning"></i> Configure system settings</li></ul></div>
                </div>
            </div>
            <div class="col-lg-3 col-md-6">
                <div class="card-glass p-3 h-100 border-warning border-2">
                    <div class="card-header bg-warning text-dark p-2 rounded-top" style="margin:-1rem -1rem 0 -1rem;"><h5 class="mb-0"><i class="bi bi-person-lines-fill me-2"></i>For Parents</h5></div>
                    <div class="card-body p-3"><ul class="list-unstyled mb-0"><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Monitor child's academic progress</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> View grades and attendance</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Communicate with teachers</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Receive instant notifications</li><li class="mb-2"><i class="bi bi-check-circle-fill text-success"></i> Support learning at home</li><li class="mb-0"><i class="bi bi-check-circle-fill text-success"></i> Stay informed and engaged</li></ul></div>
                </div>
            </div>
        </div>

        <!-- Statistics -->
        <div class="row g-4 mb-5 text-center">
            <div class="col-md-3 col-6"><div class="card-glass p-3"><i class="bi bi-people-fill text-primary display-4"></i><div class="stat-number">1000+</div><div class="stat-label">Active Users</div></div></div>
            <div class="col-md-3 col-6"><div class="card-glass p-3"><i class="bi bi-folder-fill text-success display-4"></i><div class="stat-number">500+</div><div class="stat-label">Projects Created</div></div></div>
            <div class="col-md-3 col-6"><div class="card-glass p-3"><i class="bi bi-journal-check text-warning display-4"></i><div class="stat-number">5000+</div><div class="stat-label">Assignments Submitted</div></div></div>
            <div class="col-md-3 col-6"><div class="card-glass p-3"><i class="bi bi-star-fill text-danger display-4"></i><div class="stat-number">99.9%</div><div class="stat-label">Uptime Guarantee</div></div></div>
        </div>

        <!-- Why Choose EduTrack (Expanded) -->
        <h3 class="text-center text-primary mb-4"><i class="bi bi-star-fill me-2"></i>Why Choose EduTrack?</h3>
        <div class="row g-4 mb-5">
            <div class="col-md-4"><div class="card-glass p-4 text-center h-100"><div class="feature-icon bg-primary bg-opacity-10" style="background: rgba(102,126,234,0.1);"><i class="bi bi-kanban text-primary fs-2"></i></div><h5>Comprehensive Management</h5><p class="text-muted">Complete project and assignment management tools in one integrated platform.</p></div></div>
            <div class="col-md-4"><div class="card-glass p-4 text-center h-100"><div class="feature-icon bg-success bg-opacity-10" style="background: rgba(40,167,69,0.1);"><i class="bi bi-clipboard-check text-success fs-2"></i></div><h5>Advanced Assessment</h5><p class="text-muted">Rubric-based grading and evaluation tools for fair, consistent, and transparent assessment.</p></div></div>
            <div class="col-md-4"><div class="card-glass p-4 text-center h-100"><div class="feature-icon bg-warning bg-opacity-10" style="background: rgba(255,193,7,0.1);"><i class="bi bi-graph-up text-warning fs-2"></i></div><h5>Real-Time Analytics</h5><p class="text-muted">Track progress with comprehensive reports, dashboards, and data-driven insights.</p></div></div>
            <div class="col-md-4"><div class="card-glass p-4 text-center h-100"><div class="feature-icon bg-info bg-opacity-10" style="background: rgba(23,162,184,0.1);"><i class="bi bi-chat-dots text-info fs-2"></i></div><h5>Seamless Communication</h5><p class="text-muted">Integrated messaging and real-time notifications keep everyone connected and informed.</p></div></div>
            <div class="col-md-4"><div class="card-glass p-4 text-center h-100"><div class="feature-icon bg-danger bg-opacity-10" style="background: rgba(220,53,69,0.1);"><i class="bi bi-phone text-danger fs-2"></i></div><h5>Mobile-Friendly</h5><p class="text-muted">Access anywhere, anytime, on any device – fully responsive design for seamless learning on the go.</p></div></div>
            <div class="col-md-4"><div class="card-glass p-4 text-center h-100"><div class="feature-icon bg-dark bg-opacity-10" style="background: rgba(0,0,0,0.1);"><i class="bi bi-shield-check text-dark fs-2"></i></div><h5>Secure & Reliable</h5><p class="text-muted">Enterprise-grade security with 99.9% uptime guarantee, ensuring your data is always safe and accessible.</p></div></div>
        </div>

        <!-- Meet the Team -->
        <h3 class="text-center text-primary mb-4"><i class="bi bi-people-fill me-2"></i>Meet the Team</h3>
        <div class="row g-4 mb-5">
            <div class="col-md-4"><div class="card-glass p-4 text-center"><asp:Image ID="imgTeam1" runat="server" ImageUrl="~/Image/DVT-0176.JPG" CssClass="team-avatar mb-3" AlternateText="NYARKO TIMOTHY AKWASI" /><h5>NYARKO TIMOTHY AKWASI</h5><p class="text-muted">Lead Developer &amp; Architect</p><p class="small text-muted">Passionate about building educational technology that empowers learners and transforms classrooms.</p></div></div>
            <div class="col-md-4"><div class="card-glass p-4 text-center"><asp:Image ID="imgTeam2" runat="server" ImageUrl="~/Image/DVT-0175.jpg" CssClass="team-avatar mb-3" AlternateText="SUMAILA ABDUL-RAHMAN" /><h5>SUMAILA ABDUL-RAHMAN</h5><p class="text-muted">Quality Assurance &amp; Support</p><p class="small text-muted">Ensuring every feature works flawlessly for educators and students, delivering a seamless user experience.</p></div></div>
            <div class="col-md-4"><div class="card-glass p-4 text-center"><asp:Image ID="imgTeam3" runat="server" ImageUrl="~/Image/DVT-0181.JPG" CssClass="team-avatar mb-3" AlternateText="DR. DANIEL DANSO ESSEL" /><h5>DR. DANIEL DANSO ESSEL</h5><p class="text-muted">Subject Matter Expert</p><p class="small text-muted">Bringing deep pedagogical expertise to every aspect of the platform, bridging education and technology.</p></div></div>
        </div>

        <!-- CTA -->
        <div class="text-center p-5 rounded-4" style="background: var(--primary-gradient); color: white;">
            <h3><i class="bi bi-rocket-takeoff me-2"></i>Join the EduTrack Community</h3>
            <p class="fs-5">Join thousands of students, educators, and parents transforming the classroom experience. Experience the future of project-based learning today!</p>
            <a href="<%= ResolveUrl("~/Auth/Register.aspx") %>" class="btn btn-warning btn-lg px-5"><i class="bi bi-person-plus-fill me-2"></i>Get Started Now</a>
        </div>
    </div>
</asp:Content>