<%@ Page Title="Documentation" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Documentation.aspx.cs" Inherits="EduTrack.Documentation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; }
        .card-glass { background: rgba(255,255,255,0.95); backdrop-filter: blur(10px); border: 1px solid rgba(255,255,255,0.3); border-radius: 16px; box-shadow: 0 8px 32px rgba(0,0,0,0.08); transition: all 0.3s ease; }
        .btn-gradient { background: var(--primary-gradient); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
        .search-box .form-control { padding-left: 45px; border-radius: 50px; border: 2px solid #e9ecef; transition: all 0.3s; }
        .search-box .form-control:focus { border-color: var(--primary-color); box-shadow: 0 0 0 3px rgba(102,126,234,0.2); }
        .search-box .bi-search { position: absolute; left: 16px; top: 50%; transform: translateY(-50%); color: #6c757d; font-size: 1.2rem; }
        .accordion-item { border: none; margin-bottom: 12px; border-radius: 12px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.06); }
        .accordion-button { font-weight: 600; padding: 1rem 1.5rem; background: #f8f9fa; border: none; }
        .accordion-button:not(.collapsed) { color: white; background: var(--primary-gradient); box-shadow: none; }
        .accordion-button:not(.collapsed)::after { filter: brightness(0) invert(1); }
        .accordion-body { padding: 1.5rem; background: white; border-top: 1px solid #e9ecef; }
        .doc-tag { display: inline-block; padding: 2px 12px; border-radius: 50px; font-size: 0.7rem; font-weight: 600; text-transform: uppercase; letter-spacing: 0.5px; margin-right: 6px; }
        .tag-new { background: #d1e7dd; color: #0a5e3a; }
        .tag-popular { background: #cfe2ff; color: #084298; }
        .tag-updated { background: #fff3cd; color: #664d03; }
        .quick-link-btn { border-radius: 50px; transition: all 0.3s; }
        .quick-link-btn:hover { transform: translateY(-2px); box-shadow: 0 4px 12px rgba(0,0,0,0.12); }
    </style>

    <div class="container py-4">
        <div class="card-glass p-4 mb-4">
            <div class="d-flex justify-content-between align-items-center flex-wrap">
                <div>
                    <h1 class="display-5 fw-bold text-primary"><i class="bi bi-journal-bookmark me-2"></i>Documentation</h1>
                    <p class="text-muted">Find guides, how‑tos, policies, and troubleshooting for every part of EduTrack.</p>
                </div>
                <span class="badge bg-primary-subtle text-primary px-3 py-2 border border-primary"><i class="bi bi-clock me-1"></i>Updated: <asp:Label ID="lblLastUpdated" runat="server" Text="January 2026" /></span>
            </div>
        </div>

        <!-- Search Box -->
        <div class="search-box position-relative mb-4">
            <i class="bi bi-search"></i>
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-lg" placeholder="Search documentation topics..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
        </div>

        <!-- Quick Links -->
        <div class="d-flex flex-wrap gap-2 mb-4">
            <a href="#doc1" class="btn btn-outline-primary btn-sm quick-link-btn">Getting Started</a>
            <a href="#doc2" class="btn btn-outline-info btn-sm quick-link-btn">Assignments</a>
            <a href="#doc3" class="btn btn-outline-success btn-sm quick-link-btn">Projects</a>
            <a href="#doc4" class="btn btn-outline-warning btn-sm quick-link-btn">Tests &amp; Grades</a>
            <a href="#doc5" class="btn btn-outline-secondary btn-sm quick-link-btn">Admin &amp; Support</a>
            <a href="#doc6" class="btn btn-outline-danger btn-sm quick-link-btn">FAQs</a>
            <a href="#doc7" class="btn btn-outline-warning btn-sm quick-link-btn"><i class="bi bi-person-lines-fill"></i> Parents</a>
        </div>

        <!-- Accordion -->
        <div class="accordion" id="accDocs">
            <!-- Topic 1: Getting Started -->
            <div class="accordion-item" id="doc1">
                <h2 class="accordion-header" id="head1">
                    <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapse1" aria-expanded="true" aria-controls="collapse1">
                        <span class="doc-tag tag-popular">Popular</span>
                        <strong>Getting Started / Onboarding</strong>
                    </button>
                </h2>
                <div id="collapse1" class="accordion-collapse collapse show" aria-labelledby="head1" data-bs-parent="#accDocs">
                    <div class="accordion-body">
                        <ul>
                            <li><strong>How to register and log in</strong> – Create your account and access the system.</li>
                            <li><strong>Completing your user profile</strong> – Add your personal information and preferences.</li>
                            <li><strong>Understanding your dashboard</strong> – Navigate the student, teacher, admin, or parent interface.</li>
                            <li><strong>System requirements</strong> – Browser and device recommendations for optimal performance.</li>
                            <li><strong>First‑time user guide</strong> – A walkthrough of key features for new users.</li>
                        </ul>
                        <div class="mt-3"><a href="Help.aspx" class="btn btn-primary btn-sm">Get Help <i class="bi bi-arrow-right"></i></a></div>
                    </div>
                </div>
            </div>

            <!-- Topic 2: Assignments & Submissions -->
            <div class="accordion-item" id="doc2">
                <h2 class="accordion-header" id="head2">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse2" aria-expanded="false" aria-controls="collapse2">
                        <span class="doc-tag tag-new">Updated</span>
                        <strong>Assignments &amp; Submissions</strong>
                    </button>
                </h2>
                <div id="collapse2" class="accordion-collapse collapse" aria-labelledby="head2" data-bs-parent="#accDocs">
                    <div class="accordion-body">
                        <ul>
                            <li><strong>Viewing assignments</strong> – See all active assignments with due dates.</li>
                            <li><strong>Submitting work</strong> – Upload files and add optional remarks.</li>
                            <li><strong>Checking submission status</strong> – Confirm your work has been received.</li>
                            <li><strong>Viewing feedback and grades</strong> – Access rubric scores and teacher comments.</li>
                            <li><strong>Resubmitting assignments</strong> – Replace a previous submission with a new version.</li>
                        </ul>
                        <div class="mt-3"><a href="Assignments.aspx" class="btn btn-info btn-sm">Go to Assignments <i class="bi bi-arrow-right"></i></a></div>
                    </div>
                </div>
            </div>

            <!-- Topic 3: Projects & Teams -->
            <div class="accordion-item" id="doc3">
                <h2 class="accordion-header" id="head3">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse3" aria-expanded="false" aria-controls="collapse3">
                        <span class="doc-tag tag-popular">Popular</span>
                        <strong>Projects &amp; Teams</strong>
                    </button>
                </h2>
                <div id="collapse3" class="accordion-collapse collapse" aria-labelledby="head3" data-bs-parent="#accDocs">
                    <div class="accordion-body">
                        <ul>
                            <li><strong>Creating a project</strong> – Set up a new project with title, description, and dates.</li>
                            <li><strong>Joining a project</strong> – Students can join projects via class enrollment.</li>
                            <li><strong>Managing teams</strong> – Create teams, add members, and remove members.</li>
                            <li><strong>Collaboration tools</strong> – Use built‑in chat and shared resources.</li>
                            <li><strong>Peer assessment flow</strong> – Provide structured feedback to classmates.</li>
                        </ul>
                        <div class="mt-3"><a href="Projects.aspx" class="btn btn-success btn-sm">Go to Projects <i class="bi bi-arrow-right"></i></a></div>
                    </div>
                </div>
            </div>

            <!-- Topic 4: Tests & Analytics -->
            <div class="accordion-item" id="doc4">
                <h2 class="accordion-header" id="head4">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse4" aria-expanded="false" aria-controls="collapse4">
                        <span class="doc-tag tag-updated">Updated</span>
                        <strong>Tests, Grades &amp; Analytics</strong>
                    </button>
                </h2>
                <div id="collapse4" class="accordion-collapse collapse" aria-labelledby="head4" data-bs-parent="#accDocs">
                    <div class="accordion-body">
                        <ul>
                            <li><strong>Taking tests/quizzes</strong> – Answer MCQ, True/False, Fill‑in, and Essay questions.</li>
                            <li><strong>Viewing grades</strong> – See your scores and overall averages.</li>
                            <li><strong>Understanding rubrics</strong> – Learn how criteria are scored.</li>
                            <li><strong>Using analytics</strong> – Track performance and identify areas for improvement.</li>
                            <li><strong>Exporting reports</strong> – Download grades, test results, and attendance data as CSV.</li>
                        </ul>
                        <div class="mt-3"><a href="Tests.aspx" class="btn btn-warning btn-sm">Go to Tests <i class="bi bi-arrow-right"></i></a></div>
                    </div>
                </div>
            </div>

            <!-- Topic 5: Admin & Support -->
            <div class="accordion-item" id="doc5">
                <h2 class="accordion-header" id="head5">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse5" aria-expanded="false" aria-controls="collapse5">
                        <strong>Admin &amp; Support</strong>
                    </button>
                </h2>
                <div id="collapse5" class="accordion-collapse collapse" aria-labelledby="head5" data-bs-parent="#accDocs">
                    <div class="accordion-body">
                        <ul>
                            <li><strong>User account management</strong> – Approve, deactivate, or delete users.</li>
                            <li><strong>System settings</strong> – Configure institution‑wide preferences and feature toggles.</li>
                            <li><strong>Audit logs</strong> – Monitor critical user and system activity.</li>
                            <li><strong>Contacting support</strong> – Reach out to the help desk for assistance.</li>
                            <li><strong>FAQs &amp; community resources</strong> – Browse common questions and community discussions.</li>
                        </ul>
                        <div class="mt-3"><a href="Contact.aspx" class="btn btn-secondary btn-sm">Contact Support <i class="bi bi-arrow-right"></i></a></div>
                    </div>
                </div>
            </div>

            <!-- Topic 6: FAQ -->
            <div class="accordion-item" id="doc6">
                <h2 class="accordion-header" id="head6">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse6" aria-expanded="false" aria-controls="collapse6">
                        <strong><i class="bi bi-patch-question me-2"></i>Frequently Asked Questions</strong>
                    </button>
                </h2>
                <div id="collapse6" class="accordion-collapse collapse" aria-labelledby="head6" data-bs-parent="#accDocs">
                    <div class="accordion-body">
                        <div class="mb-3"><h6><i class="bi bi-question-circle text-primary me-2"></i>What is EduTrack?</h6><p>EduTrack is a comprehensive Learning Management System designed for project‑based learning.</p></div>
                        <div class="mb-3"><h6><i class="bi bi-question-circle text-primary me-2"></i>Is EduTrack free to use?</h6><p>Yes! EduTrack is completely free for educational institutions.</p></div>
                        <div class="mb-3"><h6><i class="bi bi-question-circle text-primary me-2"></i>Can I use EduTrack on mobile?</h6><p>Yes – the system is fully responsive and works on smartphones, tablets, and desktops.</p></div>
                        <div class="mb-3"><h6><i class="bi bi-question-circle text-primary me-2"></i>How secure is my data?</h6><p>All data is encrypted and stored securely with enterprise‑grade protection.</p></div>
                        <div><h6><i class="bi bi-question-circle text-primary me-2"></i>How do I get support?</h6><p>Contact us via the <a href="Contact.aspx">Contact page</a> or email <a href="mailto:nyarkoakwasi36@gmail.com">nyarkoakwasi36@gmail.com</a>.</p></div>
                    </div>
                </div>
            </div>

            <!-- Topic 7: Parents -->
            <div class="accordion-item" id="doc7">
                <h2 class="accordion-header" id="head7">
                    <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse7" aria-expanded="false" aria-controls="collapse7">
                        <span class="doc-tag tag-new">New</span>
                        <strong><i class="bi bi-person-lines-fill me-2"></i>For Parents</strong>
                    </button>
                </h2>
                <div id="collapse7" class="accordion-collapse collapse" aria-labelledby="head7" data-bs-parent="#accDocs">
                    <div class="accordion-body">
                        <ul>
                            <li><strong>Linking to your child</strong> – How to connect your parent account to your child’s student account.</li>
                            <li><strong>Monitoring progress</strong> – View grades, assignment scores, and overall performance.</li>
                            <li><strong>Tracking attendance</strong> – See your child’s attendance record and late arrivals.</li>
                            <li><strong>Engagement insights</strong> – Understand your child’s engagement levels and reflections.</li>
                            <li><strong>Communication with teachers</strong> – Send and receive messages through the built‑in messaging system.</li>
                            <li><strong>Notifications</strong> – Get real‑time alerts about your child’s academic activities.</li>
                            <li><strong>Supporting learning at home</strong> – Use the resources and reports to help your child succeed.</li>
                        </ul>
                        <div class="mt-3"><a href="ParentPortal.aspx" class="btn btn-warning btn-sm">Go to Parent Portal <i class="bi bi-arrow-right"></i></a></div>
                    </div>
                </div>
            </div>
        </div>

        <hr class="my-5" />

        <div class="text-center">
            <p class="text-muted"><i class="bi bi-info-circle me-1"></i>Can't find what you're looking for? <a href="Contact.aspx" class="text-primary fw-semibold">Contact our support team</a> or check the <a href="FAQ.aspx" class="text-primary fw-semibold">FAQ</a>.</p>
            <div class="mt-3">
                <a href="Help.aspx" class="btn btn-outline-primary me-2"><i class="bi bi-question-circle"></i> Help Center</a>
                <a href="Contact.aspx" class="btn btn-gradient"><i class="bi bi-envelope"></i> Contact Support</a>
            </div>
        </div>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', function () {
            var searchInput = document.getElementById('<%= txtSearch.ClientID %>');
            if (!searchInput) return;
            var accordionItems = document.querySelectorAll('.accordion-item');
            searchInput.addEventListener('keyup', function () {
                var query = this.value.toLowerCase().trim();
                accordionItems.forEach(function (item) {
                    var text = item.textContent.toLowerCase();
                    if (query === '') { item.style.display = ''; }
                    else if (text.includes(query)) {
                        item.style.display = '';
                        var collapse = item.querySelector('.accordion-collapse');
                        if (collapse) { var bs = bootstrap.Collapse.getInstance(collapse) || new bootstrap.Collapse(collapse, { toggle: false }); if (!collapse.classList.contains('show')) bs.show(); }
                    } else { item.style.display = 'none'; }
                });
            });
        });
    </script>
</asp:Content>