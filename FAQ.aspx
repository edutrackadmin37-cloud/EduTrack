<%@ Page Title="FAQ" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FAQ.aspx.cs" Inherits="EduTrack.FAQ" %>
<asp:Content ID="FAQContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        :root { --primary-gradient: linear-gradient(135deg, #667eea 0%, #764ba2 100%); --primary-color: #667eea; --secondary-color: #764ba2; }
        body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f8f9fa; margin: 0; padding: 0; }
        .card-glass { background: rgba(255,255,255,0.95); backdrop-filter: blur(10px); border: 1px solid rgba(255,255,255,0.3); border-radius: 16px; box-shadow: 0 8px 32px rgba(0,0,0,0.08); transition: all 0.3s ease; }
        .btn-gradient { background: var(--primary-gradient); color: white; border: none; border-radius: 8px; padding: 0.6rem 1.5rem; font-weight: 600; transition: all 0.3s; }
        .btn-gradient:hover { transform: translateY(-3px); box-shadow: 0 6px 20px rgba(102,126,234,0.4); color: white; }
        .accordion-item { border: none; margin-bottom: 12px; border-radius: 12px; overflow: hidden; box-shadow: 0 2px 12px rgba(0,0,0,0.06); }
        .accordion-button { font-weight: 600; padding: 1rem 1.5rem; background: #f8f9fa; border: none; }
        .accordion-button:not(.collapsed) { color: white; background: var(--primary-gradient); box-shadow: none; }
        .accordion-button:not(.collapsed)::after { filter: brightness(0) invert(1); }
        .accordion-body { padding: 1.5rem; background: white; border-top: 1px solid #e9ecef; }
    </style>

    <div class="container py-4">
        <div class="card-glass p-4 mb-4 text-center">
            <h1 class="display-4 fw-bold text-primary"><i class="bi bi-patch-question-fill me-2"></i>Frequently Asked Questions</h1>
            <p class="lead text-muted">Quick answers to common questions about EduTrack</p>
        </div>

        <div class="accordion" id="faqAccordion">
            <!-- Q1 -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading1"><button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#faq1" aria-expanded="true" aria-controls="faq1"><strong>What is EduTrack?</strong></button></h2>
                <div id="faq1" class="accordion-collapse collapse show" aria-labelledby="heading1" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">EduTrack is a comprehensive Learning Management System (LMS) designed for project-based learning. It helps teachers manage classes, assignments, and tests while providing students with an interactive platform to submit work, collaborate with peers, and track their academic progress. Administrators and parents also have dedicated dashboards to monitor performance and engagement.</div>
                </div>
            </div>

            <!-- Q2 -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading2"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#faq2" aria-expanded="false" aria-controls="faq2"><strong>Is EduTrack free to use?</strong></button></h2>
                <div id="faq2" class="accordion-collapse collapse" aria-labelledby="heading2" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">Yes! EduTrack is completely free for educational institutions. We believe in making quality education accessible to everyone. There are no hidden fees or subscription costs – it's our commitment to supporting educators and students worldwide.</div>
                </div>
            </div>

            <!-- Q3 -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading3"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#faq3" aria-expanded="false" aria-controls="faq3"><strong>Can I use EduTrack on mobile devices?</strong></button></h2>
                <div id="faq3" class="accordion-collapse collapse" aria-labelledby="heading3" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">Yes! EduTrack is fully responsive and works perfectly on smartphones, tablets, and desktop computers. Whether you're at home, in the classroom, or on the go, you can access all features from any device with an internet connection.</div>
                </div>
            </div>

            <!-- Q4 -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading4"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#faq4" aria-expanded="false" aria-controls="faq4"><strong>How secure is my data?</strong></button></h2>
                <div id="faq4" class="accordion-collapse collapse" aria-labelledby="heading4" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">We take security seriously. All data is encrypted using industry-standard protocols. We follow best practices for data protection and comply with educational privacy regulations. Your information is safe, private, and never shared with third parties without your explicit consent.</div>
                </div>
            </div>

            <!-- Q5 -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading5"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#faq5" aria-expanded="false" aria-controls="faq5"><strong>Can I export my grades and reports?</strong></button></h2>
                <div id="faq5" class="accordion-collapse collapse" aria-labelledby="heading5" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">Absolutely! You can export your grades, test results, attendance records, and comprehensive reports to PDF, Excel (CSV/XLSX), and Word formats. This makes it easy to keep records, share data with parents, or integrate with other systems.</div>
                </div>
            </div>

            <!-- Q6 -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading6"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#faq6" aria-expanded="false" aria-controls="faq6"><strong>How do I get support?</strong></button></h2>
                <div id="faq6" class="accordion-collapse collapse" aria-labelledby="heading6" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">
                        You can reach our support team via multiple channels:
                        <ul>
                            <li><strong>Email:</strong> <a href="mailto:nyarkoakwasi36@gmail.com">nyarkoakwasi36@gmail.com</a></li>
                            <li><strong>Contact Form:</strong> <asp:HyperLink ID="hlContact" runat="server" NavigateUrl="~/Contact.aspx" CssClass="text-primary">Click here</asp:HyperLink></li>
                            <li><strong>Help Center:</strong> <asp:HyperLink ID="hlHelp" runat="server" NavigateUrl="~/Help.aspx" CssClass="text-primary">Browse guides</asp:HyperLink></li>
                            <li><strong>Phone:</strong> +233 54 371 3237</li>
                        </ul>
                        We strive to respond to all inquiries within 24 hours.
                    </div>
                </div>
            </div>

            <!-- Q7 - NEW -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading7"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#faq7" aria-expanded="false" aria-controls="faq7"><strong>How do I reset my password?</strong></button></h2>
                <div id="faq7" class="accordion-collapse collapse" aria-labelledby="heading7" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">Go to the Login page and click "Forgot Password?". Enter your registered email address. A password reset link will be sent to your inbox. Click the link and follow the instructions to create a new password. If you don't receive the email, check your spam folder or contact support.</div>
                </div>
            </div>

            <!-- Q8 - NEW -->
            <div class="accordion-item">
                <h2 class="accordion-header" id="heading8"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#faq8" aria-expanded="false" aria-controls="faq8"><strong>How do I submit an assignment?</strong></button></h2>
                <div id="faq8" class="accordion-collapse collapse" aria-labelledby="heading8" data-bs-parent="#faqAccordion">
                    <div class="accordion-body">Navigate to the Assignments page, select the assignment you want to submit, upload your file or enter your response, and click "Submit". You will receive a confirmation that your submission has been recorded. You can also view your submission status and any feedback provided by your teacher.</div>
                </div>
            </div>
        </div>

        <div class="card-glass p-4 mt-4 text-center">
            <h5><i class="bi bi-lightbulb text-warning me-2"></i>Didn't find your answer?</h5>
            <p>Contact us and we'll be happy to help!</p>
            <asp:HyperLink ID="hlContactSupport" runat="server" NavigateUrl="~/Contact.aspx" CssClass="btn btn-gradient"><i class="bi bi-envelope me-2"></i>Contact Support</asp:HyperLink>
        </div>
    </div>
</asp:Content>