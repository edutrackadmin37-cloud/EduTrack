<%@ Page Title="Help Center" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Help.aspx.cs" Inherits="EduTrack.Help" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
        .help-icon { width: 50px; height: 50px; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-size: 1.5rem; background: var(--primary-gradient); color: white; flex-shrink: 0; }
    </style>

    <div class="container py-4">
        <div class="card-glass p-4 mb-4 text-center">
            <h1 class="display-4 fw-bold text-primary"><i class="bi bi-question-circle-fill me-2"></i>Help Center</h1>
            <p class="lead text-muted">Find answers to common questions and support options</p>
        </div>

        <!-- Getting Started -->
        <div class="card-glass p-4 mb-4">
            <h4 class="text-primary"><i class="bi bi-rocket-takeoff me-2"></i>Getting Started</h4>
            <div class="accordion" id="gettingStarted">
                <div class="accordion-item">
                    <h2 class="accordion-header" id="gs1"><button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapseGs1" aria-expanded="true"><strong>How do I create an account?</strong></button></h2>
                    <div id="collapseGs1" class="accordion-collapse collapse show" data-bs-parent="#gettingStarted">
                        <div class="accordion-body">
                            <ol><li>Click on "Register" in the navigation menu.</li><li>Fill in your details (Full Name, Email, Password).</li><li>Select your role (Student, Teacher, or Parent).</li><li>Click "Register". Your account will be created.</li><li>Your account will remain pending until approved by an administrator.</li><li>After approval, you will receive a notification and can return to the Login page to sign in.</li></ol>
                        </div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="gs2"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseGs2" aria-expanded="false"><strong>How do I reset my password?</strong></button></h2>
                    <div id="collapseGs2" class="accordion-collapse collapse" data-bs-parent="#gettingStarted">
                        <div class="accordion-body">
                            <ol><li>Go to the Login page.</li><li>Click on "Forgot Password?".</li><li>Enter your registered email address.</li><li>If your account is approved, a password reset link will be sent to your email.</li><li>Click the link in the email and follow the instructions to create a new password.</li><li>If you don't see the email, check your spam folder.</li></ol>
                        </div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="gs3"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseGs3" aria-expanded="false"><strong>How do I update my profile?</strong></button></h2>
                    <div id="collapseGs3" class="accordion-collapse collapse" data-bs-parent="#gettingStarted">
                        <div class="accordion-body">
                            <ol><li>Log in to your account.</li><li>Click on your profile picture or name in the top right corner.</li><li>Select "Profile" from the dropdown menu.</li><li>Update your personal information (name, phone, bio, etc.).</li><li>Click "Update Profile" to save your changes.</li></ol>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- For Students -->
        <div class="card-glass p-4 mb-4">
            <h4 class="text-success"><i class="bi bi-person me-2"></i>For Students</h4>
            <div class="accordion" id="studentHelp">
                <div class="accordion-item">
                    <h2 class="accordion-header" id="st1"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSt1"><strong>How do I submit an assignment?</strong></button></h2>
                    <div id="collapseSt1" class="accordion-collapse collapse" data-bs-parent="#studentHelp">
                        <div class="accordion-body"><ol><li>Go to the Assignments page from your dashboard.</li><li>Select the assignment you want to submit.</li><li>Upload your file or enter your response in the provided field.</li><li>Add any optional remarks if needed.</li><li>Click "Submit".</li><li>You will see a confirmation that your work has been recorded. You can also check your submission status later.</li></ol></div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="st2"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSt2"><strong>How do I take a test?</strong></button></h2>
                    <div id="collapseSt2" class="accordion-collapse collapse" data-bs-parent="#studentHelp">
                        <div class="accordion-body"><ol><li>Go to the Tests page.</li><li>Select the available test from the list.</li><li>Read the instructions carefully.</li><li>Answer the questions (MCQ, True/False, Fill-in, or Essay).</li><li>Submit the test before the deadline.</li><li>Your results will be available after the teacher grades it.</li></ol></div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="st3"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSt3"><strong>How do I check my grades?</strong></button></h2>
                    <div id="collapseSt3" class="accordion-collapse collapse" data-bs-parent="#studentHelp">
                        <div class="accordion-body"><ol><li>Open your dashboard or go to the Grades page.</li><li>Review graded assignments, tests, and projects.</li><li>Click on any grade to see detailed feedback and rubric scores.</li><li>Check your overall averages and performance summaries to track your progress.</li></ol></div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="st4"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseSt4"><strong>How do I join a team for a project?</strong></button></h2>
                    <div id="collapseSt4" class="accordion-collapse collapse" data-bs-parent="#studentHelp">
                        <div class="accordion-body"><ol><li>Go to the Projects page.</li><li>Select the project you are interested in.</li><li>If team formation is open, you will see an option to join or create a team.</li><li>Join an existing team or create a new one with your classmates.</li><li>Your teacher must approve team formations if required.</li></ol></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- For Teachers -->
        <div class="card-glass p-4 mb-4">
            <h4 class="text-warning"><i class="bi bi-briefcase me-2"></i>For Teachers</h4>
            <div class="accordion" id="teacherHelp">
                <div class="accordion-item">
                    <h2 class="accordion-header" id="th1"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTh1"><strong>How do I create a project?</strong></button></h2>
                    <div id="collapseTh1" class="accordion-collapse collapse" data-bs-parent="#teacherHelp">
                        <div class="accordion-body"><ol><li>Go to the Projects page.</li><li>Click on "New Project" or "Create Project".</li><li>Enter the project title, description, objectives, and timeline.</li><li>Set the maximum team size and select the class and subject.</li><li>Choose the initial status (e.g., Draft, Proposal Submitted).</li><li>Save the project. You can add teams and assignments later.</li></ol></div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="th2"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTh2"><strong>How do I grade submissions?</strong></button></h2>
                    <div id="collapseTh2" class="accordion-collapse collapse" data-bs-parent="#teacherHelp">
                        <div class="accordion-body"><ol><li>Open the Submissions page for the relevant assignment.</li><li>Select a student submission.</li><li>Apply the rubric criteria or enter grades/comments directly.</li><li>Save the grading result.</li><li>Students will receive immediate feedback and updated grades.</li></ol></div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="th3"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapseTh3"><strong>How do I mark attendance?</strong></button></h2>
                    <div id="collapseTh3" class="accordion-collapse collapse" data-bs-parent="#teacherHelp">
                        <div class="accordion-body"><ol><li>Go to the Attendance page.</li><li>Select your class and the date.</li><li>Click "Load Students" to see the roster.</li><li>Mark each student as Present, Absent, Late, or Excused.</li><li>Click "Save Attendance" to record the attendance for that session.</li></ol></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- For Parents -->
        <div class="card-glass p-4 mb-4">
            <h4 class="text-danger"><i class="bi bi-person-lines-fill me-2"></i>For Parents</h4>
            <div class="accordion" id="parentHelp">
                <div class="accordion-item">
                    <h2 class="accordion-header" id="ph1"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapsePh1"><strong>How do I link to my child's account?</strong></button></h2>
                    <div id="collapsePh1" class="accordion-collapse collapse" data-bs-parent="#parentHelp">
                        <div class="accordion-body"><ol><li>Log in to your parent account.</li><li>Go to the Parent Dashboard.</li><li>Click on "Link Child" or "Add Student".</li><li>Enter your child's student ID or email.</li><li>The system will send a request to the student to confirm the link.</li><li>Once confirmed, you will see your child's data on your dashboard.</li></ol></div>
                    </div>
                </div>
                <div class="accordion-item">
                    <h2 class="accordion-header" id="ph2"><button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapsePh2"><strong>What can I see on my dashboard?</strong></button></h2>
                    <div id="collapsePh2" class="accordion-collapse collapse" data-bs-parent="#parentHelp">
                        <div class="accordion-body">Your dashboard provides a comprehensive overview of your child's academic progress. You can view their grades, attendance records, project participation, engagement levels, and recent notifications. You can also communicate directly with teachers through the messaging system.</div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Support -->
        <div class="card-glass p-4 text-center">
            <h5><i class="bi bi-headset me-2"></i>Still Need Help?</h5>
            <p class="mb-2">Contact our support team at <asp:HyperLink ID="hlSupportEmail" runat="server" CssClass="fw-bold text-decoration-none"></asp:HyperLink></p>
            <p class="mb-3">Or send us a message using the contact page.</p>
            <asp:HyperLink ID="hlContactUs" runat="server" NavigateUrl="~/Contact.aspx" CssClass="btn btn-gradient"><i class="bi bi-envelope me-2"></i>Contact Us</asp:HyperLink>
        </div>
    </div>
</asp:Content>